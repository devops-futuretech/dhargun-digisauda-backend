
/****** Object:  StoredProcedure [dbo].[TodayPricingBackupForSpecialRate]    Script Date: 08-09-2022 10:20:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[TodayPricingBackupForSpecialRate]
  (@PricingIds varchar(max))
	AS
	BEGIN
	
SET IDENTITY_INSERT SpecialRatePricingHistories ON

			INSERT INTO SpecialRatePricingHistories(Id,
			SkuId,
			OilTypeId,
			OilPackingTypeId,
			PlantId,
			SAPPricingCode,
			SkuCode,
			PlantCode,
			Price,
			SalesOrganizationId,
			SalesOrganization,
			DistributionChannel,
			DistributionChannelId,
			PricingReferneceId,
			DivisionId,
			Division,
			ValidFrom,
			ValidTo,
			PerUnit,
			CreatedBy,
			CreatedDate,
			ModifiedBy,
			ModifiedDate
				)

			SELECT Id,
			SkuId,
			OilTypeId,
			OilPackingTypeId,
			PlantId,
			SAPPricingCode,
			SkuCode,
			PlantCode,
			Price,
			SalesOrganizationId,
			SalesOrganization,
			DistributionChannel,
			DistributionChannelId,
			PricingReferneceId,
			DivisionId,
			Division,
			ValidFrom,
			ValidTo,
			PerUnit,
			CreatedBy,
			CreatedDate,
			ModifiedBy,
			ModifiedDate
			FROM TodayPricings where Id in (select value from string_split(@PricingIds,',')) 
		    
			SET IDENTITY_INSERT SpecialRatePricingHistories OFF		
	END

	
	--EXEC TodayPricingBackupForSpecialRate '1122418,1121956'
GO


