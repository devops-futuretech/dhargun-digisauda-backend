ALTER PROCEDURE [dbo].[ResetQuantityLimitsData]
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @CurrentDate DATE = CAST(GETDATE() AS DATE);
	PRINT @CurrentDate;
	-- CTE to find active quantity limits that haven't been updated today
	WITH CTE_ActiveQuantityLimit AS
	(
		SELECT Id
		FROM SpecalityFatDiscountUsers WITH (NOLOCK)
		WHERE 
			@CurrentDate BETWEEN CAST(ValidFrom AS DATE) AND CAST(ValidTo AS DATE)
			AND (ModifiedDate IS NULL OR CAST(ModifiedDate AS DATE) != @CurrentDate)
	)

	-- Update RemainingQuantity to ActualDiscount
	UPDATE SpecalityFatDiscountUsers
	SET RemainingQuantity = ActualDiscount,
		ModifiedDate = GETDATE()  -- Optional: track the update time
	WHERE Id IN (SELECT Id FROM CTE_ActiveQuantityLimit)

END
GO
