using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

public class KeyGeneratorModel
{
    [Key]
    public int Id { get; set; }

    public string PublicKey { get; set; }

    public string PrivateKey { get; set; }
    public string GateName { get; set; }

    public KeyGeneratorModel GenerateKey(string gateName)
    {
        using (RSA rsa = RSA.Create(2048)) // Create RSA with a 2048-bit key
        {
            // Export the public and private keys to PEM format (Base64 encoded)
            var publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
            var privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());

            // Return a new KeyGeneratorModel with the public and private keys
            return new KeyGeneratorModel
            {
                PublicKey = publicKey,
                PrivateKey = privateKey,
                GateName = gateName
            };
        }
    }
}
