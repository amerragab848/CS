using System;
using System.Drawing;
using System.IO;
using System.Xml.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using SimpleProjectManager.Module.BusinessObjects.Marketing;
using SimpleProjectManager.Module.BusinessObjects.Planning;

namespace SimpleProjectManager.Module.DatabaseUpdate {
    public class Updater : ModuleUpdater {
        Random rnd = new Random();

        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            if(ObjectSpace.CanInstantiate(typeof(Project))) {
                CreatePlanningData();
            }
            if(ObjectSpace.CanInstantiate(typeof(Customer))) {
                CreateMarketingData();
            }
        }
        private Image GetEmbeddedResourceImage(string shortName) {
            try {
                return Image.FromStream(GetEmbeddedResourceStream(shortName));
            }
            catch {
                return null;
            }
        }
        private Stream GetEmbeddedResourceStream(string shortName) {
            string embeddedResourceName = Array.Find<string>(this.GetType().Assembly.GetManifestResourceNames(), (s) => { return s.Contains(shortName); });
            return this.GetType().Assembly.GetManifestResourceStream(embeddedResourceName);
        }
        private byte[] GetEmbeddedResourceByteArray(string name) {
            UnmanagedMemoryStream stream = (UnmanagedMemoryStream)GetEmbeddedResourceStream(name);
            stream.Position = 0;
            byte[] result = new byte[stream.Length];
            stream.Read(result, 0, (Int32)stream.Length);
            return result;
        }
        private void CreatePlanningData() {
            Tracing.Tracer.LogText("Creating Persons");
            Person john = CreatePerson("Nilsen", "John", GetEmbeddedResourceByteArray("m01.jpg"), "John@example.com");
            Person sam = CreatePerson("Peterson", "Sam", GetEmbeddedResourceByteArray("m03.jpg"), "Sam@example.com");
            Person mary = CreatePerson("Tellitson", "Mary", GetEmbeddedResourceByteArray("w02.jpg"), "Mary@example.com");
            Person lily = CreatePerson("Johnson", "Lily", GetEmbeddedResourceByteArray("w04.jpg"), "Lily@example.com");

            Tracing.Tracer.LogText("Creating 'General DevExpress XAF Evaluation' Project & Tasks");
            Project generalEvaluationProject = CreateProject("General DevExpress XAF Evaluation", mary);
            CreateProjectTask(generalEvaluationProject, "1. Check general product and company information, licensing and pricing", "http://www.devexpress.com/xaf", ProjectTaskStatus.Completed, mary);
            CreateProjectTask(generalEvaluationProject, "2. Check features of the DevExpress WinForms, ASP.NET WebForms & DevExtreme HTML5/JavaScript controls used in XAF", "http://www.devexpress.com/asp\r\nhttp://www.devexpress.com/winforms\r\nhttps://js.devexpress.com/", ProjectTaskStatus.NotStarted, mary);
            CreateProjectTask(generalEvaluationProject, "3. Download a free 30-day trial (free technical support included)", "http://www.devexpress.com/Home/Try.xml", ProjectTaskStatus.Completed, mary);
            CreateProjectTask(generalEvaluationProject, "4. Play with online demo applications on the web site and research local demos in the Demo Center from the installation", "SimpleProjectManager, MainDemo, XCRM, XVideoRental, FeatureCenter, SecurityDemo, WorkflowDemo, etc. at %public%\\Documents\\", ProjectTaskStatus.InProgress, mary);
            CreateProjectTask(generalEvaluationProject, "5. Analyze key characteristics (look & feel, features set, performance, usability, etc.)", "http://www.devexpress.com/xaf", ProjectTaskStatus.InProgress, mary);
            CreateProjectTask(generalEvaluationProject, "6. Try the Getting Started tutorials and build a few a prototypes", "https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument113577", ProjectTaskStatus.InProgress, mary);
            CreateProjectTask(generalEvaluationProject, "7. Check support options and learning materials (documentation, examples, videos, blogs, webinars, etc.)", "http://www.devexpress.com/support/\r\n", ProjectTaskStatus.Completed, mary);
            CreateProjectTask(generalEvaluationProject, "8. Learn more about the Universal subscription features", "http://www.devexpress.com/Subscriptions/Universal.xml", ProjectTaskStatus.Completed, mary);
            CreateProjectTask(generalEvaluationProject, "9. Consider using a live chat on site or emailing at info@devexpress.com if you have any pre-sales questions", "http://www.devexpress.com/Home/ContactUs.xml", ProjectTaskStatus.Deferred, mary);

            Tracing.Tracer.LogText("Creating 'DevExpress XAF Features Overview' Project & Tasks");
            Project featuresOverviewProject = CreateProject("DevExpress XAF Features Overview", sam);
            try {
                XDocument doc = XDocument.Load(GetEmbeddedResourceStream("ProjectTasks.xml"));
                foreach(XElement el in doc.Root.Elements()) {
                    XAttribute subject = el.Attribute("Subject");
                    if(subject != null) {
                        ProjectTask task = ObjectSpace.FindObject<ProjectTask>(new BinaryOperator("Subject", subject.Value));
                        if(task == null) {
                            task = ObjectSpace.CreateObject<ProjectTask>();
                            task.Subject = subject.Value;

                            XElement notes = el.Element(XName.Get("Notes"));
                            if(notes != null) {
                                task.Notes = notes.Value.Replace("\n", Environment.NewLine);
                            }
                            task.AssignedTo = rnd.Next(5) % 2 == 0 ? (rnd.Next(6) % 2 == 0 ? sam : lily) : john;
                            task.Project = featuresOverviewProject;
                            task.Status = rnd.Next(7) % 2 == 0 ? (rnd.Next(8) % 2 == 0 ? ProjectTaskStatus.InProgress : ProjectTaskStatus.Completed) : (rnd.Next(9) % 2 == 0 ? ProjectTaskStatus.NotStarted : ProjectTaskStatus.Completed);
                            if(task.Status == ProjectTaskStatus.InProgress || task.Status == ProjectTaskStatus.Completed) {
                                task.StartDate = DateTime.Now.AddDays(rnd.Next(1) * (-1));
                            }
                            if(task.Status == ProjectTaskStatus.Completed) {
                                task.EndDate = DateTime.Now.AddDays(rnd.Next(2));
                            }
                        }
                    }
                    ObjectSpace.CommitChanges();
                }
            }
            catch(Exception ex) {
                Tracing.Tracer.LogError(ex);
            }
        }

