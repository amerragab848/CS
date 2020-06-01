using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Actions;
using SimpleProjectManager.Module.BusinessObjects.Planning;

namespace SimpleProjectManager.Module.Controllers {
    public class ProjectTaskController : ViewController {
        public ProjectTaskController() {
            TargetObjectType = typeof(ProjectTask);
            TargetViewType = ViewType.Any;
            SimpleAction markCompletedAction = new SimpleAction(this, "MarkCompleted", PredefinedCategory.RecordEdit) {
                TargetObjectsCriteria = (CriteriaOperator.Parse("Status != ?", ProjectTaskStatus.Completed)).ToString(),
                ConfirmationMessage = "Are you sure to mark the selected task(s) as 'Completed'?",
                ImageName = "State_Task_Completed"
            };
            markCompletedAction.Execute += (s, e) => {
                IObjectSpace viewDataContext = View.ObjectSpace;
                foreach(ProjectTask task in e.SelectedObjects) {
                    task.EndDate = DateTime.Now;
                    task.Status = ProjectTaskStatus.Completed;
                    viewDataContext.SetModified(task); // Mark the changed object as 'dirty' (only required if data properties do not provide change notifications).
                }
                viewDataContext.CommitChanges();
                MessageOptions options = new MessageOptions();
                options.Duration = 2000;
                options.Message = string.Format("{0} task(s) have been successfully updated!", e.SelectedObjects.Count);
                options.Type = InformationType.Success;
                options.Web.Position = InformationPosition.Right;
                options.Win.Caption = "Success";
                options.Win.Type = WinMessageType.Flyout;
                Application.ShowViewStrategy.ShowMessage(options);
                //viewDataContext.Refresh(); // Optionally update the UI in accordance with the latest data changes.
            };
        }
    }
}
