using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace EnglishCentralManagement.Models
{
    public class Course : BaseModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Range(1, 24)]
        public int DurationInMonths { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 100000000)]
        public decimal MonthlyFee { get; set; }

        public ICollection<Class> Classes { get; set; } = new List<Class>();
    }
}
