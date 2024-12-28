using System.IO;
using System.Security.Cryptography;
using System;

namespace HelperLibrary
{
  public static class SecurityHelper
  {
    private const int KeySize = 256;
    private const int DerivationIterations = 1000;

    public static string Encrypt(string plainText)
    {
      try
      {
        // Générer un sel aléatoire
        byte[] salt = GenerateRandomBytes(32);
        byte[] iv = GenerateRandomBytes(16);

        // Dériver une clé à partir d'une phrase secrète
        string masterKey = GetMasterKey();
        using (var keyDerivation = new Rfc2898DeriveBytes(masterKey, salt, DerivationIterations, HashAlgorithmName.SHA256))
        {
          byte[] key = keyDerivation.GetBytes(KeySize / 8);

          using (var aes = Aes.Create())
          {
            aes.KeySize = KeySize;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = key;
            aes.IV = iv;

            using (var msEncrypt = new MemoryStream())
            {
              // Format: salt (32) + iv (16) + hmac (32) + encrypted data
              msEncrypt.Write(salt, 0, salt.Length);
              msEncrypt.Write(iv, 0, iv.Length);

              // Réserver de l'espace pour le HMAC
              var hmacPosition = msEncrypt.Position;
              msEncrypt.Write(new byte[32], 0, 32);

              using (var encryptor = aes.CreateEncryptor())
              using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
              using (var swEncrypt = new StreamWriter(csEncrypt))
              {
                swEncrypt.Write(plainText);
              }

              // Calculer le HMAC
              var hmac = new HMACSHA256(key);
              var hash = hmac.ComputeHash(msEncrypt.ToArray(), salt.Length + iv.Length + 32, (int)msEncrypt.Length - (salt.Length + iv.Length + 32));

              // Écrire le HMAC après salt+iv
              msEncrypt.Position = hmacPosition;
              msEncrypt.Write(hash, 0, hash.Length);

              return Convert.ToBase64String(msEncrypt.ToArray());
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw new CryptographicException("Erreur lors du chiffrement", ex);
      }
    }

    public static string Decrypt(string cipherText)
    {
      try
      {
        byte[] cipherBytes = Convert.FromBase64String(cipherText);

        byte[] salt = new byte[32];
        byte[] iv = new byte[16];
        byte[] hmac = new byte[32];

        // Extraire salt, iv et hmac
        using (var ms = new MemoryStream(cipherBytes))
        {
          ms.Read(salt, 0, salt.Length);
          ms.Read(iv, 0, iv.Length);
          ms.Read(hmac, 0, hmac.Length);
        }

        // Dériver la clé
        string masterKey = GetMasterKey();
        using (var keyDerivation = new Rfc2898DeriveBytes(masterKey, salt, DerivationIterations, HashAlgorithmName.SHA256))
        {
          byte[] key = keyDerivation.GetBytes(KeySize / 8);

          // Vérifier le HMAC
          using (var hmacAlgo = new HMACSHA256(key))
          {
            var computedHmac = hmacAlgo.ComputeHash(cipherBytes, salt.Length + iv.Length + 32, cipherBytes.Length - (salt.Length + iv.Length + 32));
            if (!CompareBytes(hmac, computedHmac))
            {
              throw new CryptographicException("Les données ont été altérées");
            }
          }

          using (var aes = Aes.Create())
          {
            aes.KeySize = KeySize;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = key;
            aes.IV = iv;

            using (var msDecrypt = new MemoryStream(cipherBytes, salt.Length + iv.Length + 32, cipherBytes.Length - (salt.Length + iv.Length + 32)))
            using (var decryptor = aes.CreateDecryptor())
            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (var srDecrypt = new StreamReader(csDecrypt))
            {
              return srDecrypt.ReadToEnd();
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw new CryptographicException("Erreur lors du déchiffrement", ex);
      }
    }

    private static byte[] GenerateRandomBytes(int length)
    {
      var randomBytes = new byte[length];
      using (var rng = new RNGCryptoServiceProvider())
      {
        rng.GetBytes(randomBytes);
      }
      return randomBytes;
    }

    private static bool CompareBytes(byte[] a, byte[] b)
    {
      if (a.Length != b.Length) return false;
      uint diff = 0;
      for (int i = 0; i < a.Length; i++)
      {
        diff |= (uint)(a[i] ^ b[i]);
      }
      return diff == 0;
    }

    private static string GetMasterKey()
    {
      // Dans un environnement de production, cette clé devrait être :
      // 1. Stockée de manière sécurisée (ex: Azure Key Vault, AWS KMS)
      // 2. Unique par environnement
      // 3. Changée régulièrement
      // 4. Au moins 256 bits d'entropie
      return Environment.GetEnvironmentVariable("MASTER_KEY") ??
             throw new CryptographicException("La clé maître n'est pas configurée");
    }
  }
}
