using Adani.Solution.Data.DatabaseContext;

namespace Adani.Solution.Data.Seeder
{
    public interface ISeeder
    {
        void Seed(AdaniContext context);
    }
}
