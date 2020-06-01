using System.Data.Common;
using System.Data.Entity;
using DevExpress.ExpressApp.EF.Updating;

namespace SimpleProjectManager.Module.BusinessObjects.Marketing {
	//Business Model Design with Entity Framework - https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113462.aspx
	//Use the Entity Framework Code First in XAF - https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113466.aspx
	public class SimpleProjectManagerDbContext : DbContext {
		public SimpleProjectManagerDbContext(string connectionString) : base(connectionString) { }
		public SimpleProjectManagerDbContext(DbConnection connection) : base(connection, true) { }
		public DbSet<Customer> Customer { get; set; }
		public DbSet<Testimonial> Testimonial { get; set; }
	}
}
