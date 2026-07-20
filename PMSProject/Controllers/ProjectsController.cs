using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Utilities;
using Microsoft.EntityFrameworkCore;
using PMSProject.Data; 
using PMSProject.Models;
namespace PMSProject.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProjectsController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        


        public async Task<IActionResult> IndexP()
        {
            var projects = await _context.Projects
                .Include(d =>d.AssignedUser)
                .ToListAsync();
          if(User.Identity != null)
            {
                if (User.IsInRole("Admin") || User.IsInRole("TechLead"))
                {
                    return View(projects);

                }
                else
                {
                    var user = await _userManager.GetUserAsync(User);

                   if (user != null)
                    {
                        var userprojects = projects.Where(d => d.AssignedUser.Any(u => u.Id == user.Id)).ToList();

                        return View(userprojects);

                    }
                }

            }

          //without any rols

          return View(new List<ProjectModel>());

        }

        
        public async Task<IActionResult> CreateP()
        {
            //show the developers only
            var developers = await _userManager.GetUsersInRoleAsync("Developer");
            ViewBag.Users = developers;

            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateP(ProjectModel project , List<string> selectedUsersIds)
        {
            if (ModelState.IsValid)
            {
 
                //show developers list to choose
                // bcs m - m you can choose more than one project

                var selectedUsers = _context.Users.Where(u => selectedUsersIds.Contains(u.Id)).ToList();
                project.AssignedUser = selectedUsers;


                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexP)); 
            }
            return View(project); 
        }
    }
}