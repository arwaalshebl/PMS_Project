using Microsoft.AspNetCore.Identity;

namespace PMSProject.Models
{
    public class TaskAndProjectViewModel
    {
        public List<TaskModel> Tasks { get; set; }
        public List<TaskModel> DirectTasks { get; set; }

        public List<TaskModel> ProjectTasks{ get; set; }


        public List<ProjectModel> Projects { get; set; } 

        public List<SprintModel> Sprints { get; set; }

        public List<IdentityUser> Users { get; set; } // list to get all users

        public IdentityUser User { get; set; } // single to get one for details view

    }
}
