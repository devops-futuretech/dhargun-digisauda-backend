/****** Object:  StoredProcedure [dbo].[BiddingWindowDashboardReport]    Script Date: 04-10-2019 16:06:47 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'BiddingWindowDashboardReport')
    BEGIN
        DROP  Procedure BiddingWindowDashboardReport
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BiddingWindowDashboardReport] 
	-- Add the parameters for the stored procedure here
	@BiddingWindowId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    
	select Distinct 
		b.Name as BiddingWindowName,	
		cg.Name as customergroup,
		u.Name as DealerName, 
		sbt.Name as SaudaBookingType,
		sk.SkuName as SkuName,
		o.Name as OilName,
		pg.Name as PackGroupName,
		
		s.TotalPrice as QuotedPrice,
		s.BidQuantityInMT, 
		s.BidQuantityInCase,
		s.BidPrice,
		s.BidPricePerCase as BidPricePerCase,
		s.GuarateedPricePerCase as GuarateedPricePerCase,
		s.SchemeDiscount,
		s.VolumeDiscount,
		s.SkuDiscount,
		CASE 
			WHEN s.GPBenefitType = 1 THEN 'SAP'
			WHEN s.GPBenefitType = 2 THEN 'NONSAP'
			ELSE ''
		END as GPBenefitType,

		CASE 
			WHEN s.GPBenefitAppliedTypeId = 1 THEN 'User'
			WHEN s.GPBenefitAppliedTypeId = 2 THEN 'Geography'
			ELSE ''
		END as GPBenefitAppliedType,
		bf.BenefitCategory as GPBenefitCategory,
		s.GPBenefitDiscountOrDay,

		CONVERT(varchar, s.ValidFromDate,111) as SaudaValidFrom,
		CONVERT(varchar, s.ValidToDate,111) as SaudaValidTo,
		DATEDIFF(d,s.ValidFromDate,s.ValidToDate) as SaudaValidityDays,

		Status.Name as Status,		
		st.StateName as State, 
		tp.Name as TransportMode, 
		p.LoadQuantity as LoadQuantity, 
		plant.Name as Plant,
		d.Name as Depot, 
		fz.Name as FreightZone, 
		fr.Name as FreightRoute, 
		CONVERT(varchar, p.BiddingDate,111) as BiddingDate,

		s.CounterBidOffer,
		cs.Name as CounterBidStatus,

		p.ExPlantGuaranteePrice as ExPlantGuaranteePrice,
		p.ForPlantGuaranteePrice as ForPlantGuaranteePrice,
		p.ExDepotGuaranteePrice as ExDepotGuaranteePrice,
		P.ForDepotGuaranteePrice as ForDepotGuaranteePrice, 
		p.ExPlantCGST as ExPlantCGST, 
		P.ExPlantSGST as ExPlantSGST, 
		P.ExPlantIGST as ExPlantIGST, 
		p.ForPlantSGST as ForPlantSGST, 
		p.ForPlantCGST as ForPlantCGST, 
		p.ForPlantIGST as ForPlantIGST, 
		p.ExDepotSGST as ExDepotSGST, 
		p.ExDepotCGST as ExDepotCGST, 
		p.ExDepotIGST as ExDepotIGST, 
		p.ForDepotSGST as ForDepotSGST, 
		p.ForDepotCGST as ForDepotCGST, 
		p.ForDepotIGST as ForDepotIGST,
		p.ExPlantPrice as ExPlantPrice, 
		p.ForPlantPrice as ForPlantPrice, 
		p.ExDepotPrice as ExDepotPrice, 
		p.ForDepotPrice as ForDepotPrice, 
		p.ExDepotPrice as ExDepotPrice, 
		p.SumOfIngredientCost as SumOfIngredientCost, 
		
		p.CushionMargin as CushionMargin,
		p.RaMargin as RaMargin, 
		p.CustomerGroupMargin as CustomerGroupMargin, 
		p.MaterialCost as MaterialCost, 
		p.DepotCost as DepotCost, 
		p.Margin as Margin, 
		
		p.DetentionCost as DetentionCost, 
		p.HoneycombCost as HoneycombCost, 
		p.PackingCost as PackingCost, 
		p.PrimaryFrieght as PrimaryFrieght, 
		p.SecondaryFrieght as SecondaryFrieght, 
		p.SchemeCostRecovery as SchemeCostRecovery,
		
		bdost.StateName as BdoState,
		bdoct.CityName as BdoCity,
		dest.StateName as DealerState,
		dect.Cityname as DealerCity,
		StateTrader.Name as BdoName
			
	from SaudaBiddingCarts as s 
	LEFT JOIN BiddingWindows as b on s.BiddingWindowId = b.Id 
	LEFT JOIN Skus as sk on sk.Id = s.SkuId  
	LEFT JOIN Pricings as p on p.Id = s.PricingId 
	LEFT JOIN SaudaBookingTypes as sbt on p.SaudaBookingTypeId = sbt.Id  
	LEFT JOIN TransportModes as tp on p.TransportModeId = tp.Id 
	LEFT JOIN States as st on p.StateId = st.Id 
	LEFT JOIN FreightZones as fz on p.FrieghtZoneId = fz.Id 
	LEFT JOIN FreightRoutes as fr on p.FrieghtRouteId = fr.Id 
	LEFT JOIN OilTypes as o on s.OilTypeId = o.Id 
	LEFT JOIN PackGroups as pg on pg.Id = sk.PackGroupId
	LEFT JOIN Users as u on s.DealerId = u.Id
	LEFT JOIN BiddingWindowCustomerGroups as bwcg on bwcg.BiddingWindowId = b.Id  
	LEFT JOIN Depots as d on s.DepotId = d.Id 
	LEFT JOIN Depots as plant on s.PlantId = plant.Id  
	LEFT JOIN Status as status on status.Id = S.StatusId
	LEFT JOIN CustomerGroupDetails as cgd on u.Id = cgd.CustomerId
	LEFT JOIN CustomerGroups as cg on cg.Id = cgd.CustomerGroupId
	LEFT JOIN Status as cs on cs.Id = S.CounterBidStatusId
	LEFT JOIN Benefits bf on bf.Id = s.GPBenefitOrCategoryId
	LEFT JOIN UserCustomerMappings ucm on s.DealerId = ucm.CustomerId 
	JOIN UserRoles ur ON ur.UserId = ucm.UserId
	And ur.RoleId = 7 --StateTrader Role
	LEFT JOIN Users StateTrader on StateTrader.Id = ucm.UserId   
	LEFT JOIN States bdost on bdost.Id = StateTrader.StateId   --StateTrader State
	LEFT JOIN Cities bdoct on bdoct.Id = StateTrader.CityId   --StateTrader City
	LEFT JOIN States dest on dest.Id = u.StateId   --Dealer State
	LEFT JOIN Cities dect on dect.Id = u.CityId   --StateTrader City

	where s.BiddingWindowId = @BiddingWindowId
	AND cg.IsActive = 1

	END

--EXEC BiddingWindowDashboardReport 587

--Select * from SaudaBiddingCarts

--EXEC BiddingWindowDashboardReport 587
