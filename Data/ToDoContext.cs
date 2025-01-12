using Microsoft.EntityFrameworkCore;
using To_Do_List_Application.Models; // Ensure this is at the top

namespace To_Do_List_Application.Data
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options) { }

        // Fully qualify the Task model to avoid ambiguity
        public DbSet<To_Do_List_Application.Models.ToDoTask> Tasks { get; set; }
    }
}
