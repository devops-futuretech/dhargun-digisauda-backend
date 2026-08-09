using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Seeder;
using Adani.Solution.DTO.Common;

namespace Adani.Solution.Data.DatabaseContextMigrations
{
    internal sealed class MigrationConfiguration : DbMigrationsConfiguration<AdaniContext>
    {
        public MigrationConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
            MigrationsDirectory = @"DatabaseContextMigrations";
        }

        protected override void Seed(AdaniContext context)
        {
            if (Utility.IsSeederUpdate)
            {
                var instances = from t in Assembly.GetExecutingAssembly().GetTypes()
                                where t.GetInterfaces().Contains(typeof(ISeeder))
                                      && t.GetConstructor(Type.EmptyTypes) != null
                                select Activator.CreateInstance(t) as ISeeder;

                foreach (var instance in instances)
                {
                    instance.Seed(context);
                }
            }
        }
    }
}
