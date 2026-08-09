using Adani.Solution.Data.DatabaseContext;
using System;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class RoleTypeClaim : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedRoleTypeClaim(context);
        }
        private static void SeedRoleTypeClaim(IAdaniContext context)
        {
            context.RoleTypeClaims.AddOrUpdate(x => x.Id,
                 new Entities.RoleTypeClaim
                 {
                     Id = 1,
                     RoleTypeId = 1,
                     ClaimId = 1,
                     CreatedBy = 1,
                     CreatedDate = DateTime.Now
                 }
                 );
        }
    }
}
