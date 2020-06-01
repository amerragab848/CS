using System;
using System.Configuration;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.EF;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Internal;
using SimpleProjectManager.Module.BusinessObjects.Marketing;

namespace SimpleProjectManager.Web {
	// For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/DevExpressExpressAppWebWebApplicationMembersTopicAll.aspx
	public partial class SimpleProjectManagerAspNetApplication : WebApplication {
		private DevExpress.ExpressApp.SystemModule.SystemModule module1;
		private DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule module2;
		private SimpleProjectManager.Module.SimpleProjectManagerModule module3;
		private SimpleProjectManager.Module.Web.SimpleProjectManagerAspNetModule module4;
        private DevExpress.ExpressApp.Validation.ValidationModule validationModule;
        private DevExpress.ExpressApp.Validation.Web.ValidationAspNetModule validationAspNetModule;

		#region Default XAF configuration options (https://www.devexpress.com/kb=T501418)
		static SimpleProjectManagerAspNetApplication() {
			EnableMultipleBrowserTabsSupport = true;
            DevExpress.ExpressApp.Web.Editors.ASPx.ASPxGridListEditor.AllowFilterControlHierarchy = true;
            DevExpress.ExpressApp.Web.Editors.ASPx.ASPxGridListEditor.MaxFilterControlHierarchyDepth = 3;
            DevExpress.ExpressApp.Web.Editors.ASPx.ASPxCriteriaPropertyEditor.AllowFilterControlHierarchyDefault = true;
            DevExpress.ExpressApp.Web.Editors.ASPx.ASPxCriteriaPropertyEditor.MaxHierarchyDepthDefault = 3;
            DevExpress.Persistent.Base.PasswordCryptographer.EnableRfc2898 = true;
            DevExpress.Persistent.Base.PasswordCryptographer.SupportLegacySha512 = false;
        }
        private void InitializeDefaults() {
            LinkNewObjectToParentImmediately = false;
            OptimizedControllersCreation = true;
        }
        #endregion
		public SimpleProjectManagerAspNetApplication() {
			InitializeComponent();
			InitializeDefaults();
		}
        protected override IViewUrlManager CreateViewUrlManager() {
            return new ViewUrlManager();
        }
        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
			string xpoConnectionString = DbEngineDetector.PatchConnectionString(ConfigurationManager.ConnectionStrings["ConnectionStringXpo"].ConnectionString);
			string efConnectionString = DbEngineDetector.PatchConnectionString(ConfigurationManager.ConnectionStrings["ConnectionStringEF"].ConnectionString);
			if(Global.IsSiteMode()) {
				InMemoryDataStoreProvider.Register();
				xpoConnectionString = InMemoryDataStoreProvider.ConnectionString;
                var efConnectionStringItem = ConfigurationManager.ConnectionStrings["SimpleProjectManagerSiteModeConnectionStringEF"];
                if(efConnectionStringItem == null) {
                    throw new Exception("The 'SimpleProjectManagerSiteModeConnectionStringEF' item is missing in the configuration file!");
                }
                efConnectionString = efConnectionStringItem.ConnectionString;
			}
			args.ObjectSpaceProviders.Add(new XPObjectSpaceProvider(xpoConnectionString, null, true));
			args.ObjectSpaceProviders.Add(new EFObjectSpaceProvider(typeof(SimpleProjectManagerDbContext), efConnectionString));
		}
		private void SimpleProjectManagerAspNetApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e) {
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
		private void InitializeComponent() {
			this.module1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
			this.module2 = new DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule();
			this.module3 = new SimpleProjectManager.Module.SimpleProjectManagerModule();
			this.module4 = new SimpleProjectManager.Module.Web.SimpleProjectManagerAspNetModule();
            this.validationModule = new DevExpress.ExpressApp.Validation.ValidationModule();
            this.validationAspNetModule = new DevExpress.ExpressApp.Validation.Web.ValidationAspNetModule();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// SimpleProjectManagerAspNetApplication
			// 
			this.ApplicationName = "SimpleProjectManager";
            this.CheckCompatibilityType = DevExpress.ExpressApp.CheckCompatibilityType.DatabaseSchema;
			this.Modules.Add(this.module1);
			this.Modules.Add(this.module2);
			this.Modules.Add(this.module3);
			this.Modules.Add(this.module4);
            this.Modules.Add(this.validationModule);
            this.Modules.Add(this.validationAspNetModule);
			this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.SimpleProjectManagerAspNetApplication_DatabaseVersionMismatch);
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();
		}
	}
}
