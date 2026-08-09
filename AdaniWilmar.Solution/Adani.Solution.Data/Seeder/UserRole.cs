using System;
using System.Collections.Generic;
using System.Linq;
using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class UserRole : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedUserRole(context);
        }
        private static void SeedUserRole(IAdaniContext context)
        {
            context.UserRoles.AddOrUpdate(x => x.Id,
                 new Entities.UserRole
                 {
                     Id = 1,
                     UserId = 1,
                     RoleId=1
                 }
             );
        }
    }
}
