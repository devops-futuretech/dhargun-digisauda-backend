using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
   public class TradeTicketOilType : ISeeder
    {
        public void Seed(AdaniContext context)
        {
           SeedTradeTicketOilType(context);
        }

        private static void SeedTradeTicketOilType(IAdaniContext context)
        {
            context.TradeTicketOilTypes.AddOrUpdate(x => x.Id, new Entities.TradeTicketOilType
            {
                Id = 1,
                OilTypeName = "Palm",              
                IsActive = true,
                SAPId="1",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 2,
                OilTypeName = "Veg Oil",
                IsActive = true,
                SAPId = "2",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 3,
                OilTypeName = "Sunflower",
                IsActive = true,
                SAPId = "3",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 4,
                OilTypeName = "Soya Bean",
                IsActive = true,
                SAPId = "4",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 5,
                OilTypeName = "Mustard",
                IsActive = true,
                SAPId = "5",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 6,
                OilTypeName = "Rice Bran",
                IsActive = true,
                SAPId = "6",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 7,
                OilTypeName = "Bake Magic",
                IsActive = true,
                SAPId = "7",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 8,
                OilTypeName = "Rice Bran",
                IsActive = true,
                SAPId = "8",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 9,
                OilTypeName = "Stearin",
                IsActive = true,
                SAPId = "9",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 10,
                OilTypeName = "Refined Palm Kernel Oil",
                IsActive = true,
                SAPId = "10",
                DivisionId = 1
            },            
            new Entities.TradeTicketOilType
            {
                Id = 11,
                OilTypeName = "Rice Bran WAX",
                IsActive = true,
                SAPId = "11",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 12,
                OilTypeName = "Hard Stearin",
                IsActive = true,
                SAPId = "12",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 13,
                OilTypeName = "SF - AP Y14",
                IsActive = true,
                SAPId = "13",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 14,
                OilTypeName = "SF - AP Y99",
                IsActive = true,
                SAPId = "14",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 15,
                OilTypeName = "SF - AP YG1",
                IsActive = true,
                SAPId = "15",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 16,
                OilTypeName = "SF - 3 in 1 S11",
                IsActive = true,
                SAPId = "16",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 17,
                OilTypeName = "SF - 3 in 1 S13",
                IsActive = true,
                SAPId = "17",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 18,
                OilTypeName = "SF - 3 in 1 SG1",
                IsActive = true,
                SAPId = "18",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 19,
                OilTypeName = "SF - PK 4",
                IsActive = true,
                SAPId = "19",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 20,
                OilTypeName = "SF - PK 5",
                IsActive = true,
                SAPId = "20",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 21,
                OilTypeName = "SF - PK 6",
                IsActive = true,
                SAPId = "21",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 22,
                OilTypeName = "SF - PK 8",
                IsActive = true,
                SAPId = "22",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 23,
                OilTypeName = "SF - PK 9",
                IsActive = true,
                SAPId = "23",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 24,
                OilTypeName = "SF - PK 10",
                IsActive = true,
                SAPId = "24",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 25,
                OilTypeName = "SF - PKG - 1",
                IsActive = true,
                SAPId = "25",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 26,
                OilTypeName = "SF - PKG - 2",
                IsActive = true,
                SAPId = "26",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 27,
                OilTypeName = "SF - BC - 11",
                IsActive = true,
                SAPId = "27",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 28,
                OilTypeName = "SF - BC - 13",
                IsActive = true,
                SAPId = "28",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 29,
                OilTypeName = "SF - BC - 13 TIN",
                IsActive = true,
                SAPId = "29",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 30,
                OilTypeName = "SF - BCG - 1",
                IsActive = true,
                SAPId = "30",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 31,
                OilTypeName = "SF - BCG - 2",
                IsActive = true,
                SAPId = "31",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 32,
                OilTypeName = "SF - BC - 99 TIN",
                IsActive = true,
                SAPId = "32",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 33,
                OilTypeName = "SF - AER - PG 1",
                IsActive = true,
                SAPId = "33",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 34,
                OilTypeName = "SF - AER - I 1",
                IsActive = true,
                SAPId = "34",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 35,
                OilTypeName = "RBD",
                IsActive = true,
                SAPId = "35",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 36,
                OilTypeName = "PMF",
                IsActive = true,
                SAPId = "36",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 37,
                OilTypeName = "Til Oil",
                IsActive = true,
                SAPId = "37",
                DivisionId = 1
            },
            new Entities.TradeTicketOilType
            {
                Id = 38,
                OilTypeName = "RBD*",
                IsActive = true,
                SAPId = "1",
                DivisionId = 2
            },
            new Entities.TradeTicketOilType
            {
                Id = 39,
                OilTypeName = "RBD",
                IsActive = true,
                SAPId = "2",
                DivisionId = 2
            },
            new Entities.TradeTicketOilType
            {
                Id = 40,
                OilTypeName = "RSFO",
                IsActive = true,
                SAPId = "3",
                DivisionId = 2
            },
            new Entities.TradeTicketOilType
            {
                Id = 41,
                OilTypeName = "RPO",
                IsActive = true,
                SAPId = "4",
                DivisionId = 2
            },
            new Entities.TradeTicketOilType
            {
                Id = 42,
                OilTypeName = "Super oline",
                IsActive = true,
                SAPId = "5",
                DivisionId = 2
            },
            new Entities.TradeTicketOilType
            {
                Id = 43,
                OilTypeName = "PMF",
                IsActive = true,
                SAPId = "6",
                DivisionId = 2
            },
            new Entities.TradeTicketOilType
            {
                Id = 44,
                OilTypeName = "Hard Stearine",
                IsActive = true,
                SAPId = "7",
                DivisionId = 2
            },
            new Entities.TradeTicketOilType
            {
                Id = 38,
                OilTypeName = "Hyd.Oil(RBD 60 degree)",
                IsActive = true,
                SAPId = "8",
                DivisionId = 2
            },
            new Entities.TradeTicketOilType
            {
                Id = 45,
                OilTypeName = "Hyd.Oil(RBD 52 degree)",
                IsActive = true,
                SAPId = "9",
                DivisionId = 2
            },
            new Entities.TradeTicketOilType
            {
                Id = 46,
                OilTypeName = "Hyd.Oil(RBD 44 degree)",
                IsActive = true,
                SAPId = "10",
                DivisionId = 2
            },
            new Entities.TradeTicketOilType
            {
                Id = 47,
                OilTypeName = "Hyd.Oil (RBD*)",
                IsActive = true,
                SAPId = "11",
                DivisionId = 2
            },
            new Entities.TradeTicketOilType
            {
                Id = 48,
                OilTypeName = "Til Oil",
                IsActive = true,
                SAPId = "12",
                DivisionId = 2
            },
            new Entities.TradeTicketOilType
            {
                Id = 49,
                OilTypeName = "Refined Palm Kernel Oil",
                IsActive = true,
                SAPId = "13",
                DivisionId = 2
            },
            new Entities.TradeTicketOilType
            {
                Id = 50,
                OilTypeName = "GMS",
                IsActive = true,
                SAPId = "14",
                DivisionId = 2
            },
            new Entities.TradeTicketOilType
            {
                Id = 51,
                OilTypeName = "Lakpol 60",
                IsActive = true,
                SAPId = "15",
                DivisionId = 2
            },
            new Entities.TradeTicketOilType
            {
                Id = 52,
                OilTypeName = "GMS - Danisco",
                IsActive = true,
                SAPId = "16",
                DivisionId = 2
            },
            new Entities.TradeTicketOilType
            {
                Id = 53,
                OilTypeName = "Water",
                IsActive = true,
                SAPId = "17",
                DivisionId = 2
            },
            new Entities.TradeTicketOilType
            {
                Id = 54,
                OilTypeName = "Salt",
                IsActive = true,
                SAPId = "18",
                DivisionId = 2
            },
            new Entities.TradeTicketOilType
            {
                Id = 55,
                OilTypeName = "Soya Lecithin",
                IsActive = true,
                SAPId = "19",
                DivisionId = 2
            },
            new Entities.TradeTicketOilType
            {
                Id = 56,
                OilTypeName = "Citric Acid",
                IsActive = true,
                SAPId = "20",
                DivisionId = 2
            },
            new Entities.TradeTicketOilType
            {
                Id = 57,
                OilTypeName = "Sod. Citrate",
                IsActive = true,
                SAPId = "21",
                DivisionId = 2
            },
            new Entities.TradeTicketOilType
            {
                Id = 58,
                OilTypeName = "Pot. Sorbate",
                IsActive = true,
                SAPId = "22",
                DivisionId = 2
            },
            new Entities.TradeTicketOilType
            {
                Id = 59,
                OilTypeName = "Lactic Acid",
                IsActive = true,
                SAPId = "23",
                DivisionId = 2
            },
            new Entities.TradeTicketOilType
            {
                Id = 60,
                OilTypeName = "RRBD *",
                IsActive = true,
                SAPId = "24",
                DivisionId = 2
            },
            new Entities.TradeTicketOilType
            {
                Id = 61,
                OilTypeName = "Refined Palm Kernel Oline",
                IsActive = true,
                SAPId = "25",
                DivisionId = 2
            },
            new Entities.TradeTicketOilType
            {
                Id = 62,
                OilTypeName = "PS421",
                IsActive = true,
                SAPId = "26",
                DivisionId = 2
            });
        }
    }
}
