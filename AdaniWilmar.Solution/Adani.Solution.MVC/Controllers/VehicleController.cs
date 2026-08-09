using Adani.Solution.DTO;
using Adani.Solution.MVC.Attributes;
using Adani.Solution.MVC.Helpers;
using Adani.Solution.MVC.ServiceClient;
using GMCore.Helper;
using GMCore.Logger;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Adani.Solution.MVC.Controllers
{
    [TokenAuthorize]
    [CustomRedirect]
    [NoCache]
    public class VehicleController : BaseController
    {

        private const string ServiceName = "Vehicle Controller";
        private readonly VehicleClient _vehicleClient;
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;

        public VehicleController()
        {
            _vehicleClient = new VehicleClient { ControllerDelegate = this };
        }

        // GET: Vehicle
        public ActionResult VehicleTrackingList()
        {
            RoleIdDto roleIdDto = new RoleIdDto()
            {
                VerticalId = VerticalId,
                RoleId = RoleId,
                LoginUserId = UserId,
                OrganizationReportingToId = OrganizationReportingToId
            };

            return View(roleIdDto);
        }
        public ActionResult TrackRedirect(string Donumber)
        {
            Session["DoNumber"] = Donumber;
            return RedirectToAction("VehicleTrackViewData", "Vehicle");
        }
        public async Task<ActionResult> VehicleTrackViewData()
        {
            TrackingViewDto output = new TrackingViewDto();
            if (Session["DoNumber"] != null)
            {
                var donumber = Session["DoNumber"].ToString();
                if (Session["vehicleTrackingData"] != null)
                {
                    var vehicleData = JsonConvert.DeserializeObject<VehicleTrackingDto>(Session["vehicleTrackingData"].ToString());
                    output.DeliveryDetail = vehicleData.data.do_details.Where(_ => _.do_number == donumber).FirstOrDefault();
                    output.DealerCode = vehicleData.data.dealer_code;
                    output.DealerName = vehicleData.data.dealer_name;
                    
                }
                _logger.Info($" Input Data : {JsonHelper.ConvertObjectToJson(donumber)}");
                if (Session["LiftingData"] != null)
                {
                    var liftingList = JsonConvert.DeserializeObject<List<DONumberddlDto>>(Session["LiftingData"].ToString());
                    long liftingId = 0;
                    var inputDto = new LiftingSkuInputDto();
                    
                    var skudata = new List<TrackSkuOutputDto>();
                   
                    if (Session["MaterialData"] != null)
                    {
                        skudata = JsonConvert.DeserializeObject<List<TrackSkuOutputDto>>(Session["MaterialData"].ToString());
                        _logger.Info($" SKU Data : {JsonHelper.ConvertObjectToJson(skudata)}");
                        
                    }
                    else
                    {
                        var skuinputdto = new LiftingSkuInputDto();
                        skuinputdto.IsLiftingId = false;

                        skuinputdto.DoNumbers = new List<DONumberDto>()
                        {
                            new DONumberDto()
                            {
                                DoNumber=output.DeliveryDetail.do_number,
                                Status=output.DeliveryDetail.status_body.status
                            }
                        };
                        skudata = await _vehicleClient.GetSkuDataWithLiftingandDoNumber(skuinputdto);
                        _logger.Info($" SKU Data : {JsonHelper.ConvertObjectToJson(skudata)}");
                    }
                    if (liftingList.Any())
                    {
                        _logger.Info($" Input Data : {JsonHelper.ConvertObjectToJson(liftingList)}");
                        liftingId = liftingList.FirstOrDefault(_ => _.Value == donumber)!=null? liftingList.FirstOrDefault(_ => _.Value == donumber).Id : 0;
                        if(liftingId > 0)
                        {
                            inputDto.LiftingId = liftingId;
                            inputDto.IsLiftingId = true;
                            //output.SkuList = await _vehicleClient.GetSkuDataWithLiftingandDoNumber(inputDto);
                            output.SkuList = skudata.FirstOrDefault(_ => _.DoNumber==donumber) !=null ? skudata.FirstOrDefault(_ => _.DoNumber == donumber).Materials: new List<TrackSkuDto>();
                            output.ShipToParty = skudata.FirstOrDefault(_ => _.DoNumber == donumber) != null ? skudata.FirstOrDefault(_ => _.DoNumber == donumber).ShipToParty : string.Empty;
                        }
                        else
                        {
                            _logger.Info($" SKU Data : {JsonHelper.ConvertObjectToJson(inputDto)}");
                            inputDto.DoNumber = donumber;
                            inputDto.IsLiftingId = false;
                            //output.SkuList = await _vehicleClient.GetSkuDataWithLiftingandDoNumber(inputDto);
                            output.SkuList = skudata.FirstOrDefault(_ => _.DoNumber == donumber) !=null ? skudata.FirstOrDefault(_ => _.DoNumber == donumber).Materials : new List<TrackSkuDto>();
                            output.ShipToParty = skudata.FirstOrDefault(_ => _.DoNumber == donumber) != null ? skudata.FirstOrDefault(_ => _.DoNumber == donumber).ShipToParty : string.Empty;
                            _logger.Info($" SKU Data : {JsonHelper.ConvertObjectToJson(output.SkuList)}");
                        }
                    }
                    else
                    {
                        _logger.Info($" Input Data : {JsonHelper.ConvertObjectToJson(liftingList)}");
                        inputDto.DoNumber = donumber;
                        inputDto.IsLiftingId = false;
                        //output.SkuList = await _vehicleClient.GetSkuDataWithLiftingandDoNumber(inputDto);
                        output.SkuList = skudata.FirstOrDefault(_ => _.DoNumber == donumber) != null ? skudata.FirstOrDefault(_ => _.DoNumber == donumber).Materials : new List<TrackSkuDto>();
                    }
                    Session["IsNavigation"] = JsonConvert.SerializeObject(true);
                }

            }
            

            return View(output);
        }
        public async Task<ActionResult> GetVehicleTrackingData(VehicleInputDto inputDto, [DataSourceRequest] DataSourceRequest request)
        {
           List<VehicleStatusDto> vehicleData = new List<VehicleStatusDto>();
            //inputDto.CustomerCodeList = "2116683";
            //inputDto.DoNumberList = "8004704168,8004711743,8004711750,8004713923";
            try
            {
                if (!string.IsNullOrEmpty(inputDto.CustomerCodeList) && !string.IsNullOrEmpty(inputDto.DoNumberList))
                {
                    var token = await _vehicleClient.GetToken();
                    if (token != null && token.data!=null)
                    {
                        var data = await _vehicleClient.GetVehicleTrackinStatusData(token.data.token, inputDto.CustomerCodeList, inputDto.DoNumberList);
                        var billingdolist = new List<DONumberddlDto>();
                        if (data.data != null)
                        {
                            if(Session["LiftingData"] != null)
                            {
                                billingdolist = JsonConvert.DeserializeObject<List<DONumberddlDto>>(Session["LiftingData"].ToString());
                            }

                            vehicleData = data.data.do_details.Where(_ => _.do_number != null)
                                .Select(item => new VehicleStatusDto()
                                {
                                    BillingNumber = billingdolist.FirstOrDefault(_ => _.Value == item.do_number) != null ? billingdolist.FirstOrDefault(_ => _.Value == item.do_number).BillingNo : string.Empty,
                                arrived_date_time = item.status_body.current_status,
                                    distance_covered = item.status_body.distance_covered,
                                    current_status = item.status_body.current_status,
                                    last_known_date_time = item.status_body.last_known_date_time,
                                    last_known_location = item.status_body.last_known_location,
                                    trip_closed_date_time = item.status_body.trip_closed_date_time,
                                    latitude = item.status_body.latitude,
                                    longitude = item.status_body.longitude,
                                    status = item.status_body.status,
                                    DoNumber = item.do_number,
                                    eta = item.status_body.eta,
                                    VehicleNumber = item.vehicle_number,
                                    dealer_code = data.data.dealer_code,
                                    dealer_name = data.data.dealer_name
                                }).ToList();
                            var skuinputdto = new LiftingSkuInputDto();
                            skuinputdto.IsLiftingId = false;

                            skuinputdto.DoNumbers = data.data.do_details.Where(_ => _.do_number != null)
                                .Select(s => new DONumberDto()
                                {
                                    DoNumber=s.do_number,
                                    Status=s.status_body.status,
                                }).ToList();

                            var skudatas= await _vehicleClient.GetSkuDataWithLiftingandDoNumber(skuinputdto);
                            if (skudatas.Any())
                            {
                                Session["MaterialData"] = JsonConvert.SerializeObject(skudatas);
                                _logger.Info($" SKU Data : {JsonHelper.ConvertObjectToJson(skudatas)}");
                            }
                            //for (int i = 0; i < data?.do_details.Count; i++)
                            //{
                            //    DealerOrderDetailsDto item = data.do_details[i];
                            //    if (item.do_number != null)
                            //    {

                            //        var vehicleStatusdata = new VehicleStatusDto
                            //        {
                            //            arrived_date_time = item.status_body.current_status,
                            //            distance_covered = item.status_body.distance_covered,
                            //            current_status = item.status_body.current_status,
                            //            last_known_date_time = item.status_body.last_known_date_time,
                            //            last_known_location = item.status_body.last_known_location,
                            //            trip_closed_date_time = item.status_body.trip_closed_date_time,
                            //            latitude = item.status_body.latitude,
                            //            longitude = item.status_body.longitude,
                            //            status = item.status_body.status,
                            //            DoNumber = item.do_number,
                            //            eta = item.status_body.eta,
                            //            VehicleNumber = item.vehicle_number,
                            //            dealer_code = data.dealer_code,
                            //            dealer_name = data.dealer_name
                            //        };

                            //        vehicleData.Add(vehicleStatusdata);
                            //    }
                            //}
                        }

                        Session["vehicleTrackingData"] = JsonConvert.SerializeObject(data);

                        Session["IsNavigation"] = JsonConvert.SerializeObject(false);
                        Session["GridData"] = JsonConvert.SerializeObject(vehicleData);
                    }

                }


                if (Session["IsNavigation"]!=null && Convert.ToBoolean(Session["IsNavigation"]))
                {
                    if (Session["GridData"] != null)
                    {
                        vehicleData = JsonConvert.DeserializeObject<List<VehicleStatusDto>>(Session["GridData"].ToString());
                    }
                    
                }

                return Json(vehicleData.ToDataSourceResult(request));
            }
            catch (Exception)
            {
                return Json(vehicleData.ToDataSourceResult(request));
            }
        }

        public ActionResult GetVehicleTrackingStatusData(string donumber, [DataSourceRequest] DataSourceRequest request)
        {
            VehicleTrackingDto vehicleData = new VehicleTrackingDto();

            try
            {
                if (Session["vehicleTrackingData"].ToString() != null)
                {
                    vehicleData = JsonConvert.DeserializeObject<VehicleTrackingDto>(Session["vehicleTrackingData"].ToString());
                    return Json(vehicleData.data.do_details.Where(_ => _.do_number == donumber).ToDataSourceResult(request));
                }

                return Json(vehicleData.data.do_details.ToDataSourceResult(request));

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return Json(vehicleData.data.do_details.ToDataSourceResult(request));
            }
        }

        public async Task<ActionResult> VehicleTrackingView(string VehicleTrackingLink)
        {
            DealerOrderDetailsDto vehicleData = new DealerOrderDetailsDto();

            try
            {
                if(Session["DoNumber"] != null)
                {
                    vehicleData.do_number = Session["DoNumber"].ToString();
                }
                if(!string.IsNullOrEmpty(VehicleTrackingLink))
                {
                    vehicleData.tracking_link = VehicleTrackingLink;
                }

                return View(vehicleData);
            }
            catch (Exception)
            {
                return View();
            }
        }

        public async Task<ActionResult> GetDoNumberByDealerIDsddl(string selectedIds)
        {

            var result = await _vehicleClient.GetDONumberListByDistributorId(selectedIds);

            //Session["LiftingData"] = JsonConvert.SerializeObject(result);
            if (result.Any())
            {
                var list = result
                    .SelectMany(dto => dto.Value.Split(',').Select(value => new DONumberddlDto { BillingNo = dto.BillingNo, Value = value }))
                    .ToList();
                Session["LiftingData"] = JsonConvert.SerializeObject(list);
                //result = result.GroupBy(s => s.BillingNo).Select(s => new DONumberddlDto()
                //{
                //    Value = String.Join(",",s.Select(_ => _.Value).Distinct()),
                //    BillingNo = s.Key,
                //}).Distinct().ToList();
            }
            

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}