using System.ComponentModel.DataAnnotations;

namespace CipatBarzel.Models
{
    public class ThreatAmmunition
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Speed { get; set; }
    }
}
