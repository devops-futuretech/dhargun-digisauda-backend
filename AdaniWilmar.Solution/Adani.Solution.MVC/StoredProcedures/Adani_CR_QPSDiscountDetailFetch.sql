CREATE Type dbo.SkuQps
As Table(
SkuId bigint Not Null,
Quantity decimal Not Null
);

Create or Alter Procedure GetQpsDiscountDetailForSku(
	@skuQps SkuQps Readonly
	)
As
Begin
SET NOCOUNT ON;

    WITH SkuOilTypes AS (
        SELECT
            s.Id,
            s.OilTypeId,
            sq.Quantity
        FROM
            @skuQps sq
        JOIN
            Skus s ON sq.SkuId = s.Id
		where s.IsActive = 1
    ),
    OilTypeQuantities AS (
        SELECT
            OilTypeId,
            SUM(Quantity) AS TotalQuantity
        FROM
            SkuOilTypes
        GROUP BY
            OilTypeId
    )

    SELECT Distinct
        so.Id as SkuId,
        COALESCE(sd.DiscountAmount, 0) as Discount
    FROM
		SkuOilTypes so
	JOIN
		OilTypeQuantities oqt on so.OilTypeId = oqt.OilTypeId
    JOIN
        QpsDiscounts q ON so.OilTypeId = q.OilTypeId
	Left JOIN
		SlabDiscountDetails sd ON q.Id = sd.QPSId
		AND oqt.TotalQuantity BETWEEN sd.FromRange AND sd.ToRange
	WHERE q.IsActive = 1

End





        


		
        

        