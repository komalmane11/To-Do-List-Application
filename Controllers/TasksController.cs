using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using To_Do_List_Application.Data;
using To_Do_List_Application.Models;

namespace To_Do_List_Application.Controllers
{
    public class TasksController : Controller
    {
        private readonly ToDoContext _context;

        public TasksController(ToDoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search, string category)
        {
            // Store the selected category in session if provided
            if (!string.IsNullOrEmpty(category))
            {
                HttpContext.Session.SetString("SelectedCategory", category);  // Store the category in session
            }

            // Retrieve the stored category from the session
            var selectedCategory = HttpContext.Session.GetString("SelectedCategory");

            // Get all tasks from the database
            var tasks = _context.Tasks.AsQueryable();

            // Filter by search if provided
            if (!string.IsNullOrEmpty(search))
            {
                tasks = tasks.Where(t => t.Title.Contains(search) || t.Description.Contains(search));
            }

            // Filter by category if provided or if selected in session
            if (!string.IsNullOrEmpty(selectedCategory))
            {
                tasks = tasks.Where(t => t.Category == selectedCategory);
            }

            // Pass session data to the view using ViewBag
            ViewBag.SelectedCategory = selectedCategory;

            return View(await tasks.ToListAsync());
        }

        // Optional: Method to clear session data (if needed)
        public IActionResult ClearSession()
        {
            // Clear the session data if necessary
            HttpContext.Session.Clear();  // Clear all session data
            return RedirectToAction("Index");  // Redirect to the Index page after clearing the session
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Tasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskId,Title,Description,Category,DueDate,IsCompleted")] ToDoTask task)  // Updated to ToDoTask
        {
            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        // POST: Tasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaskId,Title,Description,Category,DueDate,IsCompleted")] ToDoTask task)  // Updated to ToDoTask
        {
            if (id != task.TaskId)  // Updated TaskId (not TaskID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.TaskId))  // Updated TaskId
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tasks == null)
            {
                return Problem("Entity set 'ToDoContext.Tasks'  is null.");
            }
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
            return (_context.Tasks?.Any(e => e.TaskId == id)).GetValueOrDefault();  // Updated TaskId
        }


        [HttpPost]
        public IActionResult ToggleComplete(int taskId, bool isCompleted)
        {
            var task = _context.Tasks.Find(taskId);
            if (task != null)
            {
                task.IsCompleted = isCompleted;
                _context.SaveChanges();
            }

            // Return the updated task list partial view
            return PartialView("_TaskListPartial", _context.Tasks.ToList());
        }

    }
}
