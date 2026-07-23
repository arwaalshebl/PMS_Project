using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                        var userprojects = projects.Where(d => d.UserId == user.Id).ToList();

                        return View(userprojects);

                    }
                }

            }

          //without any rols

          return View(new List<ProjectModel>());

        }

        
        public async Task<IActionResult> CreateP()
        {
            //show the project list to slecest the parent project
            ViewBag.ParentProjects = new SelectList(_context.Projects,"Id", "ProjectName");
            //show the developers only
            var developers = await _userManager.GetUsersInRoleAsync("Developer");
            ViewBag.Users = developers;

            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateP(ProjectModel project , string? selectedUserId , int? parentProjectId)
        {
            ModelState.Remove("AssignedUser");
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(selectedUserId))
                {
                    ///parent project
                    project.ParentProjectID = parentProjectId;
                    
                    /// dev
                    var selectedUser = await _userManager.FindByIdAsync(selectedUserId);
                    if (selectedUser != null)
                    {
                        

                        project.UserId = selectedUserId;
                        project.AssignedUser = selectedUser;
                    }


                }

                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard", "Home");
            }
            //show the project list to slecest the parent project
            ViewBag.ParentProjects = new SelectList(_context.Projects, "Id", "ProjectName", parentProjectId);
            //show the developers only
           // var developers = await _userManager.GetUsersInRoleAsync("Developer");
            ViewBag.Users = await _userManager.GetUsersInRoleAsync("Developer");
            return View(project); 
        }

        public IActionResult DeleteP(int id)
        {
            var project = _context.Projects

                .FirstOrDefault(p => p.Id == id);
            if (project == null) return NotFound();


            _context.Projects.Remove(project);
            _context.SaveChanges();
            return RedirectToAction("Dashboard", "Home");


        }
    }
}