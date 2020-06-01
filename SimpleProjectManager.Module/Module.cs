using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Xpo;
using System.Collections.Generic;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Updating;

namespace SimpleProjectManager.Module {
	// For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppModuleBasetopic.aspx.
	public sealed partial class SimpleProjectManagerModule : ModuleBase {
		static SimpleProjectManagerModule() {
			DevExpress.Data.Linq.CriteriaToEFExpressionConverter.SqlFunctionsType = typeof(System.Data.Entity.SqlServer.SqlFunctions);
			DevExpress.Data.Linq.CriteriaToEFExpressionConverter.EntityFunctionsType = typeof(System.Data.Entity.DbFunctions);
			// Uncomment this code to delete and recreate the database each time the data model has changed.
			// Do not use this code in a production environment to avoid data loss.
			// #if DEBUG
			// Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SimpleProjectManagerDbContext>());
			// #endif 
		}
		public SimpleProjectManagerModule() {
			InitializeComponent();
			BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
		}
		public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
			ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
			return new ModuleUpdater[] { updater };
		}
		public override void Setup(XafApplication application) {
			base.Setup(application);
			// Manage various aspects of the application UI and behavior at the module level.
		}
		public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
			base.CustomizeTypesInfo(typesInfo);
			CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
		}
	}
}
