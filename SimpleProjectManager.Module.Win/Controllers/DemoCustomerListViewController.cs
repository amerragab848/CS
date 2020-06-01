using DevExpress.Utils;
using DevExpress.ExpressApp;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using DevExpress.ExpressApp.Win.Editors;
using SimpleProjectManager.Module.BusinessObjects.Marketing;

namespace SimpleProjectManager.Module.Win.Controllers {
	//For demo purposes only.
    public class DemoCustomerListViewController : ObjectViewController<ListView, Customer> {
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            GridListEditor gridListEditor = View.Editor as GridListEditor;
            if(gridListEditor != null) {
                GridView gridView = gridListEditor.GridView;
                gridListEditor.Grid.HandleCreated += (s, e) => {
                    gridView.OptionsView.ShowColumnHeaders = false;
                    gridView.OptionsView.ShowIndicator = false;
                    gridView.OptionsView.ShowVerticalLines = DefaultBoolean.False;
                    GridColumn emailColumn = gridView.Columns["Email"];
                    if(emailColumn != null) {
                        emailColumn.MinWidth = 150;
                    }
                    GridColumn photoColumn = gridView.Columns["Photo"];
                    if(photoColumn != null) {
                        photoColumn.MinWidth = photoColumn.MaxWidth = 150;
                        ((RepositoryItemPictureEdit)photoColumn.ColumnEdit).ZoomPercent = 80;
                    }
                };
                gridView.CalcRowHeight += (s, e) => {
                    e.RowHeight = 100;
                };
            }
        }
    }
}
