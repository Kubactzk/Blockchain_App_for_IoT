using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace IoTBlockchain.Models
{
    public class Block
    {
        [Key]
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Data { get; set; }
        public string PreviousHash { get; set; }
        public int Difficulty { get; set; }
        public string Hash { get; set; }

        public int BlockNumber { get; set; }
        public bool IsValid { get; set; }
        public int Nonce { get; set; } = 0;
        public string DataSignature { get; set; } = "0";
        public Block(DateTime Timestamp, string Data, int Difficulty, string PreviousHash = "", int BlockNumber = 0, bool IsValid = true) {
            this.Timestamp = Timestamp;
            this.Data = Data;
            this.PreviousHash = PreviousHash;
            this.Difficulty = Difficulty;
            Hash = mineBlock();
            this.IsValid = IsValid;
            this.BlockNumber = BlockNumber;
        }

        public string GenerateHash()
        {
            string formattedTimestamp = Timestamp.ToString("yyyy-MM-ddTHH:mm:ss.fff");

            string input = $"{BlockNumber}{Timestamp}{Data}{DataSignature}{PreviousHash}{Nonce}";

            // Generate hash using SHA256
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(bytes);
            }
        }

        public string mineBlock()
        {
            string myHash;
            do
            {
                Nonce++;
                myHash = GenerateHash();
            }
            while (!myHash.StartsWith(new string('0', Difficulty)));

            return myHash;
        }

        public bool VerifySignature(string publicKey)
        {
            try
            {
                using (RSA rsa = RSA.Create())
                {
                    // Import the public key
                    rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);

                    // Verify the signature
                    byte[] dataBytes = Encoding.UTF8.GetBytes(Data);
                    byte[] signatureBytes = Convert.FromBase64String(DataSignature);

                    return rsa.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                }
            }
            catch
            {
                return false; // Return false if verification fails
            }
        }


    }
}
