using Mono.TextTemplating;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

using System.Timers;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMSProject.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser? AssignedUser { get; set; }

        public string? TaskName { get; set; }

        public  TaskStatus Status { get; set; }
        public TaskStage Stage { get; set; }



        ////Relations
        /////1-M PROJECT
        public int? ProjectId { get; set; }
        public  ProjectModel? Project { get; set; }

        //    member m-1 done
        // sprints 1-m

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
