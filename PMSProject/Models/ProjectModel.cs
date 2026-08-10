using Microsoft.AspNetCore.Identity;

namespace PMSProject.Models
{
    public class ProjectModel
    {
        public int Id { get; set; }

        public string? UserId { get; set; } = null;
        public IdentityUser? AssignedUser { get; set; }



        public string ProjectName { get; set; }


        //hierarchy (a family tree)
        //هنا الاي دي بيكون NULL  اذا كان هو المشروع الرئيسي ماله اي مشروع فرعي
        
        public int? ParentProjectID { get; set; }
        public ProjectModel? ParentProject { get; set; }

        //للمشاريع الفرعيه كانه علاقه مع الجدول نفسه 
        public ICollection<ProjectModel> SubProjects { get; set; } = new List<ProjectModel>();

        public int? Weight { get; set; } = 1;

        public string? Note { get; set; }

        public string? AttachmentPath { get; set; }


        // Relations
        public ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();         

    }
}
