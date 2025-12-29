
create table dbo.tbl_transaction
(
	Trade_Num int,
	Trade_Date datetime,
	Trade_Currency varchar(20),
	Trade_Price decimal(20,10),
	Trade_Maturity datetime,
	Trade_Status varchar(100)
)
insert into tbl_transaction values(1001, '2025-12-20', 'INR', 200.00, '2025-12-22', 'Successfully Processed')
insert into tbl_transaction values(1002, '2025-12-20', 'INR', 300.00, '2025-12-22', 'Successfully Processed')
insert into tbl_transaction values(1003, '2025-12-20', 'INR', 400.00, '2025-12-22', 'Cancelled')
select * from dbo.tbl_transaction


CREATE PROCEDURE dbo.sp_transaction_file_loader_new
@Trade_Num int,
@Trade_Date datetime,
@Trade_Currency varchar(20),
@Trade_Price decimal(20,10),
@Trade_Maturity datetime,
@Trade_Status varchar(100)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN
        INSERT INTO dbo.tbl_transaction (Trade_Num, Trade_Date, Trade_Currency, Trade_Price, Trade_Maturity, Trade_Status) 
		VALUES (@Trade_Num, @Trade_Date, @Trade_Currency, @Trade_Price, @Trade_Maturity, @Trade_Status);
    END
END