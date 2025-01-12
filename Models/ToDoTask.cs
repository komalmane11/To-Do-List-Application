using System.ComponentModel.DataAnnotations;

namespace To_Do_List_Application.Models
{
    public class ToDoTask
    {
        [Key] // This attribute marks TaskId as the primary key
        public int TaskId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(50)]
        public string Category { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; }
    }
}
