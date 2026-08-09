using Adani.Solution.Data.DatabaseContext;
using System;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class User : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedUser(context);
        }
        private static void SeedUser(IAdaniContext context)
        {
            context.Users.AddOrUpdate(x => x.Id,
                 new Entities.User
                 {
                     Id = 1,
                     Name = "Super Admin",
                     MobileNumber = "9000000001",
                     Email = "adani@mailinator.com",
                     Code = "adaniadmin",
                     Password = "JXJK14rJK/nCUGdsaZIc2w==",
                     //RoleId = 1,
                     IsActive = true,
                     IsBlacklisted = false,
                     ParentUserId = 0,
                     CreatedBy = 1,
                     CreatedDate = DateTime.Now,
                     CityId = 1,
                     //DivisionId = null,
                     Loadability = 0,
                     //CustomerGroupOneId = 0,
                     //CustomerGroupTwoId = 0
                 }
             );
        }
    }
}
