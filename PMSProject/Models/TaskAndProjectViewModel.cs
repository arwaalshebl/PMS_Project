namespace PMSProject.Models
{
    public class TaskAndProjectViewModel
    {
        public List<TaskModel> Tasks { get; set; }
        public List<TaskModel> DirectTasks { get; set; }

        public List<TaskModel> ProjectTasks{ get; set; }


        public List<ProjectModel> Projects { get; set; } 

        public List<SprintModel> Sprints { get; set; }
    }
}
