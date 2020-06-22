using System.Data.Entity;
using EFloggerTestAppEF6.Models.Mapping;

namespace EFloggerTestAppEF6.Models
{
    public partial class UsersContext : DbContext
    {
        static UsersContext()
        {
            Database.SetInitializer<UsersContext>(null);
        }

        public UsersContext()
            : base("Name=UsersContext")
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());
        }

        public static void Seed(UsersContext context)
        {
            var defaultUser = new User { Name = "Vasy Pupkin" };
            context.Users.Add(defaultUser);
            context.SaveChanges();
        }
    }
}
