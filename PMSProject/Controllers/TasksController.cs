using Microsoft.AspNetCore.Mvc;
using PMSProject.Data;
using PMSProject.Models;
using Microsoft.EntityFrameworkCore;
public class TasksController : Controller
{
    private readonly AppDbContext _context;
    public TasksController(AppDbContext context)
    {
        _context = context;
    }
  
    public async Task<IActionResult> IndexT()
    {
        var tasks = await _context.Tasks
            .Include(p => p.projects)
            .ToListAsync();
        return View(tasks);
    }

    public IActionResult CreateT()

    {

       
        // SHOW ALL PROJECT
        ViewBag.ProjectsList = _context.Projects.ToList();

        return View();

    }

    [HttpPost]

    [ValidateAntiForgeryToken]

    public async Task<IActionResult> CreateT(TaskModel task, List<int> selectedProjectIds)

    {
        ModelState.Remove("Projects");

    
        if (ModelState.IsValid)

        {


            //connect task with project
            // bcs m - m you can choose more thwn one project
            if (selectedProjectIds != null)

            {

                task.projects = _context.Projects

                    .Where(p => selectedProjectIds.Contains(p.Id))

                    .ToList();

            }

            _context.Add(task);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(IndexT));

        }

        var errors = ModelState.Values.SelectMany(v => v.Errors);
        Console.WriteLine("--------Task Name:" + task.TaskName);
        Console.WriteLine("--------Selected projects count:" + (selectedProjectIds?.Count??0));



        ViewBag.ProjectsList = _context.Projects.ToList();

        return View(task);

    }

}