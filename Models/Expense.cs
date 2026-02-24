using EnglishCentralManagement.Models.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCentralManagement.Models
{
    public class Expense : BaseModel
    {
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public ExpenseCategory Category { get; set; }

        public ExpenseType Type { get; set; }
        // Fixed or Variable

        public DateTimeOffset ExpenseDate { get; set; }

        public long? ClassId { get; set; }

        public Class? Class { get; set; }

        public string? Note { get; set; }
    }
}
