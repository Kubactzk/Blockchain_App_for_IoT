using IoTBlockchain.Models;
using Microsoft.AspNetCore.Mvc;
using System.CodeDom.Compiler;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IoTBlockchain.Controllers
{
    public class BlockController : Controller
    {
        private AppDbContext _context;
        private Random random = new Random();
        private static List<Data> dataBuffer = new List<Data>();

        public BlockController(AppDbContext context)
        {
            _context = context;
            dataBuffer = _context.Datas.ToList();
            if (!_context.Blocks.Any())
            {
                var genesisBlock = new Block(DateTime.Now, "Genesis Block", 2, "0");
                _context.Blocks.Add(genesisBlock);
                _context.SaveChanges();
            }
        }
        public IActionResult Index(int? difficulty)
        {
            ViewData["Difficulty"] = difficulty ?? 1; // Default difficulty to 1 if not provided
            return View(dataBuffer);
        }


        [HttpPost]
        [HttpPost]
        public IActionResult GenerateData(int difficulty)
        {
            ViewData["Difficulty"] = difficulty; 

            var data = new Data
            {
                From = $"Device_{random.Next(1, 100)}",
                To = $"Gate_1",
                Humidity = (float)(random.NextDouble() * 100),
                Temp = (float)(random.NextDouble() * 40 + 273)
            };

            var gate = _context.Keys.FirstOrDefault(k => k.GateName == "Gate_1");
            if (gate == null)
            {
                gate = new KeyGeneratorModel().GenerateKey("Gate_1");
                _context.Keys.Add(gate);
                _context.SaveChanges();
            }
            data.To = gate.PublicKey;

            if (dataBuffer.Count > 10)
            {
                string blockData = JsonSerializer.Serialize(dataBuffer).ToString();
                var latestBlock = _context.Blocks.OrderByDescending(b => b.BlockNumber).FirstOrDefault();
                var previousHash = latestBlock?.Hash ?? "0";
                int blockNum = (latestBlock?.BlockNumber ?? 0) + 1;

                var newBlock = new Block(DateTime.Now, blockData, difficulty, previousHash, blockNum);

                using (RSA rsa = RSA.Create())
                {
                    rsa.ImportRSAPrivateKey(Convert.FromBase64String(gate.PrivateKey), out _);
                    byte[] signedData = rsa.SignData(Encoding.UTF8.GetBytes(blockData), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                    newBlock.DataSignature = Convert.ToBase64String(signedData);
                }

                _context.Blocks.Add(newBlock);

                foreach (Data dt in dataBuffer)
                {
                    _context.Datas.Remove(dt);
                }
            }

            _context.Datas.Add(data);
            _context.SaveChanges();

            return RedirectToAction("Index", new { difficulty });
        }
    }
}
