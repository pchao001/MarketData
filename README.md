# MarketData 技術架構
![image](https://docs.microsoft.com/zh-tw/dotnet/architecture/modern-web-apps-azure/media/image5-7.png) 


## Required Software
- SQL Server 2016 express
- VS Code / Visutial Studio 2019

## Database Setup
- CREATE DATABASE [MarketDataDB]
- run the following script
```
use master
CREATE DATABASE [MarketDataDB]
go
use MarketDataDB
go
CREATE TABLE [dbo].[MarketData](
	[StoreDate] [varchar](50) NOT NULL,
	[StockCode] [varchar](50) NOT NULL,
	[StockName] [varchar](50) NULL,
	[YieldRate] [decimal](18, 4) NULL,
	[DividendYear] [int] NULL,
	[PERatio] [decimal](18, 4) NULL,
	[PBR] [decimal](18, 4) NULL,
	[FiscalYearQ] [varchar](50) NULL)
```
## Build
- dotnet build

## Test
- dotnet test

## Run
- cd MarketData.API
- dotnet run

## Function Scopt
## 
