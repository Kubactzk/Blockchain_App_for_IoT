using System.ComponentModel.DataAnnotations;

namespace IoTBlockchain.Models
{
    public class Data
    {
        [Key]
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public float Humidity { get; set; }
        public float Temp { get; set; }

    }
}
