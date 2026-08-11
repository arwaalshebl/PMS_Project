using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PMSProject.Data;
using PMSProject.Models;
using System.Diagnostics;
using System.Text.Json;

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
                .Include(s=>s.Sprint)
                .ToListAsync();

            var allProjects = await _context.Projects
                .Include(p => p.AssignedUser)
                .Include(pa => pa.ParentProject)
                .Include(s=>s.SubProjects)
                .ToListAsync();
            var sprints = await _context.Sprints.ToListAsync();

            var members= await _userManager.Users.ToListAsync();


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
            var directTasks = filteredTasks.Where(t => t.ProjectId == null).ToList();
            var projectTasks = filteredTasks.Where(t => t.ProjectId != null).ToList();

            var model = new TaskAndProjectViewModel
            {
                Tasks = filteredTasks,
                Projects = filteredProjects,
                Sprints = sprints,
                DirectTasks = directTasks,
                ProjectTasks = projectTasks,
                Users=members

            };

            ViewBag.Users = members;
            ViewBag.TaskList = allTasks;
            ViewBag.SprintsList = _context.Sprints.ToList();

            ViewBag.ParentProjects = allProjects.Where(pp=>pp.ParentProjectID == null).ToList();
            
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

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users 
                .Select(u => new { id = u.Id, userName = u.UserName })
                .ToListAsync();
            return Json(users);
        }
        [HttpGet]
        public async Task<IActionResult> GetSprint(int id)
        {
            var sprint = await _context.Sprints.FindAsync(id);
            if (sprint == null) return NotFound();
            return Json(sprint);
            
        }
        public async Task<IActionResult> DetailsUsers(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            // project
            var projects = await _context.Projects
                .Where(p => p.UserId == id)
                .ToListAsync();

            //direct tasks table
            var directTasks = await _context.Tasks
                .Where(t => t.UserId == id && t.ProjectId == null)
                .ToListAsync();

            //project tasks table
            var projectTasks = await _context.Tasks
                .Where(t => t.UserId == id && t.ProjectId != null)
                .ToListAsync();

            int complatedTasksCount = _context.Tasks
                .Where(t=>t.UserId == id 
                && t.Status == PMSProject.Models.TaskStatus.Done 
                & t.Project != null)
                .Count();

            ViewBag.CompletedCount = complatedTasksCount;

            var viewModel = new TaskAndProjectViewModel
            {
                User = user,
                Projects = projects,
                DirectTasks = directTasks,
                ProjectTasks = projectTasks
            };
            return View(viewModel);

        }


    }
}
