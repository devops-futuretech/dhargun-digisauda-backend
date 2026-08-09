USE [AdaniTryed]
GO

/****** Object:  StoredProcedure [dbo].[SP_StockReport]    Script Date: 16-06-2022 07:55:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_StockReport] 
	-- Add the parameters for the stored procedure here
	--@StartDate DateTime,
	--@EndDate DateTime,
	@PlantId int,
	@MaterialId int,
	@Name varchar(100),
	@MaterialDescription varchar(100),
	@Message varchar(100)
AS
DECLARE @PlantName varchar(100)
DECLARE @SLoc varchar(100)
DECLARE @BUn varchar(100)
DECLARE @Unrestricted varchar(100)
DECLARE @QualityInsp varchar(100)
DECLARE @Blocked varchar(100)
DECLARE @TransTfr varchar(100)
DECLARE @Material varchar(100)
set @Name = ltrim(rtrim(@Name))


Set NOCOUNT OFF

BEGIN TRANSACTION


--Plant/Depot begins

IF NOT EXISTS(Select 1 FROM Depots Where [Id]= @PlantId)
BEGIN
	ROLLBACK
	Select @PlantName as PlantName,
	@Name as Name,
	@MaterialDescription as MaterialDescription,
	@SLoc as SLoc,
	@BUn as BUn,
	@Unrestricted as Unrestricted,
	@QualityInsp as QualityInsp,
	@Blocked as Blocked,
	@TransTfr as TransTfr,
	@Material as Material,
	'Failed, Plant Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @PlantName = [Name] From Depots Where [Id] = @PlantId

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @PlantName as PlantName,
	@Name as Name,
	@MaterialDescription as MaterialDescription,
	@SLoc as SLoc,
	@BUn as BUn,
	@Unrestricted as Unrestricted,
	@QualityInsp as QualityInsp,
	@Blocked as Blocked,
	@TransTfr as TransTfr,
	@Material as Material,
	'Failed In Depot/Plant' as 'Message'      
RETURN 
END 
--Plant/Depot Ends

--Material Type begins

IF NOT EXISTS(Select 1 FROM MaterialTypes Where [Id]= @MaterialId)
BEGIN
	ROLLBACK
	Select @PlantName as PlantName,
	@Name as Name,
	@MaterialDescription as MaterialDescription,
	@SLoc as SLoc,
	@BUn as BUn,
	@Unrestricted as Unrestricted,
	@QualityInsp as QualityInsp,
	@Blocked as Blocked,
	@TransTfr as TransTfr,
	@Material as Material,
	'Failed, MaterialType Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @Material = [Name] From MaterialTypes Where [Id] = @MaterialId

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @PlantName as PlantName,
	@Name as Name,
	@MaterialDescription as MaterialDescription,
	@SLoc as SLoc,
	@BUn as BUn,
	@Unrestricted as Unrestricted,
	@QualityInsp as QualityInsp,
	@Blocked as Blocked,
	@TransTfr as TransTfr,
	@Material as Material,
	'Failed In Material' as 'Message'      
RETURN 
END 
--Material Type Ends

BEGIN
	Select @PlantName as PlantName,
	@Name as Name,
	@MaterialDescription as MaterialDescription,
	@SLoc as SLoc,
	@BUn as BUn,
	@Unrestricted as Unrestricted,
	@QualityInsp as QualityInsp,
	@Blocked as Blocked,
	@TransTfr as TransTfr,
	@Material as Material,
	'Success' as 'Message'
END

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @PlantName as PlantName,
	@Name as Name,
	@MaterialDescription as MaterialDescription,
	@SLoc as SLoc,
	@BUn as BUn,
	@Unrestricted as Unrestricted,
	@QualityInsp as QualityInsp,
	@Blocked as Blocked,
	@TransTfr as TransTfr,
	@Material as Material,
	'Failed In Report' as 'Message'   
RETURN 
END 
COMMIT
GO


