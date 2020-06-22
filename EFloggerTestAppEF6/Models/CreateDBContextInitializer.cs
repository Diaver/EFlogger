using System.Data.Entity;

namespace EFloggerTestAppEF6.Models
{
    public class CreateDBContextInitializer : CreateDatabaseIfNotExists<UsersContext>
    {
        protected override void Seed(UsersContext context)
        {
            UsersContext.Seed(context);
            base.Seed(context);
        }
    }

}