        private void CreateMarketingData() {
            Tracing.Tracer.LogText("Creating Customers and Testimonials");
            try {
                XDocument doc = XDocument.Load(GetEmbeddedResourceStream("Testimonials.xml"));
                foreach(XElement el in doc.Root.Elements()) {
                    XAttribute firstName = el.Attribute("FirstName");
                    if(firstName != null) {
                        Customer customer = ObjectSpace.FindObject<Customer>(new BinaryOperator("FirstName", firstName.Value));
                        if(customer == null) {
                            customer = ObjectSpace.CreateObject<Customer>();
                            customer.FirstName = firstName.Value;
                            customer.Email = customer.FirstName + "@company.com";
                            XAttribute lastName = el.Attribute("LastName");
                            if(lastName != null) {
                                customer.LastName = lastName.Value;
                            }
                            XAttribute company = el.Attribute("Company");
                            if(company != null) {
                                customer.Company = company.Value;
                            }
                            XAttribute occupation = el.Attribute("Occupation");
                            if(occupation != null) {
                                customer.Occupation = occupation.Value;
                            }
                            XAttribute photo = el.Attribute("Photo");
                            if(photo != null) {
                                customer.Photo = GetEmbeddedResourceByteArray(photo.Value);
                            }
                            else {
                                customer.Photo = GetEmbeddedResourceByteArray("Unknown-user.png");
                            }
                            Testimonial testimonial = ObjectSpace.CreateObject<Testimonial>();
                            XAttribute quote = el.Attribute("Quote");
                            if(quote != null) {
                                testimonial.Quote = quote.Value;
                            }
                            XAttribute highlight = el.Attribute("Highlight");
                            if(highlight != null) {
                                testimonial.Highlight = highlight.Value;
                            }
                            XAttribute tags = el.Attribute("Tags");
                            if(tags != null) {
                                testimonial.Tags = tags.Value;
                            }
                            XAttribute caseStudyUrl = el.Attribute("CaseStudyUrl");
                            if(caseStudyUrl != null) {
                                testimonial.CaseStudyUrl = caseStudyUrl.Value;
                            }
                            testimonial.Customer = customer;
                        }
                    }
                    ObjectSpace.CommitChanges();
                }
            }
            catch(Exception ex) {
                Tracing.Tracer.LogError(ex);
            }
        }

