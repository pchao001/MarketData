using MarketData.Model;
using MarketData.Persistence;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MarketData.Application
{
    public class MarketDataService : IMarketDataService
    {
        private readonly IMarketDataRepository _repo;
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public MarketDataService(IConfiguration configuration, HttpClient client, IMarketDataRepository repo)
        {
            _client = client;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _repo = repo;
        }
        //import
        public async Task<bool> Import(string storeDate)
        {
            var url = $"/exchangeReport/BWIBBU_d?response=json&date={storeDate}&selectType=ALL";
            // insert into table
            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            JObject jsonString = JObject.Parse(dataAsString);
            var data = (JArray)jsonString["data"];

            if (data == null)
            {
                return false;
            }
            await _repo.Delete(storeDate);

            foreach (var item in data)
            {

                var model = new MarketModel
                {
                    StoreDate = storeDate,
                    StockCode = item[0].Value<string>(),
                    StockName = item[1].Value<string>(),
                    YieldRate = item[2].Value<decimal>(),
                    DividendYear = item[3].Value<int>(),
                    PERatio = ParsePERatio(item[4]),
                    PBR = item[5].Value<decimal>(),
                    FiscalYearQ = item[6].Value<string>()
                };

                Console.WriteLine(model);
                await _repo.Create(model);
            }

            return true;
        }

        public async Task<List<MarketModel>> GetLatestData(string stockId, int days)
        {
            return await _repo.GetLatestData(stockId, days);
        }

        public async Task<List<MarketModel>> GetLatestTopPERatioData(string storeDate, int tops)
        {
            return await _repo.GetLatestTopPERatioData(storeDate, tops);
        }

        public async Task<YieldRateModel> GetStrictYieldRateRange(
            string stockCode, 
            string fromDate, 
            string endDate)
        {
            var result = new YieldRateModel
            {
                StockCode = stockCode,
                StartDate="19000101"
            };

            var data = await _repo.GetDataByRange(stockCode, fromDate, endDate);
            bool resetStart = false; // 
            for (int i = 0; i < data.Count-1; i++)
            {
                //如果是連續遞增日期 且YieldRate也是遞增 
                if (IsNextDate(data[i].StoreDate, data[i+1].StoreDate) && 
                    data[i+1].YieldRate > data[i].YieldRate)
                {
                    // StartData尚未初始化
                    if (!resetStart)
                    {
                        //設定起日
                        result.StartDate = data[i].StoreDate;
                        
                        // 新增起日YieldPair (Date,Rate)
                        result.Lists.Clear();

                        result.Lists.Add(new YieldPair
                        {
                            StoreDate = data[i].StoreDate,
                            YieldRate = data[i].YieldRate
                        });
                        resetStart = true;
                    }

                    // 設定EndDate
                    result.EndDate = data[i + 1].StoreDate;
                    result.Lists.Add(new YieldPair
                    {
                        StoreDate = data[i+1].StoreDate,
                        YieldRate = data[i+1].YieldRate
                    });
                }
                else
                {
                    //如果次日Rate 較小,則重新計算
                    resetStart = false;
                }

            }


            return result;
        }

        public async Task<bool> Create(MarketModel model)
        {
            return await _repo.Create(model);
        }
        /// <summary>
        /// 判斷是否為連續日期
        /// </summary>
        /// <param name="curDate"></param>
        /// <param name="nextDate"></param>
        /// <returns></returns>
        private bool IsNextDate(string curDate, string nextDate)
        {
           
            var dCurDate = DateTime.ParseExact(curDate, "yyyyMMdd", null).AddDays(1);
            var dNextDate = DateTime.ParseExact(nextDate, "yyyyMMdd", null);

            return dCurDate == dNextDate;
        }

        /// <summary>
        /// 解析PE Ratio
        /// </summary>
        /// <param name="token"></param>
        /// <returns>PE Ratio 如果不是數字,則轉換成0</returns>
        private decimal ParsePERatio(JToken token)
        {
            decimal result = 0;
            try
            {
                result = token.Value<decimal>();
            }
            catch (Exception)
            {

                result = 0;
            }
            return result;


        } // m
    }
}
