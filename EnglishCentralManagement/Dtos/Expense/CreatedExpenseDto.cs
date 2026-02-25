using EnglishCentralManagement.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace EnglishCentralManagement.Dtos.Expense
{
    public class CreatedExpenseDto
    {
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public ExpenseCategory Category { get; set; }
        [Required]
        public ExpenseType Type { get; set; }
        [Required]
        public DateTimeOffset ExpenseDate { get; set; }

        public long? ClassId { get; set; }
        public long? Id { get; set; }
        public string? Note { get; set; }
    }
}