        private Project CreateProject(string name, Person manager) {
            Project project = ObjectSpace.FindObject<Project>(new BinaryOperator("Name", name));
            if(project == null) {
                project = ObjectSpace.CreateObject<Project>();
                project.Name = name;
                project.Manager = manager;
                project.Save();
            }
            return project;
        }
        private ProjectTask CreateProjectTask(Project project, string subject, string notes, ProjectTaskStatus status, Person assignedTo) {
            ProjectTask task = ObjectSpace.FindObject<ProjectTask>(new BinaryOperator("Subject", subject));
            if(task == null) {
                task = ObjectSpace.CreateObject<ProjectTask>();
                task.Project = project;
                task.Subject = subject;
                task.Notes = notes;
                task.Status = status;
                task.AssignedTo = assignedTo;
                if(task.Status == ProjectTaskStatus.InProgress || task.Status == ProjectTaskStatus.Completed) {
                    task.StartDate = DateTime.Now.AddDays(rnd.Next(1) * (-1));
                }
                if(task.Status == ProjectTaskStatus.Completed) {
                    task.EndDate = DateTime.Now.AddDays(rnd.Next(2));
                }
                task.Save();
            }
            return task;
        }
        private Person CreatePerson(string lastName, string firstName, byte[] photo, string email) {
            Person person = ObjectSpace.FindObject<Person>(new BinaryOperator("FirstName", firstName));
            if(person == null) {
                person = ObjectSpace.CreateObject<Person>();
                person.FirstName = firstName;
                person.LastName = lastName;
                person.Photo = photo;
                person.Email = email;
                if(person.FirstName == "Sam") {
                    person.Birthday = new DateTime(1955, 12, 1);
                }
                else {
                    person.Birthday = new DateTime(DateTime.Now.Year - (20 + rnd.Next(10)), rnd.Next(11) + 1, rnd.Next(27) + 1);
                }
                PhoneNumber workPhone1 = ObjectSpace.CreateObject<PhoneNumber>();
                workPhone1.Number = "(818)3383-3389";
                workPhone1.PhoneType = "Work";
                person.PhoneNumbers.Add(workPhone1);
                PhoneNumber homePhone = ObjectSpace.CreateObject<PhoneNumber>();
                homePhone.Number = "(818)3383-3383";
                homePhone.PhoneType = "Home";
                person.PhoneNumbers.Add(homePhone);
                Address address = ObjectSpace.CreateObject<Address>();
                Country country = ObjectSpace.FindObject<Country>(CriteriaOperator.Parse("Name='USA'"));
                if(country == null) {
                    country = ObjectSpace.CreateObject<Country>(); ;
                    country.Name = "USA";
                    country.PhoneCode = "1";
                }
                address.Country = country;
                address.City = "Glendale";
                address.ZipPostal = "91203";
                address.StateProvince = "CA";
                address.Street = "505 N. Brand Blvd, 16th Floor";
                person.Address1 = address;
                person.Save();
            }
            return person;
        }
    }
}
