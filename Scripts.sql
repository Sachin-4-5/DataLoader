-- *************************************************************************************
-- Developer:  sachin kumar
-- Description: complete script for DataLoader application.
-- Purpose:  useful in loading transaction data into table.
-- Note: It's recommended to execute script before running the dataloader application.
-- *************************************************************************************


-- Database
CREATE DATABASE TransactionDB



-- Table
CREATE TABLE dbo.t_transaction_trade_data
(
    TD_NUM VARCHAR(250) NULL,
    TRD_TRADE_DATE VARCHAR(250) NULL,
    TRD_CURRENCY VARCHAR(250) NULL,
    TRD_PRICE VARCHAR(250) NULL,
    TRD_PRINCIPAL DECIMAL(20,10) NULL,
    TRD_SETTLE_DATE VARCHAR(250) NULL,
    TRD_STATUS VARCHAR(250) NULL,
    COUNTERPARTY_CODE VARCHAR(250) NULL,
    CUSIP VARCHAR(250) NULL,
    FUND VARCHAR(250) NULL,
    INVNUM VARCHAR(250) NULL,
    MATURITY VARCHAR(250) NULL,
    TICKER VARCHAR(250) NULL,
    TOUCH_COUNT VARCHAR(250) NULL,
    TRAN_TYPE VARCHAR(250) NULL
);



-- Stored Procedure
CREATE OR ALTER PROCEDURE dbo.sp_load_transaction_file
@TD_NUM VARCHAR(250),
@TRD_TRADE_DATE VARCHAR(250),
@TRD_CURRENCY VARCHAR(250),
@TRD_PRICE VARCHAR(250),
@TRD_PRINCIPAL DECIMAL(20,10),
@TRD_SETTLE_DATE VARCHAR(250),
@TRD_STATUS VARCHAR(250),
@COUNTERPARTY_CODE VARCHAR(250),
@CUSIP VARCHAR(250),
@FUND VARCHAR(250),
@INVNUM VARCHAR(250),
@MATURITY VARCHAR(250),
@TICKER VARCHAR(250),
@TOUCH_COUNT VARCHAR(250),
@TRAN_TYPE VARCHAR(250)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN
        INSERT INTO dbo.t_transaction_trade_data 
		(
            TD_NUM,
            TRD_TRADE_DATE,
            TRD_CURRENCY,
            TRD_PRICE,
            TRD_PRINCIPAL,
            TRD_SETTLE_DATE,
            TRD_STATUS,
            COUNTERPARTY_CODE,
            CUSIP,
            FUND,
            INVNUM,
            MATURITY,
            TICKER,
            TOUCH_COUNT,
            TRAN_TYPE
        ) 
		VALUES 
		(
            @TD_NUM,
            @TRD_TRADE_DATE,
            @TRD_CURRENCY,
            @TRD_PRICE,
            @TRD_PRINCIPAL,
            @TRD_SETTLE_DATE,
            @TRD_STATUS,
            @COUNTERPARTY_CODE,
            @CUSIP,
            @FUND,
            @INVNUM,
            @MATURITY,
            @TICKER,
            @TOUCH_COUNT,
            @TRAN_TYPE
        );
    END
END;
