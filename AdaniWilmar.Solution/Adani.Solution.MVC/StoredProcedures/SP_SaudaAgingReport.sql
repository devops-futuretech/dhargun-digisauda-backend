USE [Adani_06]
GO

/****** Object:  StoredProcedure [dbo].[SP_SaudaAgingReport]    Script Date: 26-07-2022 18:03:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author: <Author,,Name>
-- Create date: <Create Date,,>
-- Description: <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_SaudaAgingReport]
-- Add the parameters for the stored procedure here
--@StartDate DateTime,
--@EndDate DateTime,

@SaudaAging int,
@DepotId int,
@Party varchar(100),
@PartyName varchar(100),
@CityId int,
@MaterialDescription varchar(100)

AS
DECLARE @BaseDepot varchar(100)
DECLARE @City varchar(100)
DECLARE @PO_Date DateTime
DECLARE @ContractNumber varchar(100)
DECLARE @Date DateTime
DECLARE @ContractEndDate DateTime
DECLARE @ContractQuantity int
DECLARE @OS_Quantity int

set @Party = ltrim(rtrim(@Party))
set @PartyName = ltrim(rtrim(@PartyName))
set @MaterialDescription = ltrim(rtrim(@MaterialDescription))


IF NOT EXISTS(Select 1 From Cities Where [Id] = @CityId)
BEGIN
Select @BaseDepot as BaseDepot,
@Party as Party,
@PartyName as PartyName,
@City as City,
@PO_Date as PO_Date,
@ContractNumber as ContractNumber,
@MaterialDescription as MaterialDescription,
@Date as Date,
@SaudaAging as SaudaAging,
@ContractEndDate as ContractEndDate,
@ContractQuantity as ContractQuantity,
@OS_Quantity as OS_Quantity
RETURN
END
ELSE
     Select @City = [CityName] From Cities Where [Id] = @CityId

IF @@ERROR <> 0
BEGIN
Select @BaseDepot as BaseDepot,
@Party as Party,
@PartyName as PartyName,
@City as City,
@PO_Date as PO_Date,
@ContractNumber as ContractNumber,
@MaterialDescription as MaterialDescription,
@Date as Date,
@SaudaAging as SaudaAging,
@ContractEndDate as ContractEndDate,
@ContractQuantity as ContractQuantity,
@OS_Quantity as OS_Quantity
RETURN
END

IF NOT EXISTS(Select 1 From Depots Where [Id] = @DepotId)
BEGIN
Select @BaseDepot as BaseDepot,
@Party as Party,
@PartyName as PartyName,
@City as City,
@PO_Date as PO_Date,
@ContractNumber as ContractNumber,
@MaterialDescription as MaterialDescription,
@Date as Date,
@SaudaAging as SaudaAging,
@ContractEndDate as ContractEndDate,
@ContractQuantity as ContractQuantity,
@OS_Quantity as OS_Quantity
RETURN
END
ELSE
     Select @BaseDepot = [Name] From Depots Where [Id] = @DepotId

IF @@ERROR <> 0
BEGIN
Select @BaseDepot as BaseDepot,
@Party as Party,
@PartyName as PartyName,
@City as City,
@PO_Date as PO_Date,
@ContractNumber as ContractNumber,
@MaterialDescription as MaterialDescription,
@Date as Date,
@SaudaAging as SaudaAging,
@ContractEndDate as ContractEndDate,
@ContractQuantity as ContractQuantity,
@OS_Quantity as OS_Quantity
RETURN
END

BEGIN
Select @BaseDepot as BaseDepot,
@Party as Party,
@PartyName as PartyName,
@City as City,
@PO_Date as PO_Date,
@ContractNumber as ContractNumber,
@MaterialDescription as MaterialDescription,
@Date as Date,
@SaudaAging as SaudaAging,
@ContractEndDate as ContractEndDate,
@ContractQuantity as ContractQuantity,
@OS_Quantity as OS_Quantity
RETURN
END


IF @@ERROR <> 0

BEGIN
Select @BaseDepot as BaseDepot,
@Party as Party,
@PartyName as PartyName,
@City as City,
@PO_Date as PO_Date,
@ContractNumber as ContractNumber,
@MaterialDescription as MaterialDescription,
@Date as Date,
@SaudaAging as SaudaAging,
@ContractEndDate as ContractEndDate,
@ContractQuantity as ContractQuantity,
@OS_Quantity as OS_Quantity
RETURN
END

COMMIT
GO


