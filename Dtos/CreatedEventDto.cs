using System.ComponentModel.DataAnnotations;

namespace EnglishCentralManagement.Dtos
{
    public class CreatedEventDto
    {
        [Required]
        [MaxLength(500)]
        public string EventName { get; set; }

        [MaxLength(1000)]
        public string? LinkMeeting { get; set; }

        [Required]
        public DateOnly StartDate { get; set; }

        [Required]
        public DateOnly EndDate { get; set; }
        [Required]
        public TimeOnly StartTime { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }

        public bool? IsMonday { get; set; }
        public bool? IsTuesday { get; set; }
        public bool? IsWednesday { get; set; }
        public bool? IsThursday { get; set; }
        public bool? IsFriday { get; set; }
        public bool? IsSaturday { get; set; }
        public bool? IsSunday { get; set; }
        public long? StaffId { get; set; }
        public long? EventId { get; set; }
    }

    public class CalendarViewModel
    {
        public DateTime? SelectedDate { get; set; }
        public List<CreatedEventDto> Events { get; set; }
    }
}
