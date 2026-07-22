using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                    var usertasks = tasks.Where(d => d.UserId ==  user.Id).ToList();

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
       // ViewBag.ProjectsList = _context.Projects.ToList();
        ViewBag.ProjectsList = new SelectList(_context.Projects.ToList(),"Id","ProjectName");
        //show the developers only
        var developers = await _userManager.GetUsersInRoleAsync("Developer");
        ViewBag.Users =new SelectList(developers,"Id","UserName");




        return View();

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateT(TaskModel task, List<int> selectedProjectIds, string selectedUserId)
    {
        ModelState.Remove("projects");
        ModelState.Remove("AssignedUser");

        if (ModelState.IsValid)
        {
            // 1. ربط المشاريع
            if (selectedProjectIds != null && selectedProjectIds.Any())
            {
                task.projects = _context.Projects
                    .Where(p => selectedProjectIds.Contains(p.Id))
                    .ToList();
            }
            ///////
            //System.Diagnostics.Debug.WriteLine("-------Selected user count:  "+(selectedUsersIds?.Count??0));
            //if(selectedUsersIds != null && selectedUsersIds.Count >0)
            //{
            //    System.Diagnostics.Debug.WriteLine("-------fIRST USER ID:  " + selectedUsersIds[0]);

            //}
            //////
            //
            if (! string.IsNullOrEmpty(selectedUserId))
            {
                ///

                var selectedUser = await _userManager.FindByIdAsync(selectedUserId);
                    if (selectedUser != null)
                {
                    task.AssignedUser = selectedUser;

                    task.UserId = selectedUserId;
                }
   
            }

            _context.Add(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexT));
        }
        
        ViewBag.Users = new SelectList(await _userManager.GetUsersInRoleAsync("Developer"), "Id", "UserName");
        ViewBag.ProjectsList = new SelectList(_context.Projects.ToList(), "Id", "ProjectName");
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



    //[HttpGet]
    //public IActionResult GetProjectsByDeveloper(string userId)
    //{
    //    //project list for the selected devolper
    //    var projects = _context.Projects
    //        .Where(p => p.AssignedUser.Any(u => u.Id == userId))
    //        .Select(p => new { p.Id, p.ProjectName })
    //        .ToList();
    //    return Json(projects);
    //}

    [HttpGet]
    public IActionResult GetDevelopersByProject(int projectId)
    {
        //devloper list for the selected project
        var developers = _context.Projects
            .Where(p => p.Id == projectId)
            .Select(p => p.AssignedUser)
            .Select(u => new { u.Id, u.UserName })
            .Distinct()
            .ToList();
        return Json(developers);
    }

}