using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class BookingType : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            context.BookingTypes.AddOrUpdate(x => x.Id,
                new Entities.BookingType() { Id = 1, Name = "HBC SO(B)", IsActive = true },
                new Entities.BookingType() { Id = 2, Name = "Rasoi Cont(C)", IsActive = true },
                new Entities.BookingType() { Id = 3, Name = "Rasoi SO(D)", IsActive = true },
                new Entities.BookingType() { Id = 4, Name = "BIB Cont(E)", IsActive = true },
                new Entities.BookingType() { Id = 5, Name = "BIB SO(F)", IsActive = true },
                new Entities.BookingType() { Id = 6, Name = "SF Cont(G)", IsActive = true },
                new Entities.BookingType() { Id = 7, Name = "SF SO(H)", IsActive = true },
                new Entities.BookingType() { Id = 8, Name = "H&T SO(I)", IsActive = false },
                new Entities.BookingType() { Id = 9, Name = "H&T SO (M)", IsActive = false },
                new Entities.BookingType() { Id = 10, Name = "HBC Cont(A)", IsActive = true }
                );
        }
    }
}
