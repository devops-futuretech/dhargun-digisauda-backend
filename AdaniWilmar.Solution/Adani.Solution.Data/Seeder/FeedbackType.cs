using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class FeedbackType:ISeeder
    {
        public void Seed(AdaniContext context)
        {
            SeedFeedbackType(context);
        }
        private static void SeedFeedbackType(IAdaniContext context)
        {
            context.FeedbackTypes.AddOrUpdate(x => x.Id, new Entities.FeedbackType
            {
                Id = 1,
                Name = "Bug",
                IsActive = true,
            },
            new Entities.FeedbackType
            {
                Id = 2,
                Name = "Enhancement",
                IsActive = true,
            },
            new Entities.FeedbackType
            {
                Id = 3,
                Name = "Kudos",
                IsActive = true,
            });
        }
    }
}
