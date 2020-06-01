using System;
using System.Linq;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using SimpleProjectManager.Module.BusinessObjects.Marketing;

namespace SimpleProjectManager.Module.Controllers {
	public class CustomerListViewController : ViewController {
		protected override void OnActivated() {
			base.OnActivated();
			FilterController filterController = Frame.GetController<FilterController>();
			if(filterController != null) {
				filterController.FullTextSearchTargetPropertiesMode = FullTextSearchTargetPropertiesMode.AllSearchableMembers;
			}
		}
		public CustomerListViewController()
			: base() {
			TypeOfView = typeof(ListView);
			TargetObjectType = typeof(Customer);
		}
	}
}
