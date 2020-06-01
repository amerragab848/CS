using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;

namespace SimpleProjectManager.Module.Web {
    [ToolboxItemFilter("Xaf.Platform.Web")]
    public sealed partial class SimpleProjectManagerAspNetModule : ModuleBase {
        public SimpleProjectManagerAspNetModule() {
            InitializeComponent();
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            return ModuleUpdater.EmptyModuleUpdaters;
        }
    }
}
