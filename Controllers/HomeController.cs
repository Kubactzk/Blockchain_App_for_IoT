using IoTBlockchain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text;
using System.Security.Cryptography;

namespace IoTBlockchain.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Fetch all blocks from the database
            var blocks = _context.Blocks.ToList();
            foreach (var block in blocks)
            {
                block.mineBlock();
            }

            // Pass blocks to the view
            return View(blocks);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult CheckBlockchainValidity()
        {
            // Fetch all blocks from the database
            List<Block> blockchain = _context.Blocks.ToList();

            // Iterate over the blockchain starting from the second block
            for (int i = 1; i < blockchain.Count; i++)
            {
                Block currentBlock = blockchain[i];
                Block previousBlock = blockchain[i - 1];

                if(previousBlock.Hash != currentBlock.PreviousHash)
                {
                    currentBlock.IsValid = false;
                    _context.Update(currentBlock);
                }
                else
                {
                    currentBlock.IsValid = true;
                    _context.Update(currentBlock);
                }
            }
            _context.SaveChanges();

            // Return to the view with the updated blockchain
            var updatedBlocks = _context.Blocks.ToList();
            return View("Index", updatedBlocks);
        }


        public IActionResult VerifySignatures()
        {
            // Fetch all blocks
            var blocks = _context.Blocks.ToList();
            blocks.RemoveAt(0);

            foreach (var block in blocks)
            {
                // Get the public key for the gate
                var gateKey = _context.Keys.FirstOrDefault(k => k.GateName == "Gate_1");

                if (gateKey != null && block.BlockNumber != 0) // Skip genesis block
                {
                    block.IsValid = block.VerifySignature(gateKey.PublicKey);
                }
                else
                {
                    block.IsValid = false; // Mark as invalid if no key or genesis block
                }

                // Update block validity status
                _context.Update(block);
            }

            // Save changes to the database
            _context.SaveChanges();

            return RedirectToAction("Index");
        }



    }
}
