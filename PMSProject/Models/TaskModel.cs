using Mono.TextTemplating;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

using System.Timers;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMSProject.Models
{
    public class TaskModel : BaseEntityModel
    {
        public int Id { get; set; }

        //    member m-1 done
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser? AssignedUser { get; set; }

        public string? TaskName { get; set; }

        public  TaskStatus Status { get; set; }
        public TaskStage? Stage { get; set; }

        public string? Note { get; set; }


        ////Relations
        /////1-M PROJECT
        public int? ProjectId { get; set; }
        public  ProjectModel? Project { get; set; }

        // sprints 1-m
        public int? SprintId { get; set; }
        public SprintModel? Sprint { get; set; }

        public DateTime? EstimatedDate {  get; set; }
        public DateTime? StartedOn { get; set; }

        public DateTime? FinishedOn { get; set; }

       





    }
    public enum TaskStatus
    {
        [Display(Name = "Not Started")] 
        NotStarted=0,
        [Display(Name = "On Hold")]
        OnHold=1,
        [Display(Name = "In Progress")]
        InProgress=2,
        Done=3,
        Canceled=4
    }

    public enum TaskStage
    {
        Sprint = 0,
        BackLog = 1 
    }
}
