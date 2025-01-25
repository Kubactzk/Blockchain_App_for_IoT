namespace IoTBlockchain.Models
{
    public class Blockchain
    {
        public int Id { get; set; }
        public List<Block> Chain { get; set; }

        public Blockchain() {
            Chain.Add(new Block(DateTime.Now, "Genesis Block", 2, "0"));
        }

        public Block getLatestBlock()
        {
            return Chain.Last();
        }

        public void addBlock(Block block)
        {
            block.PreviousHash = this.getLatestBlock().Hash;
            block.Hash = block.GenerateHash();
            Chain.Add(block);
        }
    }
}
