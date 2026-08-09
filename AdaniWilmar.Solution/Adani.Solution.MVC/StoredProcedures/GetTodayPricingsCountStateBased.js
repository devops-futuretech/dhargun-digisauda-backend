CREATE PROCEDURE[dbo].[GetTodayPricingsCountStateBased]

AS
Select Distinct StateId, sb.Name as SaudaBookingType, sb.Id as SaudaBookingTypeId, s.StateName, v.Id as VerticalId, v.Name as VerticalName, Count(p.Id) as RecordCount From PricingBackups p Join States s ON p.StateId = s.Id
Join SaudaBookingTypes sb ON sb.Id = p.SaudaBookingTypeId
join Skus sku ON p.SkuId = sku.Id
join Verticals v ON sku.VerticalId = v.Id
Group By StateId, sb.Id, sb.Name, s.StateName, v.Id, v.Name Order By StateId desc
RETURN 0