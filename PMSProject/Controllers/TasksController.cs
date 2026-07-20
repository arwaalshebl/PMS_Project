using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMSProject.Data;
using PMSProject.Models;
public class TasksController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public TasksController(AppDbContext context , UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
  

    public async Task<IActionResult> IndexT()
    {
        
        var tasks = await _context.Tasks
             .Include(p => p.projects)
             .Include(d=>d.AssignedUser)
             .ToListAsync();
        // System.Diagnostics.Debug.WriteLine("number of tasks:" + tasks.Count);
        if (User.Identity != null)
        {
            if (User.IsInRole("Admin") || User.IsInRole("TechLead"))
            {
                return View(tasks);

            }
            else
            {
                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    var usertasks = tasks.Where(d => d.AssignedUser.Any(u => u.Id == user.Id)).ToList();

                    return View(usertasks);
                }
            }
        }
        //without any rols
        return View(new List<TaskModel>());
    }

    public async Task<IActionResult> CreateT()

    {

       
        // SHOW ALL PROJECT
        ViewBag.ProjectsList = _context.Projects.ToList();
        //show the developers only
        var developers = await _userManager.GetUsersInRoleAsync("Developer");
        ViewBag.Users = developers;




        return View();

    }

    [HttpPost]

    [ValidateAntiForgeryToken]

    public async Task<IActionResult> CreateT(TaskModel task, List<int> selectedProjectIds , List<string> selectedUsersIds )

    {
        ModelState.Remove("Projects");

    
        if (ModelState.IsValid)

        {

 


            //connect task with project
            // bcs m - m you can choose more than one project
            if (selectedProjectIds != null)

            {

                task.projects = _context.Projects

                    .Where(p => selectedProjectIds.Contains(p.Id))

                    .ToList();



            }

            //show developers list to choose
            var selectedUsers = _context.Users.Where(u => selectedUsersIds.Contains(u.Id)).ToList();
            task.AssignedUser = selectedUsers;




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
    public IActionResult DeleteT(int id)
    {
        var task = _context.Tasks

            .FirstOrDefault(p => p.Id == id);
        if (task == null) return NotFound();


        _context.Tasks.Remove(task);
        _context.SaveChanges();
        return RedirectToAction("IndexT");


    }

}