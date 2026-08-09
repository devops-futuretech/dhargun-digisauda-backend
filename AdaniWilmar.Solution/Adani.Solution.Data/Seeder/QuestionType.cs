using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Seeder;
using GMCore.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Seeder
{
    public class QuestionType : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedQuestionType(context);
        }
        private static void SeedQuestionType(IAdaniContext context)
        {
            context.QuestionTypes.AddOrUpdate(x => x.Id,
                new Entities.QuestionType
                {
                    Id = (int)DTO.Enums.QuestionType.TextEntry,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.QuestionType.TextEntry),
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = DateTime.Now
                },
                new Entities.QuestionType
                {
                    Id = (int)DTO.Enums.QuestionType.YesOrNo,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.QuestionType.YesOrNo),
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = DateTime.Now
                },
                new Entities.QuestionType
                {
                    Id = (int)DTO.Enums.QuestionType.SingleChoice,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.QuestionType.SingleChoice),
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = DateTime.Now
                },
                new Entities.QuestionType
                {
                    Id = (int)DTO.Enums.QuestionType.MultipleChoice,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.QuestionType.MultipleChoice),
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = DateTime.Now
                });
                //,
                //new Entities.QuestionType
                //{
                //    Id = (int)DTO.Enums.QuestionType.Attachments,
                //    Name = UtilityHelper.GetEnumDescription(DTO.Enums.QuestionType.Attachments),
                //    IsActive = true,
                //    CreatedBy = 1,
                //    CreatedDate = DateTime.Now
                //});
        }
    }
}
