using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsMine.Models
{
    public class HomeWorkTemplate
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool HasCondition { get; set; }
    }

    public class RequestStatus
    {
        public string ErrorMessage { get; set; }
        public bool Result { get; set; }
    }
    public class HomeWorkStudentListView : HomeWorkTemplate
    {
        public HomeWorkStudentListView(Project project)
        {
            this.Id = project.Id;
            this.Title = project.HomeWork.Title;
            this.Description = project.HomeWork.Description;
            this.HasCondition = project.HomeWork.HasCondition;
            if (project.HomeWork.HasCondition)
            {
                this.ConditionUntil = project.HomeWork.Condition.Until;
            }
            this.Mark = project.Mark;
        }
        public DateTime? ConditionUntil { get; set; }
        public int Mark { get; set; }
    }

    public class HomeWorkPresentation : HomeWorkTemplate
    {
        public HomeWorkPresentation(Project project)
        {
            this.Id = project.Id;
            this.Title = project.HomeWork.Title;
            this.Description = project.HomeWork.Description;
            this.Mark = project.Mark;
            this.HasCondition = project.HomeWork.HasCondition;
            this.Attachments = new List<AttachmentView>();
            if (project.HomeWork.HasCondition)
            {
                this.Condition = project.HomeWork.Condition;
            }
            foreach (var item in project.HomeWork.Attachments)
            {
                this.Attachments.Add(new AttachmentView(item));
            }
        }
        public int Mark { get; set; }
        public Condition Condition { get; set; }
        public ICollection<AttachmentView> Attachments { get; set; }
    }

    public class UploadHomeWork
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string FileBase64Data { get; set; }
        public string Format { get; set; }
    }

    public class UploadHomeWorkResult : RequestStatus
    {
        public UploadHomeWorkResult()
        {

        }
        public UploadHomeWorkResult(bool result , UploadHomeWorkStatus status , string errorMessage)
        {
            this.Result = result;
            this.Status = status;
            this.ErrorMessage = errorMessage;
        }
        public UploadHomeWorkStatus Status { get; set; }
    }

    public class HomeWorkProject
    {
        public HomeWorkProject(Project project)
        {
            this.ProjectId = project.Id;
            this.StudentName = project.Author.Name;
            this.CurrentMark = project.Mark;
        }
        public int ProjectId { get; set; }
        public string StudentName { get; set; }
        public int CurrentMark { get; set; }
        public Condition Condition { get; set; }
    }

    public class HomeWorkProjectsList : RequestStatus
    {
        public HomeWorkProjectsList()
        {
            this.Projects = new List<HomeWorkProject>();
        }
        public List<HomeWorkProject> Projects { get; set; }
        public HomeWorkStatus Status { get; set; }
    }

    public class HomeWorkEditView : HomeWorkTemplate
    {
        public Condition Condition { get; set; }
    }
    public enum HomeWorkStatus { OK , CannotFindHW , IncorrectHWID}
    public enum UploadHomeWorkStatus { OK , IncorrectFormat , AlreadyBloked , NotHomeWork , Exception}
}