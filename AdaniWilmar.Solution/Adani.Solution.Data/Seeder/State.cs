using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;
namespace Adani.Solution.Data.Seeder
{
   
    public class State : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedState(context);
        }

        private static void SeedState(IAdaniContext context)
        {
            context.State.AddOrUpdate(x => x.Id, new Entities.State
            {
                Id = 1,
                StateName = "ALWAR",
                CountryId = 1,
                IsActive = true,
            }
          );
        }
    }
}
