namespace PMSProject.Models
{
    public class ProjectModel
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        public string ProjectName { get; set; }


        //hierarchy (a family tree)
        //هنا الاي دي بيكون NULL  اذا كان هو المشروع الرئيسي ماله اي مشروع فرعي
        
        public int? ParentProjectID { get; set; }
        public ProjectModel? ParentProject { get; set; }

        //للمشاريع الفرعيه كانه علاقه مع الجدول نفسه 
        public ICollection<ProjectModel> SubProjects { get; set; } = new List<ProjectModel>();

        public int? Weight { get; set; } = 1;

        public string? Note { get; set; }




        // Relations
        public ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();         

    }
}
