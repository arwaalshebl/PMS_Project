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
             .Include(p => p.Project)
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

        ViewBag.SprintsList = new SelectList(_context.Sprints.ToList(), "Id", "SprintName");




        return View();

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateT(TaskModel task, int?projectId, string? selectedUserId , int? sprint)
    {
        ModelState.Remove("projects");
        ModelState.Remove("AssignedUser");

        if (ModelState.IsValid)
        {
            if(projectId.HasValue)
            {
                task.ProjectId= projectId.Value;
            }
            else
            {
                task.ProjectId = null; //task without project
            }

           
            if (! string.IsNullOrEmpty(selectedUserId))
            {
                

                var selectedUser = await _userManager.FindByIdAsync(selectedUserId);
                    if (selectedUser != null)
                {
                    task.AssignedUser = selectedUser;

                    task.UserId = selectedUserId;
                }
   
            }

            _context.Add(task);
            await _context.SaveChangesAsync();
            return RedirectToAction("Dashboard", "Home");
        }
        else
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            foreach (var error in errors) 
            {
                System.Diagnostics.Debug.WriteLine("---------MODEL STATE ERROR:-------   "+error);
            }
        }
        
        ViewBag.Users = new SelectList(await _userManager.GetUsersInRoleAsync("Developer"), "Id", "UserName");
        ViewBag.ProjectsList = new SelectList(_context.Projects.ToList(), "Id", "ProjectName");
        ViewBag.SprintsList = new SelectList(_context.Sprints.ToList(), "Id", "SprintName");

        return View(task);
    }

    public IActionResult DeleteT(int id)
    {
        var task = _context.Tasks

            .FirstOrDefault(p => p.Id == id);
        if (task == null) return NotFound();


        _context.Tasks.Remove(task);
        _context.SaveChanges();
        return RedirectToAction("Dashboard","Home");


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
        // 1. نبحث عن المشروع المطلوب مع جلب المطور المرتبط به مباشرة
        var project = _context.Projects
            .Where(p => p.Id == projectId)
            .Select(p => p.AssignedUser) // أو p.User حسب اسم المودل عندك للمطور
            .FirstOrDefault();
        // 2. إذا لم يكن هناك مطور مرتبط بهذا المشروع، نرجع لستة فاضية

        if (project == null)
        {

            return Json(new List<object>());
        }
        // 3. نضع المطور في لستة (لأن الجافاسكريبت يتوقع لستة options) ونرجعه
        var developers = new List<object>
   {
       new { id = project.Id, userName = project.UserName }
   };
        return Json(developers);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditT(int id, TaskModel task, string? selectedUserId)
    {
        if (id != task.Id) return NotFound();
        ModelState.Remove("AssignedUser");
        ModelState.Remove("UserId");
        if (ModelState.IsValid)
        {
            task.UserId = selectedUserId;
            task.AssignedUser = !string.IsNullOrEmpty(selectedUserId) ? await _userManager.FindByIdAsync(selectedUserId) : null;
            _context.Update(task);
            await _context.SaveChangesAsync();
            return RedirectToAction("Dashboard", "Home");
        }
        ViewBag.Users = new SelectList(await _userManager.GetUsersInRoleAsync("Developer"), "Id", "UserName", selectedUserId);
        return RedirectToAction("Dashboard", "Home");


    }

}