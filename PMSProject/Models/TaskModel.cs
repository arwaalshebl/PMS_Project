using Mono.TextTemplating;
using System.ComponentModel.DataAnnotations;
using System.Timers;

namespace PMSProject.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string? UserId { get; set; }


        public string? TaskName { get; set; }

        public  TaskStatus Status { get; set; }
        public TaskStage Stage { get; set; }



        ////Relations
        //projrcts m-m
        public ICollection<ProjectModel> projects { get; set; }
        //    member m-m
        // sprints m-m

        public DateTime? EstimatedDate {  get; set; }
        public DateTime? StartedOn { get; set; }

        public DateTime? FinishedOn { get; set; }

        //Over Due
        //project member





    }
    public enum TaskStatus
    {
        [Display(Name = "Not Started")] 
        NotStarted,
        Later,
        [Display(Name = "On Hold")]
        OnHold,
        [Display(Name = "In Progress")]
        InProgress,
        Done,
        Canceled
    }

    public enum TaskStage
    {
        Sprint,
        BackLog
    }
}
