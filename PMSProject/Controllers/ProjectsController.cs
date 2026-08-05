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


            var users = await _userManager.Users.ToListAsync();
            ViewBag.Users = new SelectList(users, "Id", "UserName");
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
        

            var users = await _userManager.Users.ToListAsync();
            ViewBag.Users = new SelectList(users, "Id", "UserName");
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
        [HttpGet]
        public async Task<IActionResult> DetailsP(int id)
        {
           
            var project = await _context.Projects
                .Include(p => p.AssignedUser)
                .Include(p => p.Tasks)
                    .ThenInclude(t => t.Sprint)
                    //here to get every task with his dev ==> later do when the statuse done put the dev name
                .Include(p => p.Tasks)
                    .ThenInclude(t => t.AssignedUser)
                    //هنا اشوف هل العلاقه هذي تجيب لي المهام حتى لو كان المطورين الي مسكوا المشروع مختلفين او اسوي له انكلود من جدول البروجكت 
                    
                .FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }
            var history = await _context.PublishHistory
                .Where(h=>h.ProjectId==id)
                .OrderByDescending(h=>h.PublishDate)
                .ToListAsync();
            var viewModel = new TaskAndProjectViewModel
            {
                Project = project,
                PublishHistories = history
            };
     
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditP(int id, ProjectModel project, string selectedUserId)
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
                return RedirectToAction("Dashboard", "Home");
            }
            var users = await _userManager.Users.ToListAsync();
            ViewBag.Users = new SelectList(users, "Id", "UserName", selectedUserId);
            return RedirectToAction("Dashboard", "Home");
        }

        [HttpGet]

        public IActionResult CreatePublish(int id)

        {

            // نمرر الـ ProjectId للـ View عشان نعرف النشرة تتبع لأي مشروع

            var model = new PublishHistoryModel

            {

                ProjectId = id,

                PublishDate = DateTime.Now // أو ReleaseDate حسب اسم الخاصية عندك

            };

            return View(model);

        }

        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePublish(PublishHistoryModel model)

        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                foreach (var error in errors)
                {
                    System.Diagnostics.Debug.WriteLine("---------MODEL STATE ERROR:-------   " + error);
                    //HERE print   The Project field is required.
                }
            }
             ModelState.Remove("Project");

            if (ModelState.IsValid)

            {  
                model.Id = 0;
                var testId=model.Id;

                _context.PublishHistory.Add(model);

                await _context.SaveChangesAsync();


                return RedirectToAction("DetailsP", new { id = model.ProjectId });

            }

            return View(model);

        }



    }
}