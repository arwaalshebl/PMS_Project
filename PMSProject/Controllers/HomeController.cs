using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            
            var allTasks = await _context.Tasks.Include(t => t.AssignedUser).ToListAsync();
            var allProjects = await _context.Projects.Include(p => p.AssignedUser).ToListAsync();
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
            return View(model);
        }
    }
}
