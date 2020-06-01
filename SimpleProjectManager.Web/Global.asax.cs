using System;
using DevExpress.Web;
using DevExpress.Internal;
using System.Configuration;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.Persistent.Base;
using System.Web.Routing;

namespace SimpleProjectManager.Web {
    public class Global : System.Web.HttpApplication {
        public Global() {
            InitializeComponent();
        }
        protected void Application_Start(Object sender, EventArgs e) {
            RouteTable.Routes.RegisterXafRoutes();
            ASPxWebControl.CallbackError += new EventHandler(Application_Error);

#if DEBUG
			DevExpress.ExpressApp.Web.TestScripts.TestScriptsManager.EasyTestEnabled = true;
#endif

        }
        protected void Session_Start(Object sender, EventArgs e) {
            Tracing.Initialize();
            WebApplication.SetInstance(Session, new SimpleProjectManagerAspNetApplication());
            if(IsSiteMode()) {
                WebApplication.Instance.ObjectSpaceCreated += (s, args) => {
                    if(args.ObjectSpace is DevExpress.ExpressApp.EF.EFObjectSpace) {
                        args.ObjectSpace.Committing += ObjectSpace_Committing;
                        args.ObjectSpace.Disposed += ObjectSpace_Disposed;
                    }
                };
            }
            if(IsSiteMode() || (System.Diagnostics.Debugger.IsAttached && WebApplication.Instance.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema)) {
                WebApplication.Instance.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
            DefaultVerticalTemplateContentNew.ClearSizeLimit();
            WebApplication.Instance.SwitchToNewStyle();
            WebApplication.Instance.Setup();
            WebApplication.Instance.Start();
        }
        internal static bool IsSiteMode() {
            //return true;
            return ConfigurationManager.AppSettings["SiteMode"] != null && ConfigurationManager.AppSettings["SiteMode"].ToLower() == "true";
        }
        void ObjectSpace_Committing(object sender, System.ComponentModel.CancelEventArgs e) {
            throw new UserFriendlyException("Data modifications are not allowed for EF objects in this online demo. You can test data editing functionality by installing XAF on your machine and running the demo locally.");
        }
        void ObjectSpace_Disposed(object sender, EventArgs e) {
            IObjectSpace os = (IObjectSpace)sender;
            os.Committing -= ObjectSpace_Committing;
            os.Disposed -= ObjectSpace_Disposed;
        }
        protected void Application_BeginRequest(Object sender, EventArgs e) {
        }
        protected void Application_EndRequest(Object sender, EventArgs e) {
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e) {
        }
        protected void Application_Error(Object sender, EventArgs e) {
            ErrorHandling.Instance.ProcessApplicationError();
        }
        protected void Session_End(Object sender, EventArgs e) {
            WebApplication.LogOff(Session);
            WebApplication.DisposeInstance(Session);
        }
        protected void Application_End(Object sender, EventArgs e) {
        }
        #region Web Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
        }
        #endregion
    }
}
