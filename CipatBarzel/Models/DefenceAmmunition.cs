using System.ComponentModel.DataAnnotations;

namespace CipatBarzel.Models
{
    public class DefenceAmmunition
    {
        [Key]
        public int Id { get; set; }
        [Display (Name = "סוג טיל")]
        public string Name { get; set; }
        [Display(Name = "כמות")]
        public int Amount { get; set; }
    }
}
