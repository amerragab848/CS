using System;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Filtering;
using DevExpress.Persistent.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleProjectManager.Module.BusinessObjects.Marketing {
	[ImageName("BO_Customer")]
	[NavigationItem("Marketing")]
	[DefaultProperty("FullName")]
	public class Customer {
		public const string EmailRegularExpression = "^[A-Za-z0-9_\\+-]+(\\.[A-Za-z0-9_\\+-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9-]+)*\\.([A-Za-z]{2,4})$";
		public Customer() {
			Testimonials = new List<Testimonial>();
		}
		[Browsable(false)]
		public int Id { get; protected set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		[RuleRegularExpression(EmailRegularExpression, CustomMessageTemplate = "Invalid email format!")]
		public string Email { get; set; }
		public string Company { get; set; }
		public string Occupation { get; set; }
		[Aggregated]
		public virtual IList<Testimonial> Testimonials { get; set; }

		[NotMapped, RuleRequiredField, SearchMemberOptions(SearchMemberMode.Exclude)]
		public string FullName {
			get {
				string namePart = string.Format("{0} {1}", FirstName, LastName);
				return Company != null ? string.Format("{0} ({1})", namePart, Company) : namePart;
			}
		}

		[ImageEditor(ListViewImageEditorCustomHeight = 75, DetailViewImageEditorFixedHeight = 150)]
		public byte[] Photo { get; set; }
	}

	[DefaultProperty("Customer")]
	[ImageName("BO_Note")]
	[NavigationItem("Marketing")]
	[DefaultListViewOptions(MasterDetailMode.ListViewAndDetailView, true, NewItemRowPosition.Top)]
	public class Testimonial {
		public Testimonial() {
			CreatedOn = DateTime.Now.ToString("MM-DD-YYYY");
		}
		[Browsable(false)]
		public int Id { get; protected set; }
		[FieldSize(FieldSizeAttribute.Unlimited), RuleRequiredField]
		public string Quote { get; set; }
		[FieldSize(512), RuleRequiredField(ResultType = ValidationResultType.Warning)]
		public string Highlight { get; set; }
		[VisibleInListView(false)]
		public string CreatedOn { get; internal set; }
		public string Tags { get; set; }
		[VisibleInListView(false)]
		public string CaseStudyUrl { get; set; }
		[RuleRequiredField]
		public virtual Customer Customer { get; set; }
	}
}
