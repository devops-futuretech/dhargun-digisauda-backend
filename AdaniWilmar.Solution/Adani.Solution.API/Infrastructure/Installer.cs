using System.Web.Http;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using GMCore.Logger;
using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Service;
using Adani.Solution.Service.Common;

namespace Adani.Solution.API.Infrastructure
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly().BasedOn<ApiController>().LifestylePerWebRequest());
            //container.Register(Component.For<ILogger>().ImplementedBy<Logging>());
            //Register Context
            container.Register(Component.For<IAdaniContext>().ImplementedBy<AdaniContext>().LifestyleTransient());

            //Register Services
            container.Register(Component.For<IAdminService>().ImplementedBy<AdminService>().LifestyleTransient());
            container.Register(Component.For<IAuthenticateService>().ImplementedBy<AuthenticateService>().LifestyleTransient());
            container.Register(Component.For<IAuthorizeService>().ImplementedBy<AuthorizeService>().LifestyleTransient());
            container.Register(Component.For<IUserService>().ImplementedBy<UserService>().LifestyleTransient());
            container.Register(Component.For<INotificationService>().ImplementedBy<NotificationService>().LifestyleTransient());
            container.Register(Component.For<IEmployeeService>().ImplementedBy<EmployeeService>().LifestyleTransient());

            container.Register(Component.For<IMasterService>().ImplementedBy<MasterService>().LifestyleTransient());
            container.Register(Component.For<IPricingService>().ImplementedBy<PricingService>().LifestyleTransient());
            container.Register(Component.For<ILookupService>().ImplementedBy<LookupService>().LifestyleTransient());
            container.Register(Component.For<ISalesTourPlanService>().ImplementedBy<SalesTourPlanService>().LifestyleTransient());
            container.Register(Component.For<IReverseAuctionService>().ImplementedBy<ReverseAuctionService>().LifestyleTransient());
            container.Register(Component.For<ISaudaServices>().ImplementedBy<SaudaServices>().LifestyleTransient());
            container.Register(Component.For<ILiftingService>().ImplementedBy<LiftingService>().LifestyleTransient());

            container.Register(Component.For<ISAPIntegrationService>().ImplementedBy<SAPIntegrationService>().LifestyleTransient());

            container.Register(Component.For<ITradeTicketService>().ImplementedBy<TradeTicketService>().LifestyleTransient());

            container.Register(Component.For<IResultService>().ImplementedBy<ResultService>().LifestyleTransient());
            container.Register(Component.For<IImportService>().ImplementedBy<ImportService>().LifestyleTransient());
            container.Register(Component.For<IMobileFinalPriceService>().ImplementedBy<MobileFinalPriceService>().LifestyleTransient());
            container.Register(Component.For<IMobileReverseAuctionService>().ImplementedBy<MobileReverseAuctionService>().LifestyleTransient());
            container.Register(Component.For<IMobileSaudaServices>().ImplementedBy<MobileSaudaService>().LifestyleTransient());
            container.Register(Component.For<IFinalPriceService>().ImplementedBy<FinalPriceService>().LifestyleTransient());
            container.Register(Component.For<IMobileSalesService>().ImplementedBy<MobileSalesService>().LifestyleTransient());
            container.Register(Component.For<IMobileDashboardServices>().ImplementedBy<MobileDashboardService>().LifestyleTransient());
            container.Register(Component.For<IMobileDealerSalesService>().ImplementedBy<MobileDealerSalesService>().LifestyleTransient());
            container.Register(Component.For<IMobileDealerSaudaService>().ImplementedBy<MobileDealerSaudaService>().LifestyleTransient());
            container.Register(Component.For<IMobileDealerStockService>().ImplementedBy<MobileDealerStockService>().LifestyleTransient());
            container.Register(Component.For<IMobileDealerDashboardServices>().ImplementedBy<MobileDealerDashboardService>().LifestyleTransient());
            container.Register(Component.For<IMobileSTPService>().ImplementedBy<MobileSTPService>().LifestyleTransient());
            container.Register(Component.For<ISurveyUpdatesService>().ImplementedBy<SurveyUpdatesService>().LifestyleTransient());
            container.Register(Component.For<IMobileUpdateService>().ImplementedBy<MobileUpdateService>().LifestyleTransient());
            container.Register(Component.For<IMediaService>().ImplementedBy<MediaService>().LifestyleTransient());
            container.Register(Component.For<IMobileApprovalService>().ImplementedBy<MobileApprovalService>().LifestyleTransient());
            container.Register(Component.For<ISupportService>().ImplementedBy<SupportService>().LifestyleTransient());
            container.Register(Component.For<IReportService>().ImplementedBy<ReportService>().LifestyleTransient());
            container.Register(Component.For<ISftpConnectorService>().ImplementedBy<SftpConnectorService>().LifestyleTransient());

            container.Register(Component.For<IZonalHeadService>().ImplementedBy<ZonalHeadService>().LifestyleTransient());
            //container.Register(Component.For<IRALookupService>().ImplementedBy<RALookupService>().LifestyleTransient());

            container.Register(Component.For<IRAVersionTwoService>().ImplementedBy<RAVersionTwoService>().LifestyleTransient());
            container.Register(Component.For<INationalHeadService>().ImplementedBy<NationalHeadService>().LifestyleTransient());

            container.Register(Component.For<IChatBotService>().ImplementedBy<ChatBotService>().LifestyleTransient());
            
            container.Register(Component.For<IDynamicFormService>().ImplementedBy<DynamicFormService>().LifestyleTransient());
            container.Register(Component.For<ISapAuthorizeService>().ImplementedBy<SapAuthorizeService>().LifestyleTransient());
            container.Register(Component.For<IQpsService>().ImplementedBy<QpsService>().LifestyleTransient());
            container.Register(Component.For<IRoleService>().ImplementedBy<RoleService>().LifestyleTransient());
            container.Register(Component.For<ICrossAndUpsellService>().ImplementedBy<CrossAndUpsellService>().LifestyleTransient());
        }
    }
}