using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PMSProject.Data;
using PMSProject.Models;
using System.Diagnostics;

namespace PMSProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger , AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public async Task<IActionResult> Dashboard()
        {
            
            var allTasks = await _context.Tasks
                .Include(p => p.Project)
                      .ThenInclude(p => p.ParentProject)
                .Include(t => t.AssignedUser)
                .ToListAsync();

            var allProjects = await _context.Projects
                .Include(p => p.AssignedUser)
                .Include(pa => pa.ParentProject)
                .Include(s=>s.SubProjects)
                .ToListAsync();
            List<TaskModel> filteredTasks = new List<TaskModel>();
            List<ProjectModel> filteredProjects = new List<ProjectModel>();
            
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
               
                if (User.IsInRole("Admin") || User.IsInRole("TechLead"))
                {
                    filteredTasks = allTasks;
                    filteredProjects = allProjects;
                }
                else
                {
                    
                    var user = await _userManager.GetUserAsync(User);
                    if (user != null)
                    {
                        filteredTasks = allTasks
                            .Where(t => t.UserId  == user.Id)
                            .ToList();
                        filteredProjects = allProjects
                            .Where(t => t.UserId == user.Id)
                            .ToList();
                    }
                }
            }
          
            var model = new TaskAndProjectViewModel
            {
                Tasks = filteredTasks,
                Projects = filteredProjects
            };
            //show the developers only
            var developers = await _userManager.GetUsersInRoleAsync("Developer");
            ViewBag.Users = developers;
            ViewBag.TaskList = allTasks;
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();
            return Json(project);
        }
        [HttpGet]
        public async Task<IActionResult> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();
            return Json(task);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProject(int id, ProjectModel project, string selectedUserId)
        {
            if (id != project.Id) return NotFound();
            ModelState.Remove("AssignedUser");
            ModelState.Remove("UserId");
            if (ModelState.IsValid)
            {
                project.UserId = selectedUserId;
                project.AssignedUser = !string.IsNullOrEmpty(selectedUserId) ? await _userManager.FindByIdAsync(selectedUserId) : null;
                _context.Update(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Dashboard));
            }
            ViewBag.Users = new SelectList(await _userManager.GetUsersInRoleAsync("Developer"), "Id", "UserName", selectedUserId);
                return RedirectToAction(nameof(Dashboard));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTask(int id, TaskModel task, string selectedUserId)
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
                return RedirectToAction(nameof(Dashboard));
            }
            ViewBag.Users = new SelectList(await _userManager.GetUsersInRoleAsync("Developer"), "Id", "UserName", selectedUserId);
            return RedirectToAction(nameof(Dashboard));


        }
        [HttpPost]
        public async Task<IActionResult> CreateTaskFromDashboard(TaskModel taskModel)
        {
            if (ModelState.IsValid)
            {
                _context.Tasks.Add(taskModel);
                await _context.SaveChangesAsync();
            }
           
            return RedirectToAction("Dashboard");
        }


    }
}
