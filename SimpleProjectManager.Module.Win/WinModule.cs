using System;
using System.Collections.Generic;
using System.ComponentModel;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;

namespace SimpleProjectManager.Module.Win {
    [ToolboxItemFilter("Xaf.Platform.Win")]
    public sealed partial class SimpleProjectManagerWindowsFormsModule : ModuleBase {
        public SimpleProjectManagerWindowsFormsModule() {
            InitializeComponent();
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            return ModuleUpdater.EmptyModuleUpdaters;
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            Demos.Feedback.XAFFeedbackHelper helper = new Demos.Feedback.XAFFeedbackHelper(application);
        }
    }
}
