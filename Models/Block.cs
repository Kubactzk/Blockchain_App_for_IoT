using System.ComponentModel.DataAnnotations;
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

        public Block(DateTime Timestamp, string Data, int Difficulty, string PreviousHash = "") {
            this.Timestamp = Timestamp;
            this.Data = Data;
            this.PreviousHash = PreviousHash;
            this.Difficulty = Difficulty;
            Hash = GenerateHash();
        }

        public string GenerateHash()
        {
            // Serialize data into JSON for consistent hashing
            string jsonData = JsonSerializer.Serialize(Data).ToString();

            // Generate hash using SHA256
            using (var sha256 = SHA256.Create())
            {
                var input = $"{Id}{Timestamp}{jsonData}{PreviousHash}{Difficulty}";
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
