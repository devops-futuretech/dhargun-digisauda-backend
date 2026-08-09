using GMCore.Logger;
using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using GMCore.Helper;
using Kendo.Mvc.Extensions;

namespace Adani.Solution.Service
{
    public interface IAdminService
    {
        ResultDto AddRole(RoleDto roleDto);
        ResultDto GetRoles();
        //ResultDto DeleteRole(RoleIdDto roleIdDto);

        //Role
        ResultDto AddClaim(ClaimDto claimDto);
        ResultDto GetClaims();
        ResultDto AddRoleType(RoleTypeDto roleTypeDto);
        ResultDto AddRoleTypeClaim(RoleTypeClaimDto roleTypeClaimDto);
        ResultDto AddRoleClaim(RoleClaimDto roleClaimDto);
        ResultDto GetRoleTypeClaims(RoleTypeUsersDto roleTypeIdDto);
        ResultDto GetAllRoleTypeClaims();
        ResultDto GetRoleName(int roleId);
        ResultDto GetClaimName(int claimId);
        ResultDto GetRoleTypes();
        ResultDto GetAllRoleClaims();
        ResultDto UpdateRoleClaims(RoleClaimUpdateDto roleClaimUpdateDto);
        ResultDto UpdateRoleTypeClaims(RoleTypeClaimUpdateDto roleTypeClaimUpdateDto);
        ResultDto UpdateRoleTypeHierarchy(RoleTypeHierarchyDto roleTypeHierarchyDto);
        ResultDto GetOrganizationHierarchy();
        ResultDto DeleteRoleTypeAndClaims(RoleTypeIdDto roleTypeIdDto);
        ResultDto DeleteRoleAndClaims(RoleIdDto roleIdDto);
        ResultDto GetReportingToRoles(IdInputDto inputDto);
        //ResultDto AddRole(RoleDto roleDto);
        //ResultDto GetRoles();

        //Role/Process Hierarchy

        ResultDto GetRoleHierarchyByProcess(RoleHierarchyParamDto inputDto);
        ResultDto AddOrUpdateRoleHierarchy(RoleHierarchyDto inputDto);
        ResultDto GetReportingToUsersByRole(ReportingUsersInputDto inputDto);
        ResultDto GetClaimsByRoleId(RoleIdDto inputDto);
        ResultDto GetClaimsByRoleTypeId(RoleIdDto inputDto);

        #region Reporting To Users

        ResultDto GetOrganizationReportingToUsersByUserId(ReportingUsersInputDto inputDto);
        ResultDto GetSalesReportingToUsersByUserId(ReportingUsersInputDto inputDto);
        ResultDto GetSalesReportingToUsersByCityId(ReportingUsersInputDto inputDto);
        ResultDto GetSalesReportingToUsersByCityStateDistrict(ReportingUsersInputDto inputDto);

        ResultDto GetReportingToZonalHeadUsersByUserId(ReportingUsersInputDto inputDto);
        ResultDto GetReportingToBDOUsersByUserId(ReportingUsersInputDto inputDto);

        ResultDto GetReportingToRABDOUsersByUserId(ReportingUsersInputDto inputDto);

        #endregion
    }

    public class AdminService : IAdminService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Admin Service");
        private readonly IResultService _resultService;
        private const string ServiceName = "Lookup Service";
        private string _methodName;

        //ILookupService lookupService
        public AdminService(IAdaniContext salesContext, IResultService resultService)
        {
            try
            {
                _emamiContext = salesContext;
                _resultService = resultService;
                _emamiContext.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for Lookup Service", exception);
            }
        }

        #region Roles & Claims

