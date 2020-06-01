using System;
using DevExpress.Xpo;
using System.Drawing;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;

namespace SimpleProjectManager.Module.BusinessObjects.Planning {
	[ImageName("BO_Folder")]
	[NavigationItem("Planning")]
	public class Project : BaseObject {
		public Project(Session session) : base(session) { }
		[RuleRequiredField]
		public string Name { get; set; }
		public Person Manager { get; set; }
		[Size(SizeAttribute.Unlimited)]
		public string Description { get; set; }
		[Association, Aggregated]
		public XPCollection<ProjectTask> Tasks {
			get { return GetCollection<ProjectTask>("Tasks"); }
		}
	}
	public enum ProjectTaskStatus {
		[ImageName("State_Task_NotStarted")]
		NotStarted = 0,
		[ImageName("State_Task_InProgress")]
		InProgress = 1,
		[ImageName("State_Validation_Valid")]
		Completed = 2,
		[ImageName("State_Task_Deferred")]
		Deferred = 3
	}
	[ImageName("BO_Task")]
	[NavigationItem("Planning")]
	[DefaultProperty("Subject")]
	[DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
	[Appearance("Completed1", TargetItems = "Subject", Criteria = ProjectTask.CompletedCriteria, FontStyle = FontStyle.Strikeout, FontColor = "ForestGreen")]
	[Appearance("Completed2", TargetItems = "Subject", Criteria = ProjectTask.CompletedCriteria, Enabled = false)]
	[Appearance("InProgress", TargetItems = "Subject;AssignedTo", Criteria = ProjectTask.InProgressCriteria, BackColor = "LemonChiffon")]
	[Appearance("Deferred", TargetItems = "Subject", Criteria = ProjectTask.DeferredCriteria, BackColor = "MistyRose")]
	[RuleCriteria("EndDate >= StartDate")]
	public class ProjectTask : BaseObject {
		public const string CompletedCriteria = "Status = 'Completed'";
		public const string DeferredCriteria = "Status = 'Deferred'";
		public const string InProgressCriteria = "Status = 'InProgress'";
		public ProjectTask(Session session) : base(session) { }
		[Size(255), RuleRequiredField]
		public string Subject { get; set; }
		[ImmediatePostData]
		public ProjectTaskStatus Status { get; set; }
		[RuleRequiredField(TargetCriteria = ProjectTask.CompletedCriteria)]
		public Person AssignedTo { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		[Size(SizeAttribute.Unlimited)]
		public string Notes { get; set; }
		[Association]
		public Project Project { get; set; }
	}
}
