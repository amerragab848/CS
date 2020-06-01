using System;
using System.Configuration;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.EF;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Internal;
using SimpleProjectManager.Module.BusinessObjects.Marketing;

namespace SimpleProjectManager.Win {
	// For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/DevExpressExpressAppWinWinApplicationMembersTopicAll.aspx
	public partial class SimpleProjectManagerWindowsFormsApplication : WinApplication {
		#region Default XAF configuration options (https://www.devexpress.com/kb=T501418)
		static SimpleProjectManagerWindowsFormsApplication() {
			DevExpress.Persistent.Base.PasswordCryptographer.EnableRfc2898 = true;
			DevExpress.Persistent.Base.PasswordCryptographer.SupportLegacySha512 = false;
		}
		private void InitializeDefaults() {
			LinkNewObjectToParentImmediately = false;
			OptimizedControllersCreation = true;
            UseLightStyle = true;
            SplashScreen = new DevExpress.ExpressApp.Win.Utils.DXSplashScreen(typeof(Demos.Win.XafDemoSplashScreen), new DefaultOverlayFormOptions());
            ExecuteStartupLogicBeforeClosingLogonWindow = true;
        }
		#endregion
		public SimpleProjectManagerWindowsFormsApplication() {
			InitializeComponent();
			InitializeDefaults();
		}
		protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
			args.ObjectSpaceProviders.Add(new XPObjectSpaceProvider(DbEngineDetector.PatchConnectionString(ConfigurationManager.ConnectionStrings["ConnectionStringXpo"].ConnectionString), null, false));
			args.ObjectSpaceProviders.Add(new EFObjectSpaceProvider(typeof(SimpleProjectManagerDbContext), DbEngineDetector.PatchConnectionString(ConfigurationManager.ConnectionStrings["ConnectionStringEF"].ConnectionString)));
		}
		private void SimpleProjectManagerWindowsFormsApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e) {
			//#if EASYTEST
			//For demo purposes only.
			e.Updater.Update();
			e.Handled = true;
			//#else
			//if(System.Diagnostics.Debugger.IsAttached) {
			//	e.Updater.Update();
			//	e.Handled = true;
			//}
			//else {
			//	throw new InvalidOperationException(
			//		"The application cannot connect to the specified database, because the latter doesn't exist or its version is older than that of the application.\r\n" +
			//		"This error occurred  because the automatic database update was disabled when the application was started without debugging.\r\n" +
			//		"To avoid this error, you should either start the application under Visual Studio in debug mode, or modify the " +
			//		"source code of the 'DatabaseVersionMismatch' event handler to enable automatic database update, " +
			//		"or manually create a database using the 'DBUpdater' tool.\r\n" +
			//		"Anyway, refer to the 'Update Application and Database Versions' help topic at http://help.devexpress.com/#Xaf/CustomDocument2795 " +
			//		"for more detailed information. If this doesn't help, please contact our Support Team at http://www.devexpress.com/Support/Center/");
			//}
			//#endif
		}
	}
}
