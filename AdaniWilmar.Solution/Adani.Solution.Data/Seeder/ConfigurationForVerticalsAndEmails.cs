using Adani.Solution.DTO.Enums;
using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class ConfigurationForDivisionsAndEmails : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedConfigurationForDivisionsAndEmails(context);
        }

        private static void SeedConfigurationForDivisionsAndEmails(IAdaniContext context)
        {
            context.ConfigurationForDivisionsAndEmails.AddOrUpdate(x => x.Id,
                new Entities.ConfigurationForDivisionsAndEmails
                {
                    Id = (int)DTO.Enums.ConfigurationForVerticalsAndEmails.VerticalsBasedOnSaudaValidityDate,
                    Name = "Divisions Based On Sauda Validity Date",
                    Key = "DivisionsBasedOnSaudaValidityDate",
                    Value = "",
                    Isactive = true,
                    TypeId = (int)DataType.String,
                    SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                });
        }
    }
}