        public ResultDto AddRoleType(RoleTypeDto roleTypeDto)
        {
            _methodName = "AddRoleType";
            var resultDto = new ResultDto();
            try
            {
                if (roleTypeDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (string.IsNullOrEmpty(roleTypeDto.Name))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RoleEmpty;
                    return resultDto;
                }
                if (roleTypeDto.LoginUserId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.UserIdMissing;
                    return resultDto;
                }
                var roleNameContext = _emamiContext.RoleTypes.AsNoTracking().Count(_ => _.Name == roleTypeDto.Name);
                if (roleNameContext > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RoleTypeNameExist;
                    return resultDto;
                }
                var roleTypeContext = new RoleType
                {
                    Name = roleTypeDto.Name.Trim(),
                    Description = roleTypeDto.Description,
                    IsActive = true,
                    CreatedBy = roleTypeDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow) //_lookupService.IsIndiaTimeZone() ? DateHelper.UtcToIndia(DateTime.UtcNow) : DateTime.UtcNow
                };
                _emamiContext.RoleTypes.Add(roleTypeContext);
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.RecordSaved;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto AddRole(RoleDto roleDto)
        {
            _methodName = "AddRole";
            var resultDto = new ResultDto();
            try
            {
                if (roleDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (string.IsNullOrEmpty(roleDto.Name))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RoleEmpty;
                    return resultDto;
                }
                if (roleDto.RoleTypeId == 0)
                {
                    return _resultService.ErrorMessage(Constants.RoleTypeEmpty);
                }
                if (roleDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var roleNameContext = _emamiContext.Roles.AsNoTracking().Count(_ => _.Name == roleDto.Name);
                if (roleNameContext > 0)
                {
                    return _resultService.ErrorMessage(Constants.RoleNameExist);
                }
                var roleContext = new Role
                {
                    Name = roleDto.Name.Trim(),
                    Description = roleDto.Description,
                    RoleTypeId = roleDto.RoleTypeId,
                    IsActive = true,
                    CreatedBy = roleDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow) //lookupService.IsIndiaTimeZone() ? DateHelper.UtcToIndia(DateTime.UtcNow) : DateTime.UtcNow
                };
                _emamiContext.Roles.Add(roleContext);
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.RecordSaved;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto AddClaim(ClaimDto claimDto)
        {
            _methodName = "AddClaim";
            var resultDto = new ResultDto();
            try
            {
                if (claimDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (string.IsNullOrEmpty(claimDto.Name))
                {
                    return _resultService.ErrorMessage(Constants.ClaimEmpty);
                }
                var claimNameContext = _emamiContext.Claims.AsNoTracking().Count(_ => _.Name == claimDto.Name);
                if (claimNameContext > 0)
                {
                    return _resultService.ErrorMessage(Constants.ClaimNameExist);
                }
                var claimContext = new Claim
                {
                    Name = claimDto.Name.Trim(),
                    Description = claimDto.Description,
                    IsActive = true
                };
                _emamiContext.Claims.Add(claimContext);
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.GetMessage(Constants.RecordSaved, Utility.MessageLanguage);
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto AddRoleTypeClaim(RoleTypeClaimDto roleTypeClaimDto)
        {
            _methodName = "AddRoleTypeClaim";
            var resultDto = new ResultDto();
            try
            {
                if (roleTypeClaimDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (string.IsNullOrEmpty(roleTypeClaimDto.RoleType.Name))
                {
                    return _resultService.ErrorMessage(Constants.RoleTypeEmpty);
                }
                if (roleTypeClaimDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var roleTypeNameContext = _emamiContext.RoleTypes.AsNoTracking().Count(_ => _.Name == roleTypeClaimDto.RoleType.Name);
                if (roleTypeNameContext > 0)
                {
                    return _resultService.ErrorMessage(Constants.RoleTypeNameExist);
                }
                //Save RoleType
                var newRoleTypeContext = new RoleType
                {
                    Name = roleTypeClaimDto.RoleType.Name.Trim(),
                    Description = roleTypeClaimDto.RoleType.Description,
                    IsActive = true,
                    CreatedBy = roleTypeClaimDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow) //_lookupService.IsIndiaTimeZone() ? DateHelper.UtcToIndia(DateTime.UtcNow) : DateTime.UtcNow

                };
                _emamiContext.RoleTypes.Add(newRoleTypeContext);
                _emamiContext.SaveChanges();

                //save RoleClaim
                foreach (var claimId in roleTypeClaimDto.ClaimIds)
                {
                    var newRoleTypeClaimContext = new RoleTypeClaim
                    {
                        RoleTypeId = newRoleTypeContext.Id,
                        ClaimId = claimId,
                        CreatedBy = roleTypeClaimDto.LoginUserId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow) //_lookupService.IsIndiaTimeZone() ? DateHelper.UtcToIndia(DateTime.UtcNow) : DateTime.UtcNow
                    };
                    _emamiContext.RoleTypeClaims.Add(newRoleTypeClaimContext);
                }
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.RecordSaved;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto AddRoleClaim(RoleClaimDto roleClaimDto)
        {
            _methodName = "AddRoleClaim";
            var resultDto = new ResultDto();
            try
            {
                if (roleClaimDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (string.IsNullOrEmpty(roleClaimDto.Role.Name))
                {
                    return _resultService.ErrorMessage(Constants.RoleEmpty);
                }
                if (roleClaimDto.Role.RoleTypeId == 0)
                {
                    return _resultService.ErrorMessage(Constants.RoleTypeEmpty);
                }
                if (roleClaimDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var roleNameContext = _emamiContext.Roles.AsNoTracking().Count(_ => _.Name == roleClaimDto.Role.Name);
                if (roleNameContext > 0)
                {
                    return _resultService.ErrorMessage(Constants.RoleNameExist);
                }
                //Save Role
                var newRoleContext = new Role
                {
                    Name = roleClaimDto.Role.Name.Trim(),
                    Description = roleClaimDto.Role.Description,
                    RoleTypeId = roleClaimDto.Role.RoleTypeId,
                    IsActive = true,
                    CreatedBy = roleClaimDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow) //_lookupService.IsIndiaTimeZone() ? DateHelper.UtcToIndia(DateTime.UtcNow) : DateTime.UtcNow
                };
                _emamiContext.Roles.Add(newRoleContext);
                _emamiContext.SaveChanges();

                //save RoleClaim
                foreach (var claimId in roleClaimDto.ClaimIds)
                {
                    var newRoleClaimContext = new RoleClaim
                    {
                        RoleId = newRoleContext.Id,
                        ClaimId = claimId,
                        CreatedBy = roleClaimDto.LoginUserId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow) //_lookupService.IsIndiaTimeZone() ? DateHelper.UtcToIndia(DateTime.UtcNow) : DateTime.UtcNow
                    };
                    _emamiContext.RoleClaims.Add(newRoleClaimContext);
                }
                _emamiContext.SaveChanges();

                //Save Role Hierarchy
                SaveRoleHierarchyBasedOnRole(roleClaimDto.Role.Name.Trim(), newRoleContext.Id, roleClaimDto.LoginUserId);

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.RecordSaved;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public void SaveRoleHierarchyBasedOnRole(string roleName, long roleId, long loginUserId)
        {
            var Verticals = _emamiContext.Divisions.AsNoTracking().Where(_ => _.IsActive).Select(a => a.Id).ToList();

            foreach (var data in Verticals)
            {
                var hierarchyOrgVertical = _emamiContext.RoleHierarchy.Where(_ => _.IsActive
               ).ToList();

                if (hierarchyOrgVertical != null && hierarchyOrgVertical.Any())
                {
                    var roleHierarchy = hierarchyOrgVertical.Where(_ => _.Name == roleName).ToList();
                    if (!roleHierarchy.Any())
                    {
                        var input = new RoleHierarchy
                        {
                            Name = roleName,
                            RoleId = roleId,
                            HierarchyId = hierarchyOrgVertical.Count() + 1,
                            //  VerticalId = data,
                            CreatedBy = loginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        };
                        _emamiContext.RoleHierarchy.Add(input);
                        _emamiContext.SaveChanges();
                    }
                }

                var hierarchySalesVertical = _emamiContext.RoleHierarchy.Where(_ => _.IsActive
            ).ToList();

                if (hierarchySalesVertical != null && hierarchySalesVertical.Any())
                {
                    var roleHierarchy = hierarchySalesVertical.Where(_ => _.Name == roleName).ToList();
                    if (!roleHierarchy.Any())
                    {
                        var input = new RoleHierarchy
                        {
                            Name = roleName,
                            RoleId = roleId,
                            HierarchyId = hierarchySalesVertical.Count() + 1,
                            //ProcessId = (int)DTO.Enums.HierarchyProcess.Sales,
                            //VerticalId = data,
                            CreatedBy = loginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        };
                        _emamiContext.RoleHierarchy.Add(input);
                        _emamiContext.SaveChanges();
                    }
                }
            }


            //var hierarchyOrgSpecFat = _emamiContext.RoleHierarchy.Where(_ => _.IsActive
            //&& _.ProcessId == (int)DTO.Enums.HierarchyProcess.Organization && _.VerticalId == (int)DTO.Enums.Vertical.SpecialityFat).ToList();

            //if (hierarchyOrgSpecFat != null && hierarchyOrgSpecFat.Any())
            //{
            //    var roleHierarchy = hierarchyOrgSpecFat.Where(_ => _.Name == roleName).ToList();
            //    if (!roleHierarchy.Any())
            //    {
            //        var input = new RoleHierarchy
            //        {
            //            Name = roleName,
            //            RoleId = roleId,
            //            HierarchyId = hierarchyOrgSpecFat.Count() + 1,
            //            ProcessId = (int)DTO.Enums.HierarchyProcess.Organization,
            //            VerticalId = (int)DTO.Enums.Vertical.SpecialityFat,
            //            CreatedBy = loginUserId,
            //            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
            //        };
            //        _emamiContext.RoleHierarchy.Add(input);
            //        _emamiContext.SaveChanges();
            //    }
            //}



            //var hierarchySalesSpecFat = _emamiContext.RoleHierarchy.Where(_ => _.IsActive
            //&& _.ProcessId == (int)DTO.Enums.HierarchyProcess.Sales && _.VerticalId == (int)DTO.Enums.Vertical.SpecialityFat).ToList();

            //if (hierarchySalesSpecFat != null && hierarchySalesSpecFat.Any())
            //{
            //    var roleHierarchy = hierarchySalesSpecFat.Where(_ => _.Name == roleName).ToList();
            //    if (!roleHierarchy.Any())
            //    {
            //        var input = new RoleHierarchy
            //        {
            //            Name = roleName,
            //            RoleId = roleId,
            //            HierarchyId = hierarchySalesSpecFat.Count() + 1,
            //            ProcessId = (int)DTO.Enums.HierarchyProcess.Sales,
            //            VerticalId = (int)DTO.Enums.Vertical.SpecialityFat,
            //            CreatedBy = loginUserId,
            //            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
            //        };
            //        _emamiContext.RoleHierarchy.Add(input);
            //        _emamiContext.SaveChanges();
            //    }
            //}
        }

        public ResultDto GetClaims()
        {
            _methodName = "GetClaims";
            var resultDto = new ResultDto();
            var claimListDto = new List<ClaimDto>();
            try
            {
                claimListDto = _emamiContext.Claims.Where(_ => _.IsActive).Select(c => new ClaimDto()
                {
                    ClaimId = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Id = c.Id
                }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = claimListDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetRoleTypes()
        {
            _methodName = "GetRoleTypes";
            var resultDto = new ResultDto();
            var roleTypeListDto = new List<RoleTypeDto>();
            try
            {
                roleTypeListDto = _emamiContext.RoleTypes.Where(_ => _.IsActive).OrderBy(_ => _.HierarchyId).Select(c => new RoleTypeDto()
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsPrime = c.IsPrime,
                    LevelNo = c.HierarchyId,
                    Description = c.Description
                }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = roleTypeListDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetRoles()
        {
            _methodName = "GetRoles";
            var resultDto = new ResultDto();
            var roleListDto = new List<RoleDto>();
            try
            {
                var rolesContext = _emamiContext.Roles.Where(_ => _.IsActive).OrderBy(_ => _.Name).ToList();
                if (rolesContext.Any())
                {
                    foreach (var role in rolesContext)
                    {
                        var roleDto = new RoleDto
                        {
                            Id = role.Id,
                            Name = role.Name,
                            Description = role.Description,
                            RoleTypeId = role.RoleTypeId
                        };
                        //var superHierarchyId = role.RoleType.HierarchyId - 1;
                        //var roleTypeContext = _emamiContext.RoleTypes.AsNoTracking().FirstOrDefault(_ => _.HierarchyId == superHierarchyId);
                        //if (roleTypeContext != null)
                        //{
                        //    roleDto.SuperRoleTypeId = roleTypeContext.Id;
                        //}
                        roleListDto.Add(roleDto);
                    }
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = roleListDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetAllRoleTypeClaims()
        {
            _methodName = "GetAllRoleTypeClaims";
            var resultDto = new ResultDto();
            try
            {
                var systemRoleTypeClaimsDto = new SystemRoleTypeClaimsDto
                {
                    SystemClaims = _emamiContext.Claims.AsNoTracking().Where(_ => _.IsActive).Select(c => new ClaimDto()
                    {
                        ClaimId = c.Id,
                        Name = c.Name
                    }).ToList(),
                };
                systemRoleTypeClaimsDto.SystemRoleTypes = _emamiContext.RoleTypes.Where(_ => _.IsActive).Select(r => new RoleTypeClaimOutputDto()
                {
                    RoleTypeId = r.Id,
                    RoleTypeName = r.Name,
                    IsPrime = r.IsPrime,
                    Claims = r.RoleTypeClaims.Select(x => new ClaimDto
                    {
                        ClaimId = x.ClaimId,
                        Name = x.Claim.Name
                    }).ToList()
                }).ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = systemRoleTypeClaimsDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }

        }

        public ResultDto GetRoleTypeClaims(RoleTypeUsersDto roleTypeIdDto)
        {
            _methodName = "GetRoleTypeClaims";
            var resultDto = new ResultDto();
            try
            {
                var systemRoleTypeClaimsDto = new SystemRoleTypeClaimsDto
                {
                    SystemClaims = _emamiContext.Claims.Where(_ => _.IsActive).Select(c => new ClaimDto()
                    {
                        ClaimId = c.Id,
                        Name = c.Name
                    }).ToList(),
                    SystemRoleTypes = _emamiContext.RoleTypes.Where(_ => _.IsActive && _.Id == roleTypeIdDto.RoleTypeId).Select(r => new RoleTypeClaimOutputDto()
                    {
                        RoleTypeId = r.Id,
                        RoleTypeName = r.Name,
                        IsPrime = r.IsPrime,
                        Claims = r.RoleTypeClaims.Select(x => new ClaimDto
                        {
                            ClaimId = x.ClaimId,
                            Name = x.Claim.Name
                        }).ToList()
                    }).ToList()
                };
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = systemRoleTypeClaimsDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }

        }

        public ResultDto GetAllRoleClaims()
        {
            _methodName = "GetAllRoleClaims";
            var resultDto = new ResultDto();
            try
            {
                var roleClaimViewDto = new RoleClaimViewDto
                {
                    SystemClaims = _emamiContext.Claims.AsNoTracking().Where(_ => _.IsActive).Select(c => new ClaimDto()
                    {
                        ClaimId = c.Id,
                        Name = c.Name
                    }).ToList(),
                };

                var roleClaimListContext = _emamiContext.Roles.AsNoTracking().Where(_ => _.IsActive).ToList();
                if (roleClaimListContext.Any())
                {
                    foreach (var roleClaim in roleClaimListContext)
                    {
                        var roleClaimDto = new RoleClaimRoleTypeClaimViewDto
                        {
                            RoleId = roleClaim.Id,
                            RoleName = roleClaim.Name,
                            RoleTypeId = roleClaim.RoleTypeId,
                            RoleTypeName = roleClaim.RoleType.Name
                        };
                        foreach (var claim in roleClaim.RoleClaims)
                        {
                            var claimDto = new ClaimDto()
                            {
                                ClaimId = claim.ClaimId,
                                Name = claim.Claim.Name
                            };
                            roleClaimDto.RoleClaims.Add(claimDto);
                        }
                        foreach (var roletypeClaim in roleClaim.RoleType.RoleTypeClaims)
                        {
                            var roleTypeClaimclaimDto = new ClaimDto()
                            {
                                ClaimId = roletypeClaim.ClaimId,
                                Name = roletypeClaim.Claim.Name
                            };
                            roleClaimDto.RoleTypeClaims.Add(roleTypeClaimclaimDto);
                        }
                        roleClaimViewDto.RoleClaimsAndRoleTypeClaims.Add(roleClaimDto);
                    }
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = roleClaimViewDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }

        }

        public ResultDto GetRoleName(int roleId)
        {
            _methodName = "GetRoleName";
            var resultDto = new ResultDto();
            try
            {
                var roleName = _emamiContext.Roles.AsNoTracking().FirstOrDefault(_ => _.Id == roleId).Name;
                if (roleName != null)
                {
                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = roleName;
                    return resultDto;
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = string.Empty;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetClaimName(int claimId)
        {
            _methodName = "GetClaimName";
            var resultDto = new ResultDto();
            try
            {
                var claimName = _emamiContext.Claims.AsNoTracking().FirstOrDefault(_ => _.Id == claimId).Name;
                if (claimName != null)
                {
                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = claimName;
                    return resultDto;
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = string.Empty;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto UpdateRoleClaims(RoleClaimUpdateDto roleClaimUpdateDto)
        {
            _methodName = "UpdateRoleClaims";
            var resultDto = new ResultDto();
            try
            {
                if (roleClaimUpdateDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                if (!roleClaimUpdateDto.RoleClaimIds.Any())
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                //Check the Deleting role is assigned to a User
                var exeptionMessage = new StringBuilder();
                foreach (var removeRole in roleClaimUpdateDto.RoleClaimIds.Where(_ => _.Item3 && _.Item1 > 0))
                {
                    var result = CheckRoleNotAssigned(removeRole.Item1);
                    if (!result.IsSuccess)
                    {
                        if (result.ErrorDto.ErrorCode == string.Empty)
                        {
                            exeptionMessage.AppendFormat(Constants.RoleCannotDelete + Environment.NewLine, removeRole.Item2);
                        }
                        exeptionMessage.AppendFormat(result.ErrorDto.Message + Environment.NewLine, removeRole.Item2, roleClaimUpdateDto.LoginUserId);
                    }
                }
                if (exeptionMessage.Length > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = string.Empty;
                    resultDto.ErrorDto.Message = exeptionMessage.ToString();
                    return resultDto;
                }
                foreach (var roleClaim in roleClaimUpdateDto.RoleClaimIds)
                {
                    DeleteRoleAndUpdateRoleClaims(roleClaim.Item1, roleClaim.Item3, roleClaim.Item4, roleClaimUpdateDto.LoginUserId);
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.RecordSaved;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        private ResultDto CheckRoleNotAssigned(long roleId)
        {
            var resultDto = new ResultDto();
            _logger.Debug("Check Role:" + roleId);
            try
            {
                var role = _emamiContext.Roles.AsNoTracking().FirstOrDefault(x => x.Id == roleId && !x.IsDeleted);
                if (role != null)
                {
                    var userRole = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(x => x.RoleId == roleId);
                    if (userRole != null)
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.ErrorCode = string.Empty;
                        resultDto.ErrorDto.Message = role.Name;
                        return resultDto;
                    }
                    resultDto.IsSuccess = true;
                    return resultDto;
                }
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.RoleNotFound;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;

            }
        }

        private void DeleteRoleAndUpdateRoleClaims(long roleId, bool isDeleted, IList<int> claimList, long loginUserId)
        {
            _logger.Debug("Update RoleClaim:" + roleId);
            //Remove existing RoleClaim
            var roleClaims = _emamiContext.RoleClaims.Where(x => x.RoleId == roleId).ToList();
            if (roleClaims.Any())
            {
                foreach (var claim in roleClaims)
                {
                    _emamiContext.RoleClaims.Remove(claim);
                }
            }
            //Insert new RoleClaims
            if (!isDeleted)
            {
                if (claimList.IsAny())
                {
                    foreach (var claimid in claimList)
                    {
                        var newClaim = new RoleClaim
                        {
                            ClaimId = claimid,
                            RoleId = roleId,
                            CreatedBy = loginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow) //_lookupService.IsIndiaTimeZone() ? DateHelper.UtcToIndia(DateTime.UtcNow) : DateTime.UtcNow
                        };
                        _emamiContext.RoleClaims.Add(newClaim);
                    }
                }
            }
            //Delete Role
            else
            {
                var role = _emamiContext.Roles.FirstOrDefault(x => x.Id == roleId);
                if (role != null)
                {
                    role.IsActive = false;
                    role.IsDeleted = true;
                    role.ModifiedBy = loginUserId;
                    role.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow); //_lookupService.IsIndiaTimeZone() ? DateHelper.UtcToIndia(DateTime.UtcNow) : DateTime.UtcNow;
                }
            }
            _emamiContext.SaveChanges();
        }

        public ResultDto UpdateRoleTypeClaims(RoleTypeClaimUpdateDto roleTypeClaimUpdateDto)
        {
            _methodName = "UpdateRoleTypeClaims";
            var resultDto = new ResultDto();
            try
            {
                if (!roleTypeClaimUpdateDto.RoleTypeClaimIds.Any())
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (roleTypeClaimUpdateDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                //Check the Deleting RoleType is assigned to a User
                var exeptionMessage = new StringBuilder();
                foreach (var removeRoleType in roleTypeClaimUpdateDto.RoleTypeClaimIds.Where(_ => _.Item3 && _.Item1 > 0))
                {
                    var result = CheckRoleTypeNotAssigned(removeRoleType.Item1);
                    if (!result.IsSuccess)
                    {
                        if (result.ErrorDto.ErrorCode == string.Empty)
                        {
                            exeptionMessage.AppendFormat(Constants.RoleTypeCannotDelete + Environment.NewLine, removeRoleType.Item2);
                        }
                        exeptionMessage.AppendFormat(result.ErrorDto.Message + Environment.NewLine, removeRoleType.Item2);
                    }
                }
                if (exeptionMessage.Length > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = string.Empty;
                    resultDto.ErrorDto.Message = exeptionMessage.ToString();
                    return resultDto;
                }
                foreach (var roleTypeClaim in roleTypeClaimUpdateDto.RoleTypeClaimIds)
                {
                    DeleteRoleTypeAndUpdateRoleTypeClaims(roleTypeClaim.Item1, roleTypeClaim.Item3, roleTypeClaim.Item4, roleTypeClaimUpdateDto.LoginUserId);
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.RecordSaved;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        private ResultDto CheckRoleTypeNotAssigned(long roleTypeId)
        {
            var resultDto = new ResultDto();
            _logger.Debug("Check RoleType:" + roleTypeId);
            try
            {
                var roleType = _emamiContext.RoleTypes.AsNoTracking().FirstOrDefault(x => x.Id == roleTypeId && !x.IsDeleted);
                if (roleType != null)
                {
                    var userRoleType = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(x => x.Role.RoleTypeId == roleTypeId);
                    if (userRoleType != null)
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.ErrorCode = string.Empty;
                        resultDto.ErrorDto.Message = roleType.Name;
                        return resultDto;
                    }
                    resultDto.IsSuccess = true;
                    return resultDto;
                }
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.RoleTypeNotFound;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;

            }
        }

        private ResultDto CheckRoleTypeNotAssignedToRole(long roleTypeId)
        {
            var resultDto = new ResultDto();
            _logger.Debug("Check RoleType assigned to Role:" + roleTypeId);
            try
            {
                var roleContext = _emamiContext.Roles.AsNoTracking().FirstOrDefault(x => x.RoleTypeId == roleTypeId);
                if (roleContext != null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = string.Empty;
                    resultDto.ErrorDto.Message = roleContext.Name;
                    return resultDto;
                }
                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;

            }
        }
        private ResultDto CheckRoleNotAssignedToUser(long roleId)
        {
            var resultDto = new ResultDto();
            _logger.Debug("Check Role assigned to User:" + roleId);
            try
            {
                var userContext = from user in _emamiContext.Users
                                  join userRole in _emamiContext.UserRoles on user.Id equals userRole.UserId
                                  where user.IsActive && userRole.RoleId == roleId
                                  select userRole;

                if (userContext != null && userContext.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = string.Empty;
                    resultDto.ErrorDto.Message = userContext.FirstOrDefault().Role.Name;
                    return resultDto;
                }
                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;

            }
        }

        private ResultDto CheckRoleTypeClaimsNotAssignedToRoleClaims(long roleTypeId)
        {
            var resultDto = new ResultDto();
            _logger.Debug("Check RoleType Claim:" + roleTypeId);
            try
            {
                var roleTypeClaims = _emamiContext.RoleTypeClaims.AsNoTracking().Where(x => x.RoleTypeId == roleTypeId).ToList();
                var claimIds = new List<int>();
                foreach (var roletypeClaim in roleTypeClaims)
                {
                    claimIds.Add(roletypeClaim.ClaimId);
                }
                if (roleTypeClaims.Any())
                {
                    var roleClaims = _emamiContext.RoleClaims.AsNoTracking().Where(x => x.Role.RoleTypeId == roleTypeId && claimIds.Contains(x.ClaimId)).ToList();
                    if (roleClaims.Any())
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.ErrorCode = string.Empty;
                        resultDto.ErrorDto.Message = roleClaims.FirstOrDefault().Role.Name;
                        return resultDto;
                    }
                }
                resultDto.IsSuccess = true;
                resultDto.ErrorDto.ErrorCode = string.Empty;
                resultDto.ErrorDto.Message = string.Empty;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        private void DeleteRoleTypeAndUpdateRoleTypeClaims(long roleTypeId, bool isDeleted, IList<int> claimList, long loginUserId)
        {
            _logger.Debug("Update RoleTypeClaim:" + roleTypeId);
            //Remove existing RoleTypeClaim
            var roleTypeClaims = _emamiContext.RoleTypeClaims.Where(x => x.RoleTypeId == roleTypeId).ToList();
            if (roleTypeClaims.Any())
            {
                foreach (var claim in roleTypeClaims)
                {
                    _emamiContext.RoleTypeClaims.Remove(claim);
                }
            }
            //Insert new RoleTypeClaims
            if (!isDeleted)
            {
                if (claimList.IsAny())
                {
                    foreach (var claimid in claimList)
                    {
                        var newClaim = new RoleTypeClaim
                        {
                            ClaimId = claimid,
                            RoleTypeId = roleTypeId,
                            CreatedBy = loginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow) //_lookupService.IsIndiaTimeZone() ? DateHelper.UtcToIndia(DateTime.UtcNow) : DateTime.UtcNow
                        };
                        _emamiContext.RoleTypeClaims.Add(newClaim);
                    }
                }
            }
            //Delete RoleType
            else
            {
                var roleType = _emamiContext.RoleTypes.FirstOrDefault(x => x.Id == roleTypeId);
                if (roleType != null)
                {
                    roleType.IsActive = false;
                    roleType.IsDeleted = true;
                    roleType.ModifiedBy = loginUserId;
                    roleType.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);  //_lookupService.IsIndiaTimeZone() ? DateHelper.UtcToIndia(DateTime.UtcNow) : DateTime.UtcNow;
                }
            }
            _emamiContext.SaveChanges();
        }

        //private void DeleteRoleType(long roleTypeId, long loginUserId)
        //{
        //    _logger.Debug("Delete RoleType:" + roleTypeId);
        //    //Remove existing RoleTypeClaim
        //    var roleTypeClaims = _emamiContext.RoleTypeClaims.Where(x => x.RoleTypeId == roleTypeId).ToList();
        //    if (roleTypeClaims.Any())
        //    {
        //        foreach (var claim in roleTypeClaims)
        //        {
        //            _emamiContext.RoleTypeClaims.Remove(claim);
        //        }
        //    }
        //    _emamiContext.SaveChanges();
        //    //Delete RoleType
        //    var roleType = _emamiContext.RoleTypes.FirstOrDefault(x => x.Id == roleTypeId);
        //    if (roleType != null)
        //    {
        //        roleType.IsActive = false;
        //        roleType.IsDeleted = true;
        //        roleType.ModifiedBy = loginUserId;
        //        roleType.ModifiedDate = _lookupService.IsIndiaTimeZone() ? DateHelper.UtcToIndia(DateTime.UtcNow) : DateTime.UtcNow;
        //    }
        //    _emamiContext.SaveChanges();
        //}

        //private void DeleteRole(long roleId, long loginUserId)
        //{
        //    _logger.Debug("Delete Role:" + roleId);
        //    //Remove existing RoleTypeClaim
        //    var roleClaims = _emamiContext.RoleClaims.Where(x => x.RoleId == roleId).ToList();
        //    if (roleClaims.Any())
        //    {
        //        foreach (var claim in roleClaims)
        //        {
        //            _emamiContext.RoleClaims.Remove(claim);
        //        }
        //    }
        //    _emamiContext.SaveChanges();
        //    //Delete Role
        //    var role = _emamiContext.Roles.FirstOrDefault(x => x.Id == roleId);
        //    if (role != null)
        //    {
        //        role.IsActive = false;
        //        role.IsDeleted = true;
        //        role.ModifiedBy = loginUserId;
        //        role.ModifiedDate = _lookupService.IsIndiaTimeZone() ? DateHelper.UtcToIndia(DateTime.UtcNow) : DateTime.UtcNow;
        //    }
        //    _emamiContext.SaveChanges();
        //}

        public ResultDto UpdateRoleTypeHierarchy(RoleTypeHierarchyDto roleTypeHierarchyDto)
        {
            _methodName = "UpdateRoleTypeHierarchy";
            var resultDto = new ResultDto();
            try
            {
                if (roleTypeHierarchyDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (!roleTypeHierarchyDto.RoleTpyeHierarchyNo.Any())
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                foreach (var item in roleTypeHierarchyDto.RoleTpyeHierarchyNo)
                {
                    var roleTypeContext = _emamiContext.RoleTypes.FirstOrDefault(_ => _.Id == item.Key);
                    if (roleTypeContext != null)
                    {
                        roleTypeContext.HierarchyId = item.Value;
                    }
                }
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.RecordSaved;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetOrganizationHierarchy()
        {
            _methodName = "GetOrganizationHierarchy";
            var resultDto = new ResultDto();
            var organizationHierarchyList = new OrganizationHierarchyDto();
            try
            {
                var rolecontextList = _emamiContext.Roles.AsNoTracking().Where(_ => _.IsActive && _.RoleType.IsActive).OrderBy(_ => _.RoleType.HierarchyId).ToList();
                if (!rolecontextList.Any())
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }
                var levelid = rolecontextList.Select(_ => _.RoleType.HierarchyId).FirstOrDefault();
                long? reportingId = null;
                int loopCount = 1;
                long? firstId = null;
                foreach (var role in rolecontextList)
                {
                    if (role.RoleType.HierarchyId == levelid)
                    {
                        ////Nothing
                    }
                    else if (role.RoleType.HierarchyId == levelid + 1)
                    {
                        levelid++;
                    }
                    for (int i = role.RoleType.HierarchyId - 1; i >= 1; i--)
                    {
                        var roletypecontext = _emamiContext.RoleTypes.AsNoTracking().FirstOrDefault(_ => _.HierarchyId == i && _.IsActive);
                        if (roletypecontext != null)
                        {
                            reportingId = roletypecontext.Id;
                            break;
                        }
                    }
                    if (loopCount == 1)
                    {
                        var usersListQuery = from user in _emamiContext.Users
                                             join userRole in _emamiContext.UserRoles on user.Id equals userRole.UserId
                                             where user.IsActive && userRole.RoleId == role.Id
                                             select new { user.Name, user.ImageUrl };
                        var usersList = usersListQuery.ToList();
                        //var usersList = _emamiContext.Users.AsNoTracking().Where(_ => _.IsActive && _.RoleId == role.Id).Select(_ => new { _.Name, _.ImageUrl }).ToList();
                        if (usersList.Any())
                        {
                            foreach (var user in usersList)
                            {
                                Tuple<string, long, long?, string, string> org = Tuple.Create(role.Name, role.Id, firstId, user.Name, user.ImageUrl);
                                organizationHierarchyList.OrganizationHierarchy.Add(org);
                            }
                        }
                    }
                    else
                    {
                        var usersListQuery = from user in _emamiContext.Users
                                             join userRole in _emamiContext.UserRoles on user.Id equals userRole.UserId
                                             where user.IsActive && userRole.RoleId == role.Id
                                             select new { user.Name, user.ImageUrl };
                        var usersList = usersListQuery.ToList();

                        //var usersList = _emamiContext.Users.AsNoTracking().Where(_ => _.IsActive && _.RoleId == role.Id).Select(_ => new { _.Name, _.ImageUrl }).ToList();
                        if (usersList.Any())
                        {
                            foreach (var user in usersList)
                            {
                                Tuple<string, long, long?, string, string> org1 = Tuple.Create(role.Name, role.Id, reportingId, user.Name, user.ImageUrl);
                                organizationHierarchyList.OrganizationHierarchy.Add(org1);
                            }
                        }
                    }
                    loopCount++;
                }
                organizationHierarchyList.MaxLevelid = levelid;
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = organizationHierarchyList;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto DeleteRoleTypeAndClaims(RoleTypeIdDto roleTypeIdDto)
        {
            _methodName = "DeleteRoleType";
            var resultDto = new ResultDto();
            try
            {
                if (roleTypeIdDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (roleTypeIdDto.RoleTypeId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RoleTypeEmpty;
                    return resultDto;
                }
                if (roleTypeIdDto.LoginUserId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.UserIdMissing;
                    return resultDto;
                }
                var roleType = _emamiContext.RoleTypes.AsNoTracking().FirstOrDefault(x => x.Id == roleTypeIdDto.RoleTypeId && !x.IsDeleted);
                if (roleType == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RoleTypeNotFound;
                    return resultDto;
                }
                if (roleType.IsPrime)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.PrimeRoleTypeCannotDelete;
                    return resultDto;
                }
                var exeptionMessage = new StringBuilder();
                //Check the Deleting RoleType is assigned to a Role 
                var roleResult = CheckRoleTypeNotAssignedToRole(roleTypeIdDto.RoleTypeId);
                if (!roleResult.IsSuccess)
                {
                    if (roleResult.ErrorDto.ErrorCode == string.Empty)
                    {
                        exeptionMessage.AppendFormat(Constants.RoleTypeMappedToRole, roleResult.ErrorDto.Message);
                        exeptionMessage.Append(Environment.NewLine);
                    }
                }
                //Check the Deleting RoleType is assigned to a Role Claims
                var result = CheckRoleTypeClaimsNotAssignedToRoleClaims(roleTypeIdDto.RoleTypeId);
                if (!result.IsSuccess)
                {
                    if (result.ErrorDto.ErrorCode == string.Empty)
                    {
                        exeptionMessage.AppendFormat(Constants.RoleTypeAndClaimCannotDelete, result.ErrorDto.Message);
                    }
                }
                if (exeptionMessage.Length > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = exeptionMessage.ToString();
                    return resultDto;
                }
                var roleTypeClaims = _emamiContext.RoleTypeClaims.Where(x => x.RoleTypeId == roleTypeIdDto.RoleTypeId).ToList();
                if (roleTypeClaims.Any())
                {
                    foreach (var roleTypeclaim in roleTypeClaims)
                    {
                        _emamiContext.RoleTypeClaims.Remove(roleTypeclaim);
                    }
                }
                var roleTypeContext = _emamiContext.RoleTypes.FirstOrDefault(x => x.Id == roleTypeIdDto.RoleTypeId);
                if (roleTypeContext != null)
                {
                    roleTypeContext.IsActive = false;
                    roleTypeContext.IsDeleted = true;
                    roleTypeContext.ModifiedBy = roleTypeIdDto.LoginUserId;
                    roleTypeContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow); //_lookupService.IsIndiaTimeZone() ? DateHelper.UtcToIndia(DateTime.UtcNow) : DateTime.UtcNow;
                }
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.RecordSaved;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto DeleteRoleAndClaims(RoleIdDto roleIdDto)
        {
            _methodName = "DeleteRoleType";
            var resultDto = new ResultDto();
            try
            {
                if (roleIdDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (roleIdDto.RoleId == 0)
                {
                    return _resultService.ErrorMessage(Constants.RoleEmpty);
                }
                if (roleIdDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var role = _emamiContext.Roles.AsNoTracking().FirstOrDefault(x => x.Id == roleIdDto.RoleId && !x.IsDeleted);
                if (role == null)
                {
                    return _resultService.ErrorMessage(Constants.RoleNotFound);
                }
                if (role.IsPrime)
                {
                    return _resultService.ErrorMessage(Constants.PrimeRoleCannotDelete);
                }
                var exeptionMessage = new StringBuilder();
                //Check the Deleting Role is assigned to a User 
                var roleResult = CheckRoleNotAssignedToUser(roleIdDto.RoleId);
                if (!roleResult.IsSuccess)
                {
                    if (roleResult.ErrorDto.ErrorCode == string.Empty)
                    {
                        exeptionMessage.AppendFormat(Constants.RoleCannotDelete, roleResult.ErrorDto.Message);
                    }
                }
                if (exeptionMessage.Length > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = string.Empty;
                    resultDto.ErrorDto.Message = exeptionMessage.ToString();
                    return resultDto;
                }
                var roleClaims = _emamiContext.RoleClaims.Where(x => x.RoleId == roleIdDto.RoleId).ToList();
                if (roleClaims.Any())
                {
                    foreach (var roleclaim in roleClaims)
                    {
                        _emamiContext.RoleClaims.Remove(roleclaim);
                    }
                }
                var roleContext = _emamiContext.Roles.FirstOrDefault(x => x.Id == roleIdDto.RoleId);
                if (roleContext != null)
                {
                    roleContext.IsActive = false;
                    roleContext.IsDeleted = true;
                    roleContext.ModifiedBy = roleIdDto.LoginUserId;
                    roleContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow); //_lookupService.IsIndiaTimeZone() ? DateHelper.UtcToIndia(DateTime.UtcNow) : DateTime.UtcNow;
                }
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.RecordSaved;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetReportingToRoles(IdInputDto inputDto)
        {
            _methodName = "GetReportingToRoles";
            var resultDto = new ResultDto();
            var reportingRoles = new List<DropDownDto>();
            try
            {

                var roleType = _emamiContext.RoleTypes
                    .FirstOrDefault(w => w.Id == inputDto.Id);

                if (roleType != null)
                {
                    var primaryRoleIds = _emamiContext.Roles
                    .Join(_emamiContext.RoleTypes, r => r.RoleTypeId, rt => rt.Id, (r, rt) => new { r, rt })
                    .Where(w => w.rt.HierarchyId < roleType.HierarchyId)
                    .Select(s => s.r.Id).ToList();

                    if (primaryRoleIds != null && primaryRoleIds.Any())
                    {
                        reportingRoles = _emamiContext.Users.Join(_emamiContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                            .Where(w => primaryRoleIds.Any(a => a == w.ur.RoleId))
                            .Select(s => new DropDownDto()
                            {
                                Id = s.u.Id,
                                Name = s.u.Name
                            }).ToList();

                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = reportingRoles;
                    }
                    else
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    }
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        #endregion

        #region Process/Role Hierarchy

        public ResultDto GetRoleHierarchyByProcess(RoleHierarchyParamDto inputDto)
        {
            _methodName = "GetProcessHierarchy";
            var resultDto = new ResultDto();
            var resultList = new List<RoleHierarchyDto>();
            try
            {
                var resultContext = new List<RoleHierarchy>();

                resultContext = _emamiContext.RoleHierarchy.Where(_ => _.IsActive /* && _.ProcessId == (int)DTO.Enums.HierarchyProcess.Organization
                    && _.VerticalId == (int)DTO.Enums.Vertical.Hbc */).OrderBy(_ => _.HierarchyId).ToList();

                if (resultContext != null && resultContext.Any())
                {
                    resultList = resultContext.Select(c => new RoleHierarchyDto()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        IsPrime = c.IsPrime,
                        LevelNo = c.HierarchyId,
                        Description = c.Description,
                    }).ToList();
                }
                //else
                //{
                //    if (inputDto.ProcessId == (int)DTO.Enums.HierarchyProcess.ComplaintManagementSystem && inputDto.VerticalId == (int)DTO.Enums.Vertical.SpecialityFat)
                //    {
                //        resultList = _emamiContext.Roles.Where(_ => _.IsActive && (_.Id == (int)DTO.Enums.Role.Admin || _.Id == (int)DTO.Enums.Role_CMS.AssociateBranchManager || _.Id == (int)DTO.Enums.Role_CMS.SalesExecutive || _.Id == (int)DTO.Enums.Role_CMS.Demonstrator || _.Id == (int)DTO.Enums.Role_CMS.DemoInCharge))
                //                            .Select(c => new RoleHierarchyDto()
                //                            {
                //                                Id = c.Id,
                //                                Name = c.Name,
                //                                IsPrime = c.IsPrime,
                //                                LevelNo = 0,
                //                                Description = c.Description,
                //                                ProcessId = 1,
                //                                VerticalId = 1
                //                            }).ToList();
                //    }
                //    else
                //    {
                //        resultList = _emamiContext.Roles.Where(_ => _.IsActive).Select(c => new RoleHierarchyDto()
                //        {
                //            Id = c.Id,
                //            Name = c.Name,
                //            IsPrime = c.IsPrime,
                //            LevelNo = 0,
                //            Description = c.Description,
                //            ProcessId = 1,
                //            VerticalId = 1
                //        }).ToList();
                //    }                    
                //}

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = resultList;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto AddOrUpdateRoleHierarchy(RoleHierarchyDto inputDto)
        {
            _methodName = "UpdateRoleTypeHierarchy";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }
                if (!inputDto.RoleHierarchyNo.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }

                var resultContext = _emamiContext.RoleHierarchy.Where(_ => _.IsActive /* && _.ProcessId == inputDto.ProcessId && _.VerticalId == inputDto.VerticalId */).ToList();
                if (resultContext != null && resultContext.Any())
                {
                    foreach (var item in inputDto.RoleHierarchyNo)
                    {
                        var context = resultContext.FirstOrDefault(_ => _.Id == item.Key);
                        if (context != null)
                        {
                            context.HierarchyId = item.Value;
                        }
                    }
                    _emamiContext.SaveChanges();
                }
                else
                {
                    var rolesList = _emamiContext.Roles.Where(_ => _.IsActive).Select(c => new RoleDto()
                    {
                        Id = c.Id,
                        Name = c.Name,
                    }).ToList();

                    foreach (var item in inputDto.RoleHierarchyNo)
                    {
                        var roleContext = rolesList.FirstOrDefault(_ => _.Id == item.Key);
                        var input = new RoleHierarchy
                        {
                            Name = roleContext.Name,
                            RoleId = roleContext.Id,
                            HierarchyId = item.Value,
                            //ProcessId = inputDto.ProcessId,
                            //VerticalId = inputDto.VerticalId,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        };
                        _emamiContext.RoleHierarchy.Add(input);
                    }
                    _emamiContext.SaveChanges();
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.GetMessage(Constants.RecordSaved, Utility.MessageLanguage);
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        #endregion

        #region Reporting to Users

        public ResultDto GetReportingToUsersByRole(ReportingUsersInputDto inputDto)
        {
            _methodName = "GetReportingToUsersByRole";
            var resultDto = new ResultDto();
            var reportingToUsers = new List<DropDownDto>();
            try
            {
                var divisionMappingUserIds = _emamiContext.UserDivisionMappings
                    .Where(w => inputDto.DivisionIds.Contains((long)w.DivisionId)
                    && inputDto.SalesOrganizationIds.Contains((long)w.SalesOrganizationId)
                    && inputDto.DistributionChannelIds.Contains((long)w.DistributionChannelId)
                    ).Select(_ => _.UserId).Distinct().ToList();

                var roleType = _emamiContext.RoleHierarchy
                    .FirstOrDefault(w => w.RoleId == inputDto.RoleId);

                if (roleType != null)
                {
                    var roleIds = _emamiContext.Roles
                    .Join(_emamiContext.RoleHierarchy
                    .Where(_ => _.HierarchyId == roleType.HierarchyId-1), r => r.Id, rh => rh.RoleId, (r, rh) => new { r, rh })
                    .Select(s => s.r.Id).ToList();

                    if (roleIds != null && roleIds.Any())
                    {
                        reportingToUsers = _emamiContext.Users.Join(_emamiContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                            .Where(w => (roleIds.Contains( w.ur.RoleId) 
                            && divisionMappingUserIds.Contains(w.ur.UserId))
                            || w.ur.RoleId == (int)DTO.Enums.Role.Admin
                            )
                            .Select(s => new DropDownDto()
                            {
                                Id = s.u.Id,
                                Name = s.u.Name
                            }).ToList();

                       

                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = reportingToUsers;
                    }
                    else
                    {


                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    }
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetOrganizationReportingToUsersByUserId(ReportingUsersInputDto inputDto)
        {
            _methodName = "GetReportingToUsersByUserId";
            var resultDto = new ResultDto();
            var reportingToUsers = new List<DropDownDto>();
            try
            {
                reportingToUsers = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId && _.IsActive)
                 .Select(s => new DropDownDto()
                 {
                     Id = s.Id,
                     Name = s.Name
                 }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = reportingToUsers;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetSalesReportingToUsersByUserId(ReportingUsersInputDto inputDto)
        {
            _methodName = "GetSalesReportingToUsersByUserId";
            var resultDto = new ResultDto();
            var reportingToUsers = new List<DropDownDto>();
            try
            {
                if (inputDto.RoleId == (int)DTO.Enums.Role.StateTrader)
                {
                    reportingToUsers = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                                        join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                                        where ucm.UserId == inputDto.LoginUserId
                                        select new DropDownDto
                                        {
                                            Id = u.Id,
                                            Name = u.Name,
                                        }).ToList();
                }
                else
                {
                    reportingToUsers = _emamiContext.UserReportingToMappings.AsNoTracking()
                        .Join(_emamiContext.Users.AsNoTracking(), ur => ur.UserId, u => u.Id, (ur, u) => new { ur, u })
                        .Where(_ => _.ur.ReportingToUserId == inputDto.LoginUserId && _.u.IsActive)
                        .Select(s => new DropDownDto()
                        {
                            Id = s.u.Id,
                            Name = s.u.Name
                        }).ToList();                        ;

                 //   reportingToUsers = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId && _.IsActive)
                 //.Select(s => new DropDownDto()
                 //{
                 //    Id = s.Id,
                 //    Name = s.Name
                 //}).ToList();
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = reportingToUsers;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetSalesReportingToUsersByCityId(ReportingUsersInputDto inputDto)
        {
            _methodName = "GetSalesReportingToUsersByUserId";
            var resultDto = new ResultDto();
            var reportingToUsers = new List<DropDownDto>();
            try
            {
                if (inputDto.RoleId == (int)DTO.Enums.Role.StateTrader)
                {
                    reportingToUsers = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                                        join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                                        where ucm.UserId == inputDto.LoginUserId
                                        && u.CityId == inputDto.CityId
                                        select new DropDownDto
                                        {
                                            Id = u.Id,
                                            Name = u.Name,
                                        }).OrderBy(o => o.Name).Distinct().ToList();
                }
                else
                {
                    IQueryable<User> reportingTo = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId && _.IsActive && _.CityId == inputDto.CityId);

                    var DealersbyBDO = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                                        join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                                        where reportingTo.Any(a => a.Id == ucm.UserId) //ucm.UserId == inputDto.LoginUserId
                                        && u.CityId == inputDto.CityId
                                        select new DropDownDto
                                        {
                                            Id = u.Id,
                                            Name = u.Name,
                                        }).OrderBy(o => o.Name).Distinct().ToList();

                    reportingToUsers.AddRange(DealersbyBDO);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = reportingToUsers;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
            }
            return resultDto;
        }


        public ResultDto GetSalesReportingToUsersByCityStateDistrict(ReportingUsersInputDto inputDto)
        {
            _methodName = "GetSalesReportingToUsersByCityStateDistrict";
            var resultDto = new ResultDto();
            var reportingToUsers = new List<DropDownDto>();
            try
            {
                if (inputDto.RoleId == (int)DTO.Enums.Role.StateTrader)
                {
                    reportingToUsers = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                                        join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                                        where ucm.UserId == inputDto.LoginUserId
                                        && inputDto.CityId > 0 ? u.CityId == inputDto.CityId : inputDto.CityId == 0
                                        && inputDto.DistrictId > 0? u.DistrictId == inputDto.DistrictId : inputDto.DistrictId == 0
                                        && inputDto.StateId > 0 ? u.StateId==inputDto.StateId : inputDto.StateId==0
                                        select new DropDownDto
                                        {
                                            Id = u.Id,
                                            Name = u.Name,
                                        }).OrderBy(o => o.Name).Distinct().ToList();
                }
                else
                {
                    IQueryable<User> reportingTo = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId && _.IsActive
                    && inputDto.CityId > 0 ? _.CityId == inputDto.CityId : inputDto.CityId == 0
                                        && inputDto.DistrictId > 0 ? _.DistrictId == inputDto.DistrictId : inputDto.DistrictId == 0
                                        && inputDto.StateId > 0 ? _.StateId == inputDto.StateId : inputDto.StateId == 0

                    );

                    var DealersbyBDO = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                                        join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                                        where reportingTo.Any(a => a.Id == ucm.UserId) //ucm.UserId == inputDto.LoginUserId
                                        && inputDto.CityId > 0 ? u.CityId == inputDto.CityId : inputDto.CityId == 0
                                        && inputDto.DistrictId > 0 ? u.DistrictId == inputDto.DistrictId : inputDto.DistrictId == 0
                                        && inputDto.StateId > 0 ? u.StateId == inputDto.StateId : inputDto.StateId == 0
                                        select new DropDownDto
                                        {
                                            Id = u.Id,
                                            Name = u.Name,
                                        }).OrderBy(o => o.Name).Distinct().ToList();

                    reportingToUsers.AddRange(DealersbyBDO);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = reportingToUsers;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetReportingToZonalHeadUsersByUserId(ReportingUsersInputDto inputDto)
        {
            _methodName = "GetReportingToZonalHeadUsersByUserId";
            var resultDto = new ResultDto();
            var reportingToUsers = new List<DropDownDto>();
            try
            {
                if (inputDto.LoginUserId == (int)DTO.Enums.Role.Admin)
                {
                    reportingToUsers = _emamiContext.Users.AsNoTracking()
                        .Join(_emamiContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur })
                        .Where(w => (w.UserRoles.RoleId == (int)DTO.Enums.Role.ZonalTrader)
                        //&& w.Users.DivisionId == inputDto.VerticalId
                        ).Select(_ => _.Users)
                        .Select(s => new DropDownDto()
                        {
                            Id = s.Id,
                            Name = s.Name
                        }).ToList();
                }
                else
                {
                    reportingToUsers = _emamiContext.Users.Where(_ => _.ReportingToId == inputDto.LoginUserId && _.IsActive)
                        .Join(_emamiContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur })
                        .Where(w => (w.UserRoles.RoleId == (int)DTO.Enums.Role.ZonalTrader)
                        //&& w.Users.DivisionId == inputDto.VerticalId
                        ).Select(_ => _.Users)
                        .Select(s => new DropDownDto()
                        {
                            Id = s.Id,
                            Name = s.Name
                        }).ToList();
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = reportingToUsers;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetReportingToBDOUsersByUserId(ReportingUsersInputDto inputDto)
        {
            _methodName = "GetReportingToBDOUsersByUserId";
            var resultDto = new ResultDto();
            var reportingToUsers = new List<DropDownDto>();
            try
            {
                var userList = _emamiContext.Users.Where(_ => _.ReportingToId == inputDto.UserId && _.IsActive)
                   .Join(_emamiContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur })
                   .Where(w => (w.UserRoles.RoleId == (int)DTO.Enums.Role.StateTrader)
                   //&& w.Users.DivisionId == inputDto.VerticalId
                   );

                reportingToUsers = userList.ToList()
                 .Select(s => new DropDownDto()
                 {
                     Id = s.Users.Id,
                     Name = s.Users.Name
                 }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = reportingToUsers;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetReportingToRABDOUsersByUserId(ReportingUsersInputDto inputDto)
        {
            _methodName = "GetReportingToRABDOUsersByUserId";
            var resultDto = new ResultDto();
            var reportingToUsers = new List<DropDownDto>();
            try
            {
                var userList = _emamiContext.Users.Where(_ => _.ReportingToId == inputDto.UserId && _.IsActive
                //&& _.SaudaBookingTypeId==(int)DTO.Enums.SaudaBookingTypes.ReverseAuction
                )
                   .Join(_emamiContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur })
                   .Where(w => (w.UserRoles.RoleId == (int)DTO.Enums.Role.StateTrader)
                   //&& w.Users.DivisionId == inputDto.VerticalId
                   );

                reportingToUsers = userList.ToList()
                 .Select(s => new DropDownDto()
                 {
                     Id = s.Users.Id,
                     Name = s.Users.Name
                 }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = reportingToUsers;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetClaimsByRoleId(RoleIdDto inputDto)
        {
            _methodName = "GetClaimsByRoleId";
            var resultDto = new ResultDto();
            var claimslist = new List<ClaimDto>();
            try
            {
                claimslist = _emamiContext.RoleClaims.Where(_ => _.RoleId == inputDto.RoleId).Select(r => new ClaimDto()
                {
                    ClaimId = r.ClaimId,
                    Name = r.Claim.Name
                }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = claimslist;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
            }
            return resultDto;
        }
        public ResultDto GetClaimsByRoleTypeId(RoleIdDto inputDto)
        {
            _methodName = "GetClaimsByRoleTypeId";
            var resultDto = new ResultDto();
            var claimslist = new List<ClaimDto>();
            try
            {
                claimslist = _emamiContext.RoleTypeClaims.Where(_ => _.RoleTypeId == inputDto.RoleId).Select(r => new ClaimDto()
                {
                    ClaimId = r.ClaimId,
                    Name = r.Claim.Name
                }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = claimslist;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
            }
            return resultDto;
        }


        #endregion
    }



    //public class AdminService: IAdminService
    //{
    //    private readonly IEmamiContext _emamiContext;
    //    private readonly ILogger _logger = Logging.GetLogger("Admin Service");        
    //    private const string ServiceName = "Lookup Service";
    //    private string _methodName;

    //    public AdminService(IEmamiContext salesContext)
    //    {
    //        try
    //        {
    //            _emamiContext = salesContext;                
    //        }
    //        catch (Exception exception)
    //        {
    //            _logger.Error("Error instantiating dependencies for Lookup Service", exception);
    //        }
    //    }

    //    public ResultDto AddRole(RoleDto roleDto)
    //    {
    //        _methodName = "AddRole";
    //        var resultDto = new ResultDto();
    //        try
    //        {
    //            if (roleDto == null)
    //            {
    //                resultDto.IsSuccess = false;
    //                resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
    //                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
    //                return resultDto;
    //            }
    //            if (string.IsNullOrEmpty(roleDto.Name))
    //            {
    //                resultDto.IsSuccess = false;
    //                resultDto.ErrorDto.ErrorCode = Constants.RoleEmpty;
    //                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RoleEmpty, Utility.MessageLanguage);
    //                return resultDto;
    //            }
    //            if (roleDto.RoleTypeId == 0)
    //            {
    //                resultDto.IsSuccess = false;
    //                resultDto.ErrorDto.ErrorCode = Constants.RoleTypeEmpty;
    //                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RoleTypeEmpty, Utility.MessageLanguage);
    //                return resultDto;
    //            }
    //            if (roleDto.LoginUserId == 0)
    //            {
    //                resultDto.IsSuccess = false;
    //                resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
    //                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Utility.MessageLanguage);
    //                return resultDto;
    //            }
    //            var roleNameContext = _emamiContext.Roles.AsNoTracking().Count(_ => _.Name == roleDto.Name && !_.IsDeleted);
    //            if (roleNameContext > 0)
    //            {
    //                resultDto.IsSuccess = false;
    //                resultDto.ErrorDto.ErrorCode = Constants.RoleNameExist;
    //                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RoleNameExist, Utility.MessageLanguage);
    //                return resultDto;
    //            }
    //            var roleContext = new Role
    //            {
    //                Name = roleDto.Name.Trim(),
    //                Description = roleDto.Description,
    //                IsActive = true,
    //                CreatedBy = roleDto.LoginUserId,
    //                CreatedDate = DateTime.UtcNow
    //            };
    //            _emamiContext.Roles.Add(roleContext);
    //            _emamiContext.SaveChanges();

    //            resultDto.IsSuccess = true;
    //            resultDto.SuccessDto.Response = Constants.GetMessage(Constants.RecordSaved, Utility.MessageLanguage);
    //            return resultDto;
    //        }
    //        catch (Exception exception)
    //        {
    //            var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
    //            resultDto.IsSuccess = false;
    //            resultDto.ErrorDto.ErrorCode = Constants.Exception;
    //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
    //            _logger.Error(message);
    //            return resultDto;
    //        }
    //    }

    //    public ResultDto DeleteRole(RoleIdDto roleIdDto)
    //    {
    //        _methodName = "DeleteRole";
    //        var resultDto = new ResultDto();
    //        try
    //        {
    //            if (roleIdDto == null)
    //            {
    //                resultDto.IsSuccess = false;
    //                resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
    //                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
    //                return resultDto;
    //            }
    //            if (roleIdDto.RoleId == 0)
    //            {
    //                resultDto.IsSuccess = false;
    //                resultDto.ErrorDto.ErrorCode = Constants.RoleEmpty;
    //                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RoleEmpty, Utility.MessageLanguage);
    //                return resultDto;
    //            }
    //            if (roleIdDto.LoginUserId == 0)
    //            {
    //                resultDto.IsSuccess = false;
    //                resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
    //                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Utility.MessageLanguage);
    //                return resultDto;
    //            }
    //            var role = _emamiContext.Roles.AsNoTracking().FirstOrDefault(x => x.Id == roleIdDto.RoleId && !x.IsDeleted);
    //            if (role == null)
    //            {
    //                resultDto.IsSuccess = false;
    //                resultDto.ErrorDto.ErrorCode = Constants.RoleNotFound;
    //                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RoleNotFound, Utility.MessageLanguage);
    //                return resultDto;
    //            }
    //            if (role.IsPrime)
    //            {
    //                resultDto.IsSuccess = false;
    //                resultDto.ErrorDto.ErrorCode = Constants.PrimeRoleCannotDelete;
    //                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.PrimeRoleCannotDelete, Utility.MessageLanguage);
    //                return resultDto;
    //            }
    //            var exeptionMessage = new StringBuilder();
    //            //Check the Deleting Role is assigned to a User 
    //            var roleResult = CheckRoleNotAssignedToUser(roleIdDto.RoleId);
    //            if (!roleResult.IsSuccess)
    //            {
    //                if (roleResult.ErrorDto.ErrorCode == string.Empty)
    //                {
    //                    exeptionMessage.AppendFormat(Constants.GetMessage(Constants.RoleCannotDelete, Utility.MessageLanguage), roleResult.ErrorDto.Message);
    //                }
    //            }
    //            if (exeptionMessage.Length > 0)
    //            {
    //                resultDto.IsSuccess = false;
    //                resultDto.ErrorDto.ErrorCode = string.Empty;
    //                resultDto.ErrorDto.Message = exeptionMessage.ToString();
    //                return resultDto;
    //            }
    //            var roleContext = _emamiContext.Roles.FirstOrDefault(x => x.Id == roleIdDto.RoleId);
    //            if (roleContext != null)
    //            {
    //                roleContext.IsActive = false;
    //                roleContext.IsDeleted = true;
    //                roleContext.ModifiedBy = roleIdDto.LoginUserId;
    //                roleContext.ModifiedDate = DateTime.UtcNow;
    //            }
    //            _emamiContext.SaveChanges();
    //            resultDto.IsSuccess = true;
    //            resultDto.SuccessDto.Response = Constants.GetMessage(Constants.RecordSaved, Utility.MessageLanguage);
    //            return resultDto;
    //        }
    //        catch (Exception exception)
    //        {
    //            var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
    //            resultDto.IsSuccess = false;
    //            resultDto.ErrorDto.ErrorCode = Constants.Exception;
    //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
    //            _logger.Error(message);
    //            return resultDto;
    //        }
    //    }

    //    public ResultDto GetRoles()
    //    {
    //        _methodName = "GetRoles";
    //        var resultDto = new ResultDto();
    //        var roleListDto = new List<RoleDto>();
    //        try
    //        {
    //            roleListDto = _emamiContext.Roles.AsNoTracking().Where(_ => _.IsActive && _.Id != (int)DTO.Enums.Role.System).OrderBy(_ => _.Name).Select(_ => new RoleDto
    //            {
    //                Id = _.Id,
    //                Name = _.Name,
    //                Description = _.Description,
    //            }).ToList();

    //            resultDto.IsSuccess = true;
    //            resultDto.SuccessDto.Response = roleListDto;
    //            return resultDto;
    //        }
    //        catch (Exception exception)
    //        {
    //            var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
    //            resultDto.IsSuccess = false;
    //            resultDto.ErrorDto.ErrorCode = Constants.Exception;
    //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
    //            _logger.Error(message);
    //            return resultDto;
    //        }
    //    }

    //    private ResultDto CheckRoleNotAssignedToUser(long roleId)
    //    {
    //        var resultDto = new ResultDto();
    //        _logger.Debug("Check Role assigned to User:" + roleId);
    //        try
    //        {
    //            var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(x => x.RoleId == roleId && x.IsActive);
    //            if (userContext != null)
    //            {
    //                resultDto.IsSuccess = false;
    //                resultDto.ErrorDto.ErrorCode = string.Empty;
    //                resultDto.ErrorDto.Message = userContext.Role.Name;
    //                return resultDto;
    //            }
    //            resultDto.IsSuccess = true;
    //            return resultDto;
    //        }
    //        catch (Exception exception)
    //        {
    //            var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
    //            resultDto.IsSuccess = false;
    //            resultDto.ErrorDto.ErrorCode = Constants.Exception;
    //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
    //            _logger.Error(message);
    //            return resultDto;

    //        }
    //    }
    //}
}
