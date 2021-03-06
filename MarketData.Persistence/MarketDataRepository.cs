using Dapper;
using MarketData.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Http;
using System.Text;
 
using System.Threading.Tasks;

namespace MarketData.Persistence
{
    public class MarketDataRepository : IMarketDataRepository
    {
         
        private readonly IConfiguration _configuration;
        public MarketDataRepository(IConfiguration configuration)
        {
            
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        }
        
        public async Task<List<MarketModel>> GetLatestData(string stockId, int days)
        {
            var connString = _configuration["ConnectionStrings:MarketDataDB"];
            using (var connection = new SqlConnection(connString))
            {
                var sql = $"select top {days} * " +
                    @" from MarketData 
                        where StockCode=@StockCode
                        order by StoreDate desc";

                
                var result = await connection.QueryAsync<MarketModel>(sql,
                    new { StockCode = stockId, Days=days });

                return result.AsList<MarketModel>();
            }

                
        }

        public async Task<List<MarketModel>> GetLatestTopPERatioData(string storeDate, int tops)
        {
            var connString = _configuration["ConnectionStrings:MarketDataDB"];
            using (var connection = new SqlConnection(connString))
            {
                var sql = $"select top {tops} * " +
                    @" from MarketData 
                        where StoreDate=@StoreDate
                        order by PERatio desc";


                var result = await connection.QueryAsync<MarketModel>(sql,
                    new { StoreDate = storeDate });

                return result.AsList<MarketModel>();
            }
        }

        public async Task<List<MarketModel>> GetDataByRange(string stockCode, string fromDate, string endDate)
        {
            var connString = _configuration["ConnectionStrings:MarketDataDB"];
            using (var connection = new SqlConnection(connString))
            {
                //排除不是遞增的資料

                var sql = @" select a.*
                    from MarketData a
                    left join MarketData after
                        on a.StockCode = after.StockCode
                        and Cast(a.StoreDate as date) = DATEADD(day, -1, after.StoreDate)
                    where a.StockCode = @StockCode
                        and a.StoreDate >= @FromDate and a.StoreDate <= @EndDate
                        and after.YieldRate - a.YieldRate > 0
                ";

                try
                {
                    var result =  connection.QueryAsync<MarketModel>(sql,
                      new
                      {
                          StockCode = stockCode,
                          FromDate = fromDate,
                          EndDate = endDate
                      }).Result;

                    return result.AsList<MarketModel>();
                }
                catch (Exception)
                {

                    throw;
                }
              
            }
        }


        public async Task<bool> Create(MarketModel model)
        {
            var connString = _configuration["ConnectionStrings:MarketDataDB"];
            using var connection = new SqlConnection(connString);
            var affected =
               await connection.ExecuteAsync
                   ("INSERT INTO MarketData (StoreDate, StockCode, StockName,YieldRate,DividendYear,PERatio,PBR,FiscalYearQ) " +
                   "VALUES (@StoreDate, @StockCode, @StockName,@YieldRate,@DividendYear,@PERatio,@PBR,@FiscalYearQ)",
                    model);

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> Delete(string storeDate)
        {
            var connString = _configuration["ConnectionStrings:MarketDataDB"];
            using var connection = new SqlConnection(connString);
            var affected =
               await connection.ExecuteAsync
                   ("delete from  MarketData where StoreDate=@StoreDate",
                   new { StoreDate = storeDate });

            if (affected == 0)
                return false;

            return true;
        }

      
    }
}

 

    
 
