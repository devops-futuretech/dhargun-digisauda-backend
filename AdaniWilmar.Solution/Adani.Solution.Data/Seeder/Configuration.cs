using Adani.Solution.DTO.Enums;
using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class Configuration : ISeeder
    {
        public void Seed(AdaniContext context)
        {
             // SeedConfiguration(context);
        }

        private static void SeedConfiguration(IAdaniContext context)
        {
            context.Configurations.AddOrUpdate(x => x.Id,
                new Entities.Configuration
                {
                    Id = 1,
                    Name = "App Key",
                    Key = "AppKey",
                    Value = "87F81A7B-AFBB-4914-8766-28A8C9414298",
                    Isactive = false,
                    TypeId = (int)DataType.String,
                    ////SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                },
                new Entities.Configuration
                {
                    Id = 2,
                    Name = "Web Key",
                    Key = "WebKey",
                    Value = "EF8F4B8E-E702-4D4B-9E46-BBAC9AE14BD7",
                    Isactive = false,
                    TypeId = (int)DataType.String,
                    ////SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                },
                new Entities.Configuration
                {
                    Id = 3,
                    Name = "Notification Email",
                    Key = "NotificationEmail",
                    Value = "deal@emamiagrotech.com",
                    Isactive = true,
                    TypeId = (int)DataType.String,
                    ////SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                },
                new Entities.Configuration
                {
                    Id = 4,
                    Name = "Auditor Email",
                    Key = "AuditorEmail",
                    Value = "rajalingam.paulraj@impigertech.com",
                    Isactive = false,
                    TypeId = (int)DataType.String,
                    ////SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                }, new Entities.Configuration
                {
                    Id = 5,
                    Name = "Counter Bid Buffer Time (Mins)",
                    Key = "CounterBidBufferTime",
                    Value = "60",
                    Isactive = true,
                    TypeId = (int)DataType.Int,
                    ////SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                }, new Entities.Configuration
                {
                    Id = 6,
                    Name = "Bid Status Time (Mins)",
                    Key = "BidStatusTime",
                    Value = "5",
                    Isactive = false,
                    TypeId = (int)DataType.Int,
                    ////SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                }, new Entities.Configuration
                {
                    Id = 7,
                    Name = "Firebase SenderId",
                    Key = "FirebaseSenderId",
                    Value = "948096657912",
                    Isactive = false,
                    TypeId = (int)DataType.Int,
                    ////SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                }, new Entities.Configuration
                {
                    Id = 8,
                    Name = "Push Notify Server key",
                    Key = "PushNotifyServerkey",
                    Value = "AAAA3L726fg:APA91bGCicmeqd7FnBvAQfkzGKVUtNAgAUFBUiuthzM-siJZTl9aCAvFM79dBoCtPSO_JENZDYO90Ie1rvRSwN0Mf3mRSxAU2Qxs3_Tqy7yOaNz_KZfqBaIdHX3QcIHacdaXGOka3R6j",
                    Isactive = false,
                    TypeId = (int)DataType.String,
                    ////SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                }, new Entities.Configuration
                {
                    Id = 9,
                    Name = "Push Notify Url",
                    Key = "PushNotifyUrl",
                    Value = " https://fcm.googleapis.com/fcm/send",
                    Isactive = false,
                    TypeId = (int)DataType.String,
                    ////SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                }, new Entities.Configuration
                {
                    Id = 10,
                    Name = "Is SMS",
                    Key = "IsSMS",
                    Value = "True",
                    Isactive = true,
                    TypeId = (int)DataType.Boolean,
                    ////SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                }, new Entities.Configuration
                {
                    Id = 11,
                    Name = "Is EMAIL",
                    Key = "IsEMAIL",
                    Value = "True",
                    Isactive = true,
                    TypeId = (int)DataType.Boolean,
                    ////SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                }, new Entities.Configuration
                {
                    Id = 12,
                    Name = "Is Push Notification",
                    Key = "IsPushNotification",
                    Value = "True",
                    Isactive = true,
                    TypeId = (int)DataType.Boolean,
                    ////SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                }, new Entities.Configuration
                {
                    Id = 13,
                    Name = "Login Base Hour",
                    Key = "LoginBaseHour",
                    Value = "6",
                    Isactive = true,
                    TypeId = (int)DataType.Int,
                    ////SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                }, new Entities.Configuration
                {
                    Id = 14,
                    Name = "Counter Bid Allow Count",
                    Key = "CounterBidCount",
                    Value = "2",
                    Isactive = true,
                    TypeId = (int)DataType.Int,
                    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                }, new Entities.Configuration
                {
                    Id = 15,
                    Name = "Password Expiry Days",
                    Key = "PasswordExpiryDays",
                    Value = "9999",
                    Isactive = true,
                    TypeId = (int)DataType.Int,
                    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                }, new Entities.Configuration
                {
                    Id = 16,
                    Name = "Password Expiry Enabled",
                    Key = "PasswordExpiryEnabled",
                    Value = "False",
                    Isactive = true,
                    TypeId = (int)DataType.Boolean,
                    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                }, 
                //new Entities.Configuration
                //{
                //    Id = (int)DTO.Enums.Configuration.MinimumVehicleCapacityinPercent,
                //    Name = "Minimum Vehicle Capacity in Percent",
                //    Key = "MinimumVehicleCapacityinPercent",
                //    Value = "93",
                //    Isactive = true,
                //    TypeId = (int)DataType.Decimal,
                //    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                //}, 
                new Entities.Configuration
                {
                    Id = (int)DTO.Enums.Configuration.SupportFeatures,
                    Name = "Support Features",
                    Key = "SupportFeatures",
                    Value = "Dashboard,Sauda,Indent,Sales,STP",
                    Isactive = true,
                    TypeId = (int)DataType.String,
                    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                }, new Entities.Configuration
                {
                    Id = (int)DTO.Enums.Configuration.SupportEmail,
                    Name = "Support Email",
                    Key = "SupportEmail",
                    Value = "arunprasath.kuppusamy@Impigertech.com",
                    Isactive = true,
                    TypeId = (int)DataType.String,
                    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                }, 
                //new Entities.Configuration
                //{
                //    Id = (int)DTO.Enums.Configuration.SaudaconversionMinValue,
                //    Name = "Sauda conversion Minimum Value (MT)",
                //    Key = "SaudaconversionMinValue",
                //    Value = "0.008",
                //    Isactive = true,
                //    TypeId = (int)DataType.Decimal,
                //    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                //},
                //new Entities.Configuration
                //{
                //    Id = (int)DTO.Enums.Configuration.SaudaconversionMaxValue,
                //    Name = "Sauda conversion Maximum Value (MT)",
                //    Key = "SaudaconversionMaxValue",
                //    Value = "0.006",
                //    Isactive = true,
                //    TypeId = (int)DataType.Decimal,
                //    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                //},
                new Entities.Configuration
                {
                    Id = (int)DTO.Enums.Configuration.QuantityLimitforBookingSaudaEnabled,
                    Name = "Quantity Limit for Booking Sauda Enabled",
                    Key = "QuantityLimitforBookingSaudaEnabled",
                    Value = "False",
                    Isactive = true,
                    TypeId = (int)DataType.Boolean,
                    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                }, new Entities.Configuration
                {
                    Id = (int)DTO.Enums.Configuration.CallRecordMappingReattachBufferTime,
                    Name = "Call Record Mapping Re-attach Buffer Time (Mins)",
                    Key = "Call Record Mapping Re-attach Buffer Time (Mins)",
                    Value = "30",
                    Isactive = true,
                    TypeId = (int)DataType.Int,
                    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                }, new Entities.Configuration
                {
                    Id = (int)DTO.Enums.Configuration.DaysForAudioFilesShownAgainstSaudaMapping,
                    Name = "Days For Audio Files Shown Against SaudaMapping",
                    Key = "Days For Audio Files Shown Against SaudaMapping",
                    Value = "30",
                    Isactive = true,
                    TypeId = (int)DataType.Int,
                    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                }, new Entities.Configuration
                {
                    Id = (int)DTO.Enums.Configuration.AudioFilesBackupDays,
                    Name = "Audio Files Backup Days",
                    Key = "Audio Files Backup Days",
                    Value = "50",
                    Isactive = true,
                    TypeId = (int)DataType.Int,
                    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                }, new Entities.Configuration
                {
                    Id = (int)DTO.Enums.Configuration.InboundInterfacenotSyncedToSAPMinutes,
                    Name = "Inbound Interface not Synced To SAP Minutes",
                    Key = "Inbound Interface not Synced To SAP Minutes",
                    Value = "60",
                    Isactive = true,
                    TypeId = (int)DataType.Int,
                    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                }, new Entities.Configuration
                {
                    Id = (int)DTO.Enums.Configuration.ReportSyncDateValidationInHours,
                    Name = "Report Sync Date Validation In Hours",
                    Key = "Report Sync Date Validation In Hours",
                    Value = "2",
                    Isactive = true,
                    TypeId = (int)DataType.Int,
                    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                },
                //new Entities.Configuration
                //{
                //    Id = (int)DTO.Enums.Configuration.MinimumVolumeCapacityinPercent,
                //    Name = "Minimum Volume Capacity in Percent",
                //    Key = "Minimum Volume Capacity in Percent",
                //    Value = "80",
                //    Isactive = true,
                //    TypeId = (int)DataType.Int,
                //    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                //}, 
                new Entities.Configuration
                {
                    Id = (int)DTO.Enums.Configuration.DiscountAmountforSpecialRateApproval,
                    Name = "Discount Amount for Special Rate Approval",
                    Key = "Discount Amount for Special Rate Approval",
                    Value = "50",
                    Isactive = true,
                    TypeId = (int)DataType.Int,
                    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                },
                //new Entities.Configuration
                //{
                //    Id = (int)DTO.Enums.Configuration.MaximumVehicleCapacityinPercent,
                //    Name = "Maximum Vehicle Capacity in Percent",
                //    Key = "Maximum Vehicle Capacity in Percent",
                //    Value = "105",
                //    Isactive = true,
                //    TypeId = (int)DataType.Int,
                //    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                //}, new Entities.Configuration
                //{
                //    Id = (int)DTO.Enums.Configuration.MaximumVolumeCapacityinPercent,
                //    Name = "Maximum Volume Capacity in Percent",
                //    Key = "Maximum Volume Capacity in Percent",
                //    Value = "100",
                //    Isactive = true,
                //    TypeId = (int)DataType.Int,
                //    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                //}, 
                new Entities.Configuration
                {
                    Id = (int)DTO.Enums.Configuration.SaudaExtensionDays,
                    Name = "Sauda Extension Days",
                    Key = "Sauda Extension Days",
                    Value = "2",
                    Isactive = true,
                    TypeId = (int)DataType.Int,
                    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                }, 
                //new Entities.Configuration
                //{
                //    Id = (int)DTO.Enums.Configuration.IsApplySpecialityFatDiscount,
                //    Name = "Apply Speciality Fat Discount",
                //    Key = "Apply Speciality Fat Discount",
                //    Value = "False",
                //    Isactive = true,
                //    TypeId = (int)DataType.Int,
                //    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                //},
                new Entities.Configuration
                {
                    Id = (int)DTO.Enums.Configuration.EmailsBasedOnDivisionForSaudaReport,
                    Name = "Emails Based On Division For Sauda Report",
                    Key = "Emails Based On Division For Sauda Report",
                    Value = "",
                    Isactive = true,
                    TypeId = (int)DataType.Int,
                    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                }, 
                //new Entities.Configuration
                //{
                //    Id = (int)DTO.Enums.Configuration.FromRange1,
                //    Name = "From Range 1",
                //    Key = "From Range 1",
                //    Value = "0",
                //    Isactive = true,
                //    TypeId = (int)DataType.Int,
                //    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                //}, new Entities.Configuration
                //{
                //    Id = (int)DTO.Enums.Configuration.ToRange1,
                //    Name = "To Range 1",
                //    Key = "To Range 1",
                //    Value = "15",
                //    Isactive = true,
                //    TypeId = (int)DataType.Int,
                //    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                //}, new Entities.Configuration
                //{
                //    Id = (int)DTO.Enums.Configuration.FromRange2,
                //    Name = "From Range 2",
                //    Key = "From Range 2",
                //    Value = "16",
                //    Isactive = true,
                //    TypeId = (int)DataType.Int,
                //    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                //}, new Entities.Configuration
                //{
                //    Id = (int)DTO.Enums.Configuration.ToRange2,
                //    Name = "To Range 2",
                //    Key = "To Range 2",
                //    Value = "30",
                //    Isactive = true,
                //    TypeId = (int)DataType.Int,
                //    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                //}, new Entities.Configuration
                //{
                //    Id = (int)DTO.Enums.Configuration.FromRange3,
                //    Name = "From Range 3",
                //    Key = "From Range 3",
                //    Value = "31",
                //    Isactive = true,
                //    TypeId = (int)DataType.Int,
                //    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                //}, new Entities.Configuration
                //{
                //    Id = (int)DTO.Enums.Configuration.ToRange3,
                //    Name = "To Range 3",
                //    Key = "To Range 3",
                //    Value = "45",
                //    Isactive = true,
                //    TypeId = (int)DataType.Int,
                //    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                //}, new Entities.Configuration
                //{
                //    Id = (int)DTO.Enums.Configuration.FromRange4,
                //    Name = "From Range 4",
                //    Key = "From Range 4",
                //    Value = "61",
                //    Isactive = true,
                //    TypeId = (int)DataType.Int,
                //    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                //}, new Entities.Configuration
                //{
                //    Id = (int)DTO.Enums.Configuration.ToRange4,
                //    Name = "To Range 4",
                //    Key = "To Range 4",
                //    Value = "90",
                //    Isactive = true,
                //    TypeId = (int)DataType.Int,
                //    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                //}, new Entities.Configuration
                //{
                //    Id = (int)DTO.Enums.Configuration.ToRange5,
                //    Name = "To Range 5",
                //    Key = "To Range 5",
                //    Value = "> 91",
                //    Isactive = true,
                //    TypeId = (int)DataType.Int,
                //    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                //}, 
                new Entities.Configuration
                {
                    Id = (int)DTO.Enums.Configuration.GPSTrackingDayToDeleteRecord,
                    Name = "GPS Tracking Day To Delete Record",
                    Key = "GPS Tracking Day To Delete Record",
                    Value = "2",
                    Isactive = true,
                    TypeId = (int)DataType.Int,
                    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                }, new Entities.Configuration
                {
                    Id = (int)DTO.Enums.Configuration.OpenContractTimeInHrs,
                    Name = "Open Contract Time in Hrs",
                    Key = "Open Contract Time in Hrs",
                    Value = "2",
                    Isactive = true,
                    TypeId = (int)DataType.Int,
                    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                }, new Entities.Configuration
                {
                    Id = (int)DTO.Enums.Configuration.SaudaExpireNotification,
                    Name = "Sauda Expire Notification",
                    Key = "Sauda Expire Notification",
                    Value = "2",
                    Isactive = true,
                    TypeId = (int)DataType.Int,
                    //SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess
                }, new Entities.Configuration
                {
                    Id = (int)DTO.Enums.Configuration.DueDaysCount,
                    Name = "Due Days Count",
                    Key = "Due Days Count",
                    Value = "",
                    Isactive = true,
                    TypeId = (int)DataType.Int,
                },
                new Entities.Configuration
                {
                    Id = (int)DTO.Enums.Configuration.SalesReportDeleteDaysCount,
                    Name = "Sales Report Delete Days Count",
                    Key = "Sales Report Delete Days Count",
                    Value = "",
                    Isactive = true,
                    TypeId = (int)DataType.Int,
                },
                new Entities.Configuration
                {
                    Id = (int)DTO.Enums.Configuration.CustomerLedgerDeleteDaysCount,
                    Name = "Customer Ledger Delete Days Count",
                    Key = "Customer Ledger Delete Days Count",
                    Value = "",
                    Isactive = true,
                    TypeId = (int)DataType.Int,
                },
                new Entities.Configuration
                {
                    Id = (int)DTO.Enums.Configuration.SaudaCreationNationalTraderApproval,
                    Name = "Sauda Creation National Trader Approval",
                    Key = "Sauda Creation National Trader Approval",
                    Value = "False",
                    Isactive = true,
                    TypeId = (int)DataType.Boolean,
                },
                new Entities.Configuration
                {
                    Id = (int)DTO.Enums.Configuration.SaudaCreationZonalTraderApproval,
                    Name = "Sauda Creation Zonal Trader Approval",
                    Key = "Sauda Creation Zonal Trader Approval",
                    Value = "False",
                    Isactive = true,
                    TypeId = (int)DataType.Boolean,
                },
                new Entities.Configuration
                {
                    Id = (int)DTO.Enums.Configuration.AccountStatementDays,
                    //Id = 59,
                    Name = "Account Statement Days",
                    Key = "Account Statement Days",
                    Value = "180",
                    Isactive = true,
                    TypeId = (int)DataType.Int,
                },
                new Entities.Configuration
                {
                    Id = (int)DTO.Enums.Configuration.AccountStatementHitCount,
                    //Id = 59,
                    Name = "Account Statement Hit Count",
                    Key = "Account Statement Hit Count",
                    Value = "4",
                    Isactive = true,
                    TypeId = (int)DataType.Int,
                }
                );
            //context.SaveChanges();
        }
    }
}
