using IoTBlockchain.Models;
using Microsoft.AspNetCore.Mvc;
using System.CodeDom.Compiler;
using System.Text.Json;

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


        public IActionResult Index()
        {
            return View(dataBuffer);
        }

        public IActionResult generateData()
        {
            // Generating random values for the Data model
            
            var data = new Data
            {
                From = $"Device_{random.Next(1, 100)}",  // Random device name like Device_1, Device_2...
                To = $"Gate_{random.Next(1, 5)}",    // Random device name for "To"
                Humidity = (float)(random.NextDouble() * 100),  // Random float for humidity (0-100)
                Temp = (float)(random.NextDouble() * 40 + 273)  // Random float for temperature (0-40)
            };

            if (dataBuffer.Count > 10)
            {
                var blockData = JsonSerializer.Serialize(dataBuffer);
                //latest block
                var latestBlock = _context.Blocks.OrderByDescending(b => b.Id).FirstOrDefault();

                //if no previous hash
                var previousHash = latestBlock?.Hash ?? "0";

                var newBlock = new Block(DateTime.Now, blockData, 2, previousHash);

                //add new block
                _context.Blocks.Add(newBlock);
                foreach(Data dt in dataBuffer)
                {
                    _context.Datas.Remove(dt);
                }
                
            }
            // Adding the generated data to the database

            _context.Datas.Add(data);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
