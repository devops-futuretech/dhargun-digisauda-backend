using Adani.Solution.Data.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Seeder
{
    public class MediaType : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            SeedMediaType(context);
        }

        private static void SeedMediaType(IAdaniContext context)
        {
            context.MediaType.AddOrUpdate(x => x.Id, new Entities.MediaType
            {
                Id = 1,
                Name = "Image",
                IsActive = true,
            },
            new Entities.MediaType
            {
                Id = 2,
                Name = "Video",
                IsActive = true,
            },
            new Entities.MediaType
            {
                Id = 3,
                Name = "Pdf",
                IsActive = true,
            },
            new Entities.MediaType
            {
                Id = (int)DTO.Enums.MediaType.Excel,
                Name = "Excel",
                IsActive = true,
            },
            new Entities.MediaType
            {
                Id = (int)DTO.Enums.MediaType.Word,
                Name = "Word",
                IsActive = true,
            },
            new Entities.MediaType
            {
                Id = (int)DTO.Enums.MediaType.Text,
                Name = "Text",
                IsActive = true,
            },
            new Entities.MediaType
            {
                Id = (int)DTO.Enums.MediaType.Audio,
                Name = "Audio",
                IsActive = true,
            }
            );
        }
    }
}
