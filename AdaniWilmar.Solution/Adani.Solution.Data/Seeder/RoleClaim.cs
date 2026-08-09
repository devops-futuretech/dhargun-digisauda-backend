using Adani.Solution.Data.DatabaseContext;
using System;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class RoleClaim : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedRoleClaim(context);
        }
        private static void SeedRoleClaim(IAdaniContext context)
        {
            context.RoleClaims.AddOrUpdate(x => x.Id,
                 new Entities.RoleClaim
                 {
                     Id = 1,
                     RoleId = 1,
                     ClaimId = 1,
                     CreatedBy = 1,
                     CreatedDate = DateTime.Now
                 }
                 );
        }
    }
}
