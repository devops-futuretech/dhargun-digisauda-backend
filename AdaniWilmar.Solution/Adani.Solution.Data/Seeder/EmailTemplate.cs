using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class EmailTemplate : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            SeedEmailTemplate(context);
        }


        private static void SeedEmailTemplate(IAdaniContext context)
        {
            const string companyName = "AWL Agri business";
            var htmlTemplateString = "<!DOCTYPE html><html lang='en'><head><meta charset='UTF - 8'><meta http-equiv='X - UA - Compatible' content='IE = edge'><meta name='viewport' content='width = device - width,initial - scale = 1'></head><body style='margin: 0; padding: 0' dir='ltr' bgcolor='#ffffff'><table border='0' cellspacing='0' cellpadding='0' align='center' id='m_-7626415423304311386email_table' style='border-collapse:collapse'><tbody><tr><td id='email-temp-container' style='font-family:Poppins,Helvetica Neue,Helvetica,Lucida Grande,tahoma,verdana,arial,sans-serif;background:#fff'><table border='0' width='100%' cellspacing='0' cellpadding='0' style='border-collapse:collapse'><tbody><tr><td><table class='emlogo' id='emlogo1' border='0' width='100%' cellspacing='0' cellpadding='0' style='border-collapse:collapse;text-align:center;width:100%'><tbody><tr><td style='line-height:0;width:600px;max-width:600px;padding:0 0 15px 0'><table border='0' width='100%' cellspacing='0' cellpadding='0' style='border-collapse:collapse'><tbody><tr><td style='background:transparent linear-gradient(270deg,#e9322b 0,#fbad18 100%) 0 0 no-repeat padding-box;width:100%;height:4px'></td></tr><tr style='background:#fff9ef;background:transparent linear-gradient(270deg,rgba(233,50,43,.07) 0,rgba(251,173,24,.07) 100%) 0 0 no-repeat padding-box'><td style='width:100%;text-align:center;height:120px'><img height='60' src='https://sauda.adaniwilmar.in:8080/images/logo1.png' style='border:0' class='CToWUd' data-bit='iit'></td></tr></tbody></table></td></tr></tbody></table></td></tr><tr><td><table border='0' width='100%' cellspacing='0' cellpadding='0' style='border-collapse:collapse;margin:0 auto 0 auto'><tbody><tr><td><table border='0' width='100%' cellspacing='0' cellpadding='0' style='border-collapse:collapse;margin:0 auto 0 auto;width:95%'><tbody><tr><td><table border='0' width='100%' cellspacing='0' cellpadding='0' style='border-collapse:collapse'><tbody><tr><td><table border='0' cellspacing='0' cellpadding='0' style='border-collapse:collapse'><tbody><tr><td>##MailContent##</td></tr><tr><td height='20' style='line-height:20px'>&nbsp;</td></tr><tr><td><p style='margin:10px 0 10px 0;color:#050b13;font-weight:700;font-size:16px'>Regards,</p><p style='margin:10px 0 10px 0;color:#050b13;font-size:16px'>Adani Groups</p></td></tr></tbody></table></td></tr></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table></td></tr><tr><td><table border='0' cellspacing='0' cellpadding='0' style='border-collapse:collapse;margin:0 auto 0 auto;width:95%'><tbody><tr><td height='20'>&nbsp;</td></tr><tr><td style='width:100%;text-align:center;height:70px;border-top:1px solid rgba(112,112,112,.3)'><div style='color:#050b13;font-size:12px;margin:0 auto 5px auto'>Adani groups Limited 2022. All rights reserved.<br></div></td></tr></tbody></table></td></tr></tbody></table><style></style></body></html>";
            context.EmailTemplate.AddOrUpdate(x => x.Id, new Entities.EmailTemplate
            {
                Id = 1,
                Name = "ForgotPasswordEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear User,</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Your password is: [PASSWORD] - Team " + companyName + "</p>",
                SMSTemplateID= "1707166210322098973"

            }, new Entities.EmailTemplate
            {
                Id = 2,
                Name = "ForgotPasswordSMS",
                Template = "Value",
                PlainTemplate = "Your password is: [PASSWORD] - Team " + companyName + "",
                SMSTemplateID= "1707166210322098973"
            }, new Entities.EmailTemplate
            {
                Id = 3,
                Name = "OtpEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear User,</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Your OTP for resetting password is: [OTP_VALUE]. Kindly do not share with anyone. - Team " + companyName + "</p>",
                SMSTemplateID= "1707166210328063944"
            }, new Entities.EmailTemplate
            {
                Id = 4,
                Name = "OtpSMS",
                Template = "Value",
                PlainTemplate = "Your OTP for resetting password is: [OTP_VALUE]. Kindly do not share with anyone. - Team " + companyName + "",
                SMSTemplateID = "1707166210328063944"
            }, new Entities.EmailTemplate
            {
                Id = 5,
                Name = "ReachUsEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Greetings,</h2>" +
                                    "<p style=\"margin-bottom:35px;\">[MESSAGE]</p>" +
                                    "<p style=\"margin:5px 0 40px; font-size: 16px; color: #444444;\">Regards,<br /><strong> [NAME] </strong><br />"
            }, new Entities.EmailTemplate
            {
                Id = 6,
                Name = "SaudaCreationEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear Sir,</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Your booking for: [SKU_NAME], [QUANTITY] @ Rs.[PRICE] plus Tax is confirmed. Please lift before expiry date.</p>"

            }, new Entities.EmailTemplate
            {
                Id = 7,
                Name = "SaudaCreationSMS",
                Template = "Value",
                PlainTemplate = "Your booking for: [SKU_NAME], [QUANTITY] @ Rs.[PRICE] plus Tax is confirmed. Please lift before expiry date - Team " + companyName + "",
                SMSTemplateID= "1707166210336335229"

            }, new Entities.EmailTemplate
            {
                Id = 8,
                Name = "SaudaApprovalEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear Sir,</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Congratulations! Your booking has been accepted for Sku: [SKU_NAME]  [QUANTITY] Qty @ Rs.[PRICE].</p>"
            }, new Entities.EmailTemplate
            {
                Id = 9,
                Name = "SaudaApprovalSMS",
                Template = "Value",
                PlainTemplate = "Congratulations! Your booking has been accepted for Sku: [SKU_NAME]  [QUANTITY] @ Rs.[PRICE]  - Team " + companyName + "",
                SMSTemplateID= "1707166210343251061"
            }, new Entities.EmailTemplate
            {
                Id = 10,
                Name = "LiftingRequestCreationEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear Sir,</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Your Sales Order is successfully submitted for Lifting Request Number - [LIFTING_REQUEST_NUMBER].</p>"
            }, new Entities.EmailTemplate
            {
                Id = 11,
                Name = "LiftingRequestCreationSMS",
                Template = "Value",
                PlainTemplate = "Dear Sir, Your Sales Order is successfully submitted for [LIFTING_REQUEST_NUMBER]. - Team " + companyName + "",
                SMSTemplateID= "1707166210352293151"
            }, new Entities.EmailTemplate
            {
                Id = 12,
                Name = "LiftingRequestApprovalEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear Sir,</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Your Sales Order request is approved.</p>"
            }, new Entities.EmailTemplate
            {
                Id = 13,
                Name = "LiftingRequestApprovalSMS",
                Template = "Value",
                PlainTemplate = "Dear Sir, Your Sales Order request is approved."
            }, new Entities.EmailTemplate
            {
                Id = 14,
                Name = "SpecialRateApprovalEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear [USER_NAME],</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Rate approval for [CUSTOMER_NAME], Sku: [SKU_NAME] for [QUANTITY]Case(s) at Rs.[PRICE] is been approved.</p>"

            }, new Entities.EmailTemplate
            {
                Id = 15,
                Name = "SpecialRateApprovalSMS",
                Template = "Value",
                PlainTemplate = "Dear [USER_NAME], Rate approval for [CUSTOMER_NAME], Sku: [SKU_NAME] for [QUANTITY]Case(s) at Rs.[PRICE] is been approved - Team " + companyName + "",
                SMSTemplateID= "1707166210361083255"
            }, new Entities.EmailTemplate
            {
                Id = 16,
                Name = "SaudaLimitApprovalEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear Sir,</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Sauda limit extension from [CONTRACT_QTY] MT to [QUANTITY] MT request is approved and extended for [CUSTOMER_NAME].</p>"
            }, new Entities.EmailTemplate
            {
                Id = 17,
                Name = "SaudaLimitApprovalSMS",
                Template = "Value",
                PlainTemplate = "Sauda limit enhancement from [CONTRACT_QTY] MT to [QUANTITY] MT request is approved and extended for [CUSTOMER_NAME] - Team " + companyName + "",
                SMSTemplateID= "1707166210365478366"
            }, new Entities.EmailTemplate
            {
                Id = 18,
                Name = "PriceDiscoveryEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear User,</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Request for Sku: [SKU_NAME] is [APPROVE_REJECT].</p>"
            }, new Entities.EmailTemplate
            {
                Id = 19,
                Name = "PriceDiscoverySMS",
                Template = "Value",
                PlainTemplate = "Request for Sku: [SKU_NAME] is [APPROVE_REJECT]."
            }, new Entities.EmailTemplate
            {
                Id = 20,
                Name = "SaudaConversionApprovalEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear Sir,</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Your Sauda conversion from Sku: [SKU_OLD] to Sku: [SKU_NEW] request is approved for [CUSTOMER_NAME]. </p>"

            }, new Entities.EmailTemplate
            {
                Id = 21,
                Name = "SaudaConversionApprovalSMS",
                Template = "Value",
                PlainTemplate = "Your Sauda conversion from Sku: [SKU_OLD] to Sku: [SKU_NEW] request is approved for [CUSTOMER_NAME]."
            },
            new Entities.EmailTemplate
            {
                Id = 22,
                Name = "FinalRateNotificationEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear User,</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Final pricing of Sku: [SKU_NAME] is [SKU_PRICING] </p>"
            }, new Entities.EmailTemplate
            {
                Id = 23,
                Name = "FinalRateNotificationSMS",
                Template = "Value",
                PlainTemplate = "Final pricing of Sku: [SKU_NAME] is [SKU_PRICING]"
            },
            new Entities.EmailTemplate
            {
                Id = 24,
                Name = "UserIncotermsEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear User,</h2>" +
                "<p><h2 style=\"margin-bottom:30px;display:inline;\">[NAME] ([ROLE_NAME])</h2> Incoterms has been modified. </p>" +
                                    "<p>IncoTerms: [INCO_TERMS]</p>" +
                                    "<p>Newly Added IncoTerms: [NEW_INCO_TERMS]</p>" +
                                    "<p>Removed IncoTerms: [REMOVED_INCO_TERMS]</p>"

            },
            new Entities.EmailTemplate
            {
                Id = 25,
                Name = "CounterBidOfferNotificationEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear Sir,</h2>" +
                                "<p style=\"margin-bottom:40px;\">Following booking, you have placed today are in counter, with the counter rates Sku: [SKU_NAME] @ Rs.[COUNTER_BID_Offer]/Case.</p>"
            }, new Entities.EmailTemplate
            {
                Id = 26,
                Name = "CounterBidOfferNotificationSMS",
                Template = "Value",
                PlainTemplate = "COUNTER BID : Dear Customer, your bid through window name ( [BIDDINGWINDOW_NAME] ) is lower than the accepted price and hence cannot be accepted. However we are happy to give you the following counter offer for SKU: [SKU_NAME] @ Rs.[COUNTER_BID_Offer]/case. Kindly confirm your decision before window closes."
            },
            new Entities.EmailTemplate
            {
                Id = 27,
                Name = "SaudaOrderPendingNotificationEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear Sir,</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Sauda for Sku: [SKU_NAME] booked for [QUANTITY]Case(s) at Rs.[PRICE]. </p>"
            }, new Entities.EmailTemplate
            {
                Id = 28,
                Name = "SaudaOrderPendingNotificationSMS",
                Template = "Value",
                PlainTemplate = "Sauda for Sku: [SKU_NAME] booked for [QUANTITY]Case(s) at Rs.[PRICE] - Team " + companyName + "",
                SMSTemplateID= "1707166210370425618"
            },
             new Entities.EmailTemplate
             {
                 Id = 29,
                 Name = "SaudaOrderHoldNotificationEmail",
                 Template = htmlTemplateString,
                 PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear Sir,</h2>" +
                                "<p style=\"margin-bottom:40px;\">Your booking is on hold for Sku: [SKU_NAME]  [QUANTITY]Case(s) @ Rs.[PRICE].</p>"
             }, new Entities.EmailTemplate
             {
                 Id = 30,
                 Name = "SaudaOrderHoldNotificationSMS",
                 Template = "Value",
                 PlainTemplate = "Your booking is on hold for Sku: [SKU_NAME]  [QUANTITY]Case(s) @ Rs.[PRICE]."
             },
            new Entities.EmailTemplate
            {
                Id = 31,
                Name = "TradeTicketQuantityIncrease",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear [NAME],</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Total sauda booked for the day is more than TT request quantity.If you increase additional TT quantity. Trade Ticket ContractQuantity is [CONTRACT_QTY]. Sauda Total BidQuantity is [BIDDING_QTY]</p>"

            },
            new Entities.EmailTemplate
            {
                Id = 32,
                Name = "TradeTicketQuantityIncreaseSMS",
                Template = "Value",
                PlainTemplate = "Trade Ticket ContractQuantity is [CONTRACT_QTY]. Sauda Total BidQuantity is [BIDDING_QTY]"
            },
            new Entities.EmailTemplate
            {
                Id = 33,
                Name = "PCPApproval",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear [NAME],</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Your PCP is been [APPROVE_REJECT].</p>"

            },
            new Entities.EmailTemplate
            {
                Id = 34,
                Name = "PCPApprovalSMS",
                Template = "Value",
                PlainTemplate = "Your PCP is been [APPROVE_REJECT]."
            },
            new Entities.EmailTemplate
            {
                Id = 35,
                Name = "MTPApproval",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear [NAME],</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Your MTP is been [APPROVE_REJECT].</p>"

            },
            new Entities.EmailTemplate
            {
                Id = 36,
                Name = "MTPApprovalSMS",
                Template = "Value",
                PlainTemplate = "Your MTP is been [APPROVE_REJECT]."
            },
            new Entities.EmailTemplate
            {
                Id = 37,
                Name = "MTPDeviationApproval",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear [NAME],</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Your MTP deviation for [Date] is been [APPROVE_REJECT].</p>"

            },
            new Entities.EmailTemplate
            {
                Id = 38,
                Name = "MTPDeviationApprovalSMS",
                Template = "Value",
                PlainTemplate = "Your MTP deviation for [Date] is been [APPROVE_REJECT]."
            },
            new Entities.EmailTemplate
            {
                Id = 39,
                Name = "SaudaExtensionApprovalNotificationEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear Sir,</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Your Sauda extension request for [NO_OF_Days] Days has been approved</p>"

            },
            new Entities.EmailTemplate
            {
                Id = 40,
                Name = "SaudaExtensionApprovalNotificationSMS",
                Template = "Value",
                PlainTemplate = "Dear Sir, Your Sauda extension request for [NO_OF_Days] Days has been approved - Team " + companyName + "",
                SMSTemplateID= "1707166210375594302"
            },
            new Entities.EmailTemplate
            {
                Id = 41,
                Name = "SaudaOrderRejectNotificationEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear Sir,</h2>" +
                                "<p style=\"margin-bottom:40px;\">Your booking is rejected for Sku: [SKU_NAME] [QUANTITY]Case(s) @ Rs.[PRICE].</p>"
            }, new Entities.EmailTemplate
            {
                Id = 42,
                Name = "SaudaOrderRejectNotificationSMS",
                Template = "Value",
                PlainTemplate = "Your booking is rejected for Sku: [SKU_NAME] [QUANTITY]Case(s) @ Rs.[PRICE] - Team " + companyName + "",
                SMSTemplateID= "1707166210380382862"

            }, new Entities.EmailTemplate
            {
                Id = 43,
                Name = "ServiceNotificationEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear User,</h2>" +
                                    "<p style=\"margin-bottom:40px;\">[METHOD_NAME] process is successfully completed ##date##. " + companyName + ".</p>"
            }, new Entities.EmailTemplate
            {
                Id = 44,
                Name = "ServiceNotificationSMS",
                Template = "Value",
                PlainTemplate = "[METHOD_NAME] process is successfully completed ##date##. " + companyName + "."
            }, new Entities.EmailTemplate
            {
                Id = 45,
                Name = "SaudaConversionRequestEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear Sir,</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Your Sauda conversion from Sku: [SKU_OLD] to Sku: [SKU_NEW] request is been successfully submitted for [CUSTOMER_NAME].</p>"

            }, new Entities.EmailTemplate
            {
                Id = 46,
                Name = "SaudaConversionRequestSMS",
                Template = "Value",
                PlainTemplate = "Your Sauda conversion from Sku: [SKU_OLD] to Sku: [SKU_NEW] request is been successfully submitted for [CUSTOMER_NAME]."
            }, new Entities.EmailTemplate
            {
                Id = 47,
                Name = "SaudaConversionRejectEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear Sir,</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Your Sauda conversion from Sku: [SKU_OLD] to Sku: [SKU_NEW] request is been rejected for [CUSTOMER_NAME].</p>"

            }, new Entities.EmailTemplate
            {
                Id = 48,
                Name = "SaudaConversionRejectSMS",
                Template = "Value",
                PlainTemplate = "Dear Sir, Your Sauda conversion from Sku: [SKU_OLD] to Sku: [SKU_NEW] request is been rejected for [CUSTOMER_NAME]."
            }, new Entities.EmailTemplate
            {
                Id = 49,
                Name = "SaudaLimitExtensionCreationEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear Sir,</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Your Sauda limit enhancement from [CONTRACT_QTY] to  [QUANTITY] request is successfully submitted for [CUSTOMER_NAME].</p>"
            }, new Entities.EmailTemplate
            {
                Id = 50,
                Name = "SaudaLimitExtensionCreationSMS",
                Template = "Value",
                PlainTemplate = "Your Sauda limit enhancement from [CONTRACT_QTY] to  [QUANTITY] request is successfully submitted for [CUSTOMER_NAME]."
            }, new Entities.EmailTemplate
            {
                Id = 51,
                Name = "FinalPricePublishNotificationEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear Sir,</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Price for all SKUs is been released. Kindly check your app for Price.</p>"

            }, new Entities.EmailTemplate
            {
                Id = 52,
                Name = "FinalPricePublishNotificationSMS",
                Template = "Value",
                PlainTemplate = "Dear Sir, Price for all SKUs is been released. Kindly check your app for Price."
            }, new Entities.EmailTemplate
            {
                Id = 53,
                Name = "CustomerDetailsChangeNotificationEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear Sir,</h2>" +
                                    "<p style=\"margin-bottom:40px;\">[INCOTERM_MOBILE_NO] for [Name] customer is been changed.</p>"

            }, new Entities.EmailTemplate
            {
                Id = 54,
                Name = "CustomerDetailsChangeNotificationSMS",
                Template = "Value",
                PlainTemplate = "Dear Sir, [INCOTERM_MOBILE_NO] for [Name] customer is been changed."
            }, new Entities.EmailTemplate
            {
                Id = 55,
                Name = "SaudaExtensionRejectNotificationEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear Sir,</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Your Sauda extension request for [NO_OF_Days] Days is been rejected.</p>"

            },
            new Entities.EmailTemplate
            {
                Id = 56,
                Name = "SaudaExtensionRejectNotificationSMS",
                Template = "Value",
                PlainTemplate = "Dear Sir, Your Sauda extension request for [NO_OF_Days] Days is been rejected."
            },
            new Entities.EmailTemplate
            {
                Id = 57,
                Name = "SaudaExtensionRequestNotificationEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear Sir,</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Your Sauda extension request for [NO_OF_Days] Days has been successfully submitted for [CUSTOMER_NAME].</p>"

            },
            new Entities.EmailTemplate
            {
                Id = 58,
                Name = "SaudaExtensionRequestNotificationSMS",
                Template = "Value",
                PlainTemplate = "Dear Sir, Your Sauda extension request for [NO_OF_Days] Days has been submitted  successfully to  [CUSTOMER_NAME]. - Team " + companyName + "",
                SMSTemplateID= "1707166210384368872"
            }, new Entities.EmailTemplate
            {
                Id = 59,
                Name = "SpecialRateRejectEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear [USER_NAME],</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Rate approval for [CUSTOMER_NAME], Sku: [SKU_NAME] for [QUANTITY]Case(s) at Rs.[PRICE] is been rejected.</p>"

            }, new Entities.EmailTemplate
            {
                Id = 60,
                Name = "SpecialRateRejectSMS",
                Template = "Value",
                PlainTemplate = "Dear [USER_NAME], Rate approval for [CUSTOMER_NAME], Sku: [SKU_NAME] for [QUANTITY]Case(s) at Rs.[PRICE] is been rejected."
            }, new Entities.EmailTemplate
            {
                Id = 61,
                Name = "SaudaLimitRejectEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear Sir,</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Your Sauda limit extension from [CONTRACT_QTY] MT to [QUANTITY] MT request is rejected for [CUSTOMER_NAME]</p>"
            }, new Entities.EmailTemplate
            {
                Id = 62,
                Name = "SaudaLimitRejectSMS",
                Template = "Value",
                PlainTemplate = "Your Sauda limit extension from [CONTRACT_QTY] MT to [QUANTITY] MT request is rejected for [CUSTOMER_NAME]"
            }, new Entities.EmailTemplate
            {
                Id = 63,
                Name = "SaudaApprovalTPFlowEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear Sir,</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Thank you for participating in the " + companyName + " sauda booking process.</p>" +
                                    "<p style=\"margin-bottom:40px;\">Congratulations! Your sauda booking has been approved.</p>" +
                                    "<p style=\"margin-bottom:40px;\">Sku: [SKU_NAME] for [QUANTITY] at Rs.[PRICE] booked [BY_FOR] [USER_NAME].</p>"
            }, new Entities.EmailTemplate
            {
                Id = 64,
                Name = "SaudaApprovalTPFlowSMS",
                Template = "Value",
                PlainTemplate = "Thank you for participating in the " + companyName + " sauda booking process. Congratulations! Your sauda booking has been accepted. Sku: [SKU_NAME] for [QUANTITY] at Rs.[PRICE] booked [BY_FOR] [USER_NAME]. - Team " + companyName + "",
                SMSTemplateID = "1707166210389044523"
            },new Entities.EmailTemplate
            {
                Id = 65,
                Name = "SaudaHoldTPFlowNotificationEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear Sir,</h2>" +
                                "<p style=\"margin-bottom:40px;\">Thank you for participating in the " + companyName + " sauda booking process. </p>" +
                                "<p style=\"margin-bottom:40px;\">Congratulations! Your sauda booking has been on hold. </p>" +
                                    "<p style=\"margin-bottom:40px;\">Sku: [SKU_NAME] for [QUANTITY]Case(s) at Rs.[PRICE] booked [BY_FOR] [USER_NAME]. </p>"
            }, new Entities.EmailTemplate
            {
                Id = 66,
                Name = "SaudaHoldTPFlowNotificationSMS",
                Template = "Value",
                PlainTemplate = "Dear Sir, Thank you for participating in the " + companyName + " sauda booking process. Congratulations! Your sauda booking has been on hold. Sku: [SKU_NAME] for [QUANTITY]Case(s) at Rs.[PRICE] booked [BY_FOR] [USER_NAME]."
            },new Entities.EmailTemplate
            {
                Id = 67,
                Name = "SaudaRejectTPFlowNotificationEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear Sir,</h2>" +
                                "<p style=\"margin-bottom:40px;\">Thank you for participating in the " + companyName + " sauda booking process. </p>" +
                                "<p style=\"margin-bottom:40px;\">The following sauda booking you placed have been rejected. </p>" +
                                    "<p style=\"margin-bottom:40px;\">Sku: [SKU_NAME] for [QUANTITY] at Rs.[PRICE] booked [BY_FOR] [USER_NAME]. </p>"
            }, new Entities.EmailTemplate
            {
                Id = 68,
                Name = "SaudaRejectTPFlowNotificationSMS",
                Template = "Value",
                PlainTemplate = "Dear Sir, Thank you for participating in the  " + companyName + " sauda booking process. The following sauda booking you placed has been rejected. Sku: [SKU_NAME] for [QUANTITY] at Rs.[PRICE] booked [BY_FOR] [USER_NAME]. - Team " + companyName + "",
                SMSTemplateID= "1707166210393624212"

            }, new Entities.EmailTemplate
            {
                Id = 69,
                Name = "RAFinalPricePublishNotificationEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear Sir,</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Price of [Date] from [FROM_TIME] to [TO_TIME] for all SKUs is been released. Kindly check your app for Price.</p>"

            }, new Entities.EmailTemplate
            {
                Id = 70,
                Name = "RAFinalPricePublishNotificationSMS",
                Template = "Value",
                PlainTemplate = "Dear Sir, Price of [Date] from [FROM_TIME] to [TO_TIME] for all SKUs is been released. Kindly check your app for Price."
            }, new Entities.EmailTemplate
            {
                Id = 71,
                Name = "SaudaCreationRAFlowEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear Sir,</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Thank you for participating in the " + companyName + " bidding process.</p>" +
                                    "<p style=\"margin-bottom:40px;\">Congratulations! Your bid has been accepted.</p>" +
                                    "<p style=\"margin-bottom:40px;\">Sku: [SKU_NAME] for [QUANTITY]Case(s) at Rs.[PRICE] bid [BY_FOR] [USER_NAME].</p>"
            }, new Entities.EmailTemplate
            {
                Id = 72,
                Name = "SaudaCreationRAFlowSMS",
                Template = "Value",
                PlainTemplate = "Dear Sir, Thank you for participating in the " + companyName + " bidding process. Congratulations! Your bid has been accepted. Sku: [SKU_NAME] for [QUANTITY]Case(s) at Rs.[PRICE] bid [BY_FOR] [USER_NAME]."
            }, new Entities.EmailTemplate
            {
                Id = 73,
                Name = "PriceConfigFinalPricePublishEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear Customer,</h2>" +
                                    "<p style=\"margin-bottom:20px;\">Indicative rates for today [Date] from [FROM_TIME] to [TO_TIME] </p>" +
                                    "<p style=\"margin-bottom:20px;\">[INCO_TERMS] tax paid </p>" +
                                    "<p style=\"margin-bottom:20px;\">Oil-Rate </p>" +
                                    "<p style=\"margin-bottom:20px;\">[SKU_PRICINGS] </p>" +
                                    "<p style=\"margin-bottom:40px;margin-top:20px;\">Company reserves the right to withdraw rates anytime</p>"
            }, new Entities.EmailTemplate
            {
                Id = 74,
                Name = "PriceConfigFinalPricePublishSMS",
                Template = "Value",
                PlainTemplate = "Dear Customer, Indicative rates for today [Date] from [FROM_TIME] to [TO_TIME], [INCO_TERMS] tax paid. Oil-Rate : [SKU_PRICINGS]. Company reserves the right to withdraw rates anytime."
            }, new Entities.EmailTemplate
            {
                Id = 75,
                Name = "SpecalityFatDiscountUserEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear User,</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Your Quantity Limit of Material : [SKU_NAME] from [FROM_DATE] to [TO_DATE] is [QUANTITY] MT.</p>"

            }, new Entities.EmailTemplate
            {
                Id = 76,
                Name = "SpecalityFatDiscountUserSMS",
                Template = "Value",
                PlainTemplate = "Dear User, Your Quantity Limit of Material : [SKU_NAME] from [FROM_DATE] to [TO_DATE] is [QUANTITY] MT."
            }, new Entities.EmailTemplate
            {
                Id = 77,
                Name = "SpecalityFatDiscountAcceptEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear User,</h2>" +
                                    "<p style=\"margin-bottom:20px;\">Your request for Speciality Fat Limit has been accepted. </p>" +
                                    "<p style=\"margin-bottom:40px;\">Speciality Fat Limit of Sku: [SKU_NAME] from [FROM_DATE] to [TO_DATE] is [QUANTITY] MT. </p>"
            }, new Entities.EmailTemplate
            {
                Id = 78,
                Name = "SpecalityFatDiscountAcceptSMS",
                Template = "Value",
                PlainTemplate = "Dear User, Your request for Speciality Fat Limit has been accepted. Speciality Fat Limit of Sku: [SKU_NAME] from [FROM_DATE] to [TO_DATE] is [QUANTITY] MT. "
            }, new Entities.EmailTemplate
            {
                Id = 79,
                Name = "SpecalityFatDiscountRejectEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear User,</h2>" +
                                    "<p style=\"margin-bottom:20px;\">Your request for Speciality Fat Limit has been rejected. </p>" +
                                    "<p style=\"margin-bottom:40px;\">Speciality Fat Limit of Sku: [SKU_NAME] from [FROM_DATE] to [TO_DATE] is [QUANTITY] MT. </p>"
            }, new Entities.EmailTemplate
            {
                Id = 80,
                Name = "SpecalityFatDiscountRejectSMS",
                Template = "Value",
                PlainTemplate = "Dear User, Your request for Speciality Fat Limit has been rejected. Speciality Fat Limit of Sku: [SKU_NAME] from [FROM_DATE] to [TO_DATE] is [QUANTITY] MT. "
            }, new Entities.EmailTemplate
            {
                Id = 81,
                Name = "SpecalityFatDiscountUserSaveEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear [USER_NAME],</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Specality Fat Discount for [USER_NAME], Sku: [SKU_NAME] is Rs.[ACTUAL_DISCOUNT].</p>"

            }, new Entities.EmailTemplate
            {
                Id = 82,
                Name = "SpecalityFatDiscountUserSaveSMS",
                Template = "Value",
                PlainTemplate = "Dear [USER_NAME], Specality Fat Discount for [USER_NAME], Sku: [SKU_NAME] is Rs.[ACTUAL_DISCOUNT]."
            }, new Entities.EmailTemplate
            {
                Id = 83,
                Name = "SupportIssueSubmittedEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">[USER_NAME] created an issue</h2>" +
                                    "<p style=\"margin-bottom:40px;\">[MESSAGE]</p>"


            }, new Entities.EmailTemplate
            {
                Id = 84,
                Name = "SupportIssueSubmittedSMS",
                Template = "Value",
                PlainTemplate = "Dear [USER_NAME], Support - Your query is been laced and contact admin for further information."
            }, new Entities.EmailTemplate
            {
                Id = 85,
                Name = "BiddingWindowStopedEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear User,</h2>" + "<p style=\"margin - bottom:40px;\"> [BIDDINGWINDOW_NAME] has been Stopped.</p>"
            }, new Entities.EmailTemplate
            {
                Id = 86,
                Name = "BiddingWindowStopedSms",
                Template = "Value",
                PlainTemplate = "Dear User, [BIDDINGWINDOW_NAME] has been stopped."
            }, new Entities.EmailTemplate
            {
                Id = 87,
                Name = "SaudaBiddingApprovedNotificationEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear Sir,</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Thank you for participating in the " + companyName + " bidding process.</p>" +
                                    "<p style=\"margin-bottom:40px;\">Congratulations! Your bid has been accepted.</p>" +
                                    "<p style=\"margin-bottom:40px;\">Sku: [SKU_NAME] bid [BY_FOR] [USER_NAME].</p>"
            }, new Entities.EmailTemplate
            {
                Id = 88,
                Name = "SaudaBiddingApprovedNotificationSMS",
                Template = "Value",
                PlainTemplate = "Dear Sir, Thank you for participating in the " + companyName + " bidding process. Congratulations! Your bid has been accepted. Sku: [SKU_NAME]  bid [BY_FOR] [USER_NAME]."
            }, new Entities.EmailTemplate
            {
                Id = 89,
                Name = "WindowPricePublishEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style =\"margin-bottom:30px; \">PRICE PUBLISHED: Dear Customer,</h2><p style=\"margin-bottom:40px; \"> Guarantee Price has been published for the bidding window ( <span style=\"font-style: italic; \"> [BIDDINGWINDOW_NAME] </span> ). Window start time <span style=\"font-style: italic; \">[BIDDINGWINDOW_STARTTIME]</span>, End time: <span style=\"font-style: italic; \">[BIDDINGWINDOW_ENDTIME]</span>. Please note that you can allocate the base SKU quantity to other required SKUs by <span style=\"font-style: italic; \">[SAUDAALLOCATIONTIME]</span>, failing which the bid SKU will be accepted as final.( happy bidding! ) </p>"
            }, new Entities.EmailTemplate
            {
                Id = 90,
                Name = "WindowPricePublishSMS",
                Template = "Value",
                PlainTemplate = "PRICE PUBLISHED: Dear Customer, Guarantee Price has been published for the bidding window ( [BIDDINGWINDOW_NAME] ). Window start time [BIDDINGWINDOW_STARTTIME], End time: [BIDDINGWINDOW_ENDTIME]. Please note that you can allocate the base SKU quantity to other required SKUs by [SAUDAALLOCATIONTIME], failing which the bid SKU will be accepted as final.( happy bidding! )"
            }
            , new Entities.EmailTemplate
            {
                Id = 91,
                Name = "WindowStoppedEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style =\"margin-bottom:30px; \">WINDOW STOPPED: Dear Customer,</h2><p style=\"margin-bottom:40px; \"> Bidding Window ( <span style=\"font-style: italic;\"> [BIDDINGWINDOW_NAME] </span> ) for timing <span style=\"font-style: italic;\">[BIDDINGWINDOW_STARTTIME]</span> to <span style=\"font-style: italic;\">[BIDDINGWINDOW_ENDTIME]</span> has been stopped for internal reasons. We shall intimate you on the upcoming bidding window timings.</p>"
            }, new Entities.EmailTemplate
            {
                Id = 92,
                Name = "WindowStoppedSMS",
                Template = "Value",
                PlainTemplate = "WINDOW STOPPED: Dear Customer, Bidding Window ( [BIDDINGWINDOW_NAME] )for timing [BIDDINGWINDOW_STARTTIME] to [BIDDINGWINDOW_ENDTIME] has been stopped for internal reasons. We shall intimate you on the upcoming bidding window timings."
            }, new Entities.EmailTemplate
            {
                Id = 93,
                Name = "SurpriseDiscountEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px; \">SURPRISE DISCOUNT: Dear Customer,</h2><p style=\"margin-bottom:40px;\"> Congratulations! Your today's successful bid(s) is entitled for the following surprise discount: [SKU_NAME]. [BENEFIT_TYPE] Benefit - [BENEFIT] - [DISCOUNTORDAYS] [PERCASE_OR_DAYS].</p>"
            }, new Entities.EmailTemplate
            {
                Id = 94,
                Name = "SurpriseDiscountSMS",
                Template = "Value",
                PlainTemplate = "SURPRISE DISCOUNT: Dear Customer, Congratulations! Your today's successful bid(s) is entitled for the following surprise discount: [SKU_NAME]. [BENEFIT_TYPE] Benefit - [BENEFIT] - [DISCOUNTORDAYS] [PERCASE_OR_DAYS].",
            }, new Entities.EmailTemplate
            {
                Id = 95,
                Name = "WindowCreationEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style =\"margin-bottom:30px; \">WINDOW CREATION: Dear Customer,</h2><p style=\"margin-bottom:40px;\">Bidding Window ( <span style=\"font-style: italic; \"> [BIDDINGWINDOW_NAME] </span> ) has been created. Window start time <span style=\"font-style: italic; \">[BIDDINGWINDOW_STARTTIME]</span>, End time: <span style=\"font-style: italic; \">[BIDDINGWINDOW_ENDTIME]</span>. Please note that you can allocate the base SKU quantity to other required SKUs by <span style=\"font-style: italic; \">[SAUDAALLOCATIONTIME]</span>, failing which the bid SKU will be accepted as final.</p>"
            }, new Entities.EmailTemplate
            {
                Id = 96,
                Name = "WindowCreationSMS",
                Template = "Value",
                PlainTemplate = "WINDOW CREATION: Dear Customer, Bidding Window ( [BIDDINGWINDOW_NAME] ) has been created. Window start time [BIDDINGWINDOW_STARTTIME], End time: [BIDDINGWINDOW_ENDTIME]. Please note that you can allocate the base SKU quantity to other required SKUs by [SAUDAALLOCATIONTIME], failing which the bid SKU will be accepted as final."
            },
            new Entities.EmailTemplate
            {
                Id = 97,
                Name = "WindowCompletedEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style =\"margin-bottom:30px;\">Dear Customer,</h2>" + "<p style=\"margin - bottom:40px;\"> Bidding window ( <span style=\"font-style: italic; \">[BIDDINGWINDOW_NAME]</span> ) for timing <span style=\"font-style: italic; \">[BIDDINGWINDOW_STARTTIME] to [BIDDINGWINDOW_ENDTIME]</span> has been completed. Please note that you can allocate the base SKU quantity to other required SKUs by <span style=\"font-style: italic; \">[SAUDAALLOCATIONTIME]</span>, failing which the bid SKU will be accepted as final. </p>"
            }, new Entities.EmailTemplate
            {
                Id = 98,
                Name = "WindowCompletedSMS",
                Template = "Value",
                PlainTemplate = "Dear Customer, Bidding window ( [BIDDINGWINDOW_NAME] ) for timing [BIDDINGWINDOW_STARTTIME] to [BIDDINGWINDOW_ENDTIME] has been completed. Please note that you can allocate the base SKU quantity to other required SKUs by [SAUDAALLOCATIONTIME], failing which the bid SKU will be accepted as final."
            }, new Entities.EmailTemplate
            {
                Id = 99,
                Name = "VolumeCapacity",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear User,</h2>" + "<p style=\"margin - bottom:40px;\"> <table> <tr> OilType[BIDDINGWINDOW_OILTYPE], Total Volume Capacity <b>[BIDDINGWINDOW_TOTALVOLUMECAPACITY] </b> ,Remaining Volume Capacity <b>  [BIDDINGWINDOW_REMAININGVOLUMECAPACITY] </b> </tr > </table > </p> "
            },
            new Entities.EmailTemplate
            {
                Id = 100,
                Name = "WindowCompletedNotParticipatedEmail",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style =\"margin-bottom:30px;\">WINDOW COMPLETED : Dear Customer,</h2>" + "<p style=\"margin - bottom:40px;\"> Bidding Window  ( <span style=\"font-style: italic; \">[BIDDINGWINDOW_NAME]</span> ) for timing [BIDDINGWINDOW_STARTTIME] to [BIDDINGWINDOW_ENDTIME] has been completed.  Seems like you have not bidded for this window. Looking forward to your participation in the upcoming windows. </p>"
            }, new Entities.EmailTemplate
            {
                Id = 101,
                Name = "WindowCompletedNotParticipatedSMS",
                Template = "Value",
                PlainTemplate = "WINDOW COMPLETED : Dear Customer, Bidding Window  ( [BIDDINGWINDOW_NAME] ) for timing [BIDDINGWINDOW_STARTTIME] to [BIDDINGWINDOW_ENDTIME] has been completed.  Seems like you have not bidded for this window. Looking forward to your participation in the upcoming windows."
            },
             new Entities.EmailTemplate
             {
                 Id = 102,
                 Name = "ConversionUnitAndDiffRateEmail",
                 Template = htmlTemplateString,
                 PlainTemplate = "<p>Dear Admin, <br> The following SKUs' Unit and Basic rate difference are missing in Master (Transaction -> Sauda Conversion Unit and Basic rate) </p><br><br>" +
                                    "<div style='padding-bottom: 50px;'><table text - align = left border = 1  width = 100% align = center cellpadding = 10 style = 'border-collapse:collapse' ><tr><th width = 30 % style = 'padding: 10px;' ><b> From Sku Code </b></th><th width = 70 % style = 'padding: 10px;' ><b> From Sku Name </b></th><tr> " +
                                    "[TableContent1]" +
                                    "</table></div><br><br><div style='padding-bottom: 50px;'><table text - align = left border = 1  width = 100% align = center cellpadding = 10 style = 'border-collapse:collapse' ><tr><th width=30% style='padding: 10px;'><b>To Sku Code</b></th><th width=70% style='padding: 10px;'><b>To Sku Name</b></th><tr>" +
                                    "[TableContent2]" +
                                    "</table></div><br><br>"
             }, new Entities.EmailTemplate
             {
                 Id = 103,
                 Name = "SaudaExtensionEmail",
                 Template = htmlTemplateString,
                 PlainTemplate = "Dear Admin,The following states have not been mapped in the master screen(In Menu - Masters-> Extension Policy). So some of the Saudas can't be extended."
             }, new Entities.EmailTemplate
             {
                 Id = 104,
                 Name = "AboutWindowEndSms",
                 Template = "Value",
                 PlainTemplate = "Dear Customer, The Bidding window time is about to end in [BiddingWindowTimeToEnd].So you can bid only untill the window ends."
             }, new Entities.EmailTemplate
             {
                 Id = 105,
                 Name = "CustomerCounterBidofferSms",
                 Template = "Value",
                 PlainTemplate = "Dear Customer, The Counter bid offer will expire when the bidding window ends.The window is about to end in [BiddingWindowTimeToEnd],so you use offer only untill the window ends."
            }, new Entities.EmailTemplate
            {
                Id = 106,
                Name = "Form Approval Status updated",
                Template = "Value",
                PlainTemplate = "Form Id [FORMID] was [STATUS] by [USER_NAME]"
            }, new Entities.EmailTemplate
            {
                Id = 107,
                Name = "Form Status updated",
                Template = "Value",
                PlainTemplate = "Form Id [FORMID]'s status was changed to [STATUS] by [USER_NAME]"
            }, new Entities.EmailTemplate
            {
                Id = 108,
                Name = "UnderstandingFormSubmit",
                Template = "Value",
                PlainTemplate = "New understanding form was submitted against form Id [FORMID]"
            }, new Entities.EmailTemplate
            {
                Id = 109,
                Name = "New Form assigned",
                Template = "Value",
                PlainTemplate = "[FORMNAME] was assigned to you."
            }, new Entities.EmailTemplate
            {
                Id = 110,
                Name = "Demo scheduled",
                Template = "Value",
                PlainTemplate = "Demo scheduled on [Date] against Form Id [FORMID]"
            }, new Entities.EmailTemplate
            {
                Id = 111,
                Name = "Demo rescheduled",
                Template = "Value",
                PlainTemplate = "Demo rescheduled on [Date] against Form Id [FORMID]"
            }, new Entities.EmailTemplate
            {
                Id = 112,
                Name = "ComplaintFormSubmit",
                Template = "Value",
                PlainTemplate = "New complaint form was submitted against form Id [FORMID]"
            }, new Entities.EmailTemplate
            {
                Id = 113,
                Name = "SaudaExpiryNoification",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear [USER_NAME],</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Greetings for the day!</p><br><br>"+
                                    "<p style=\"margin-bottom:40px;\">Your Sauda Order Number([OrderNumber]) placed on ([date])is getting expired on ([Expirydate]). Kindly raise sales orders on or before expiry. In case of further delay or any other queries, please contact your respective person of contact.</p><br><br>",                                    
                SMSTemplate = "Dear [USER_NAME], Greetings for the day! Your Sauda Order Number([OrderNumber]) placed on ([Date])is getting expired on ([Expirydate]). Kindly raise sales orders on or before expiry. In case of further delay or any other queries, please contact your respective person of contact.</p><br><br>",
                SMSTemplateID= "1707166416827439041"
            }, new Entities.EmailTemplate
            {
                Id = 114,
                Name = "OverDueNotification",
                Template = htmlTemplateString,
                PlainTemplate = "<h2 style=\"margin-bottom:30px;\">Dear [USER_NAME],</h2>" +
                                    "<p style=\"margin-bottom:40px;\">Greetings for the day!</p><br><br>" +
                                    "<p style=\"margin-bottom:40px;\">We would like to remind you that[Amount] sum is due for payment since[Date]. For more information, contact Admin.</p><br><br>",
                                    
                SMSTemplate = "Dear [USER_NAME], We would like to remind you that[Amount] sum is due for payment since[Date]. For more information, contact Admin.Regards, Team " + companyName + "",
                SMSTemplateID= "1707166416815986054"
            }
           

          ) ;
        }
    }
}
