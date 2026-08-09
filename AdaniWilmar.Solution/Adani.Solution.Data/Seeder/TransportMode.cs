using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class TransportMode : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            SeedTransportMode(context);
        }

        private static void SeedTransportMode(IAdaniContext context)
        {
            context.TransportModes.AddOrUpdate(x => x.Id, new Entities.TransportMode
            {
                Id = 1,
                Name = "Truck",
                IsActive = true,
            },
            new Entities.TransportMode
            {
                Id = 2,
                Name = "Rake",
                IsActive = true,
            },
            new Entities.TransportMode
            {
                Id = 3,
                Name = "Lorry",
                IsActive = false,
            },
            new Entities.TransportMode
            {
                Id = 4,
                Name = "Ven",
                IsActive = false,
            });
        }
    }
}
