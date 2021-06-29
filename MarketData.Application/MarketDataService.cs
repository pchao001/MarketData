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
        public async Task Import(string storeDate)
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
                return;
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


        }

        public async Task<List<MarketModel>> GetLatestData(string stockId, int days)
        {
            return await _repo.GetLatestData(stockId, days);
        }

        public async Task<List<MarketModel>> GetLatestTopPERatioData(string storeDate, int tops)
        {
            return await _repo.GetLatestTopPERatioData(storeDate, tops);
        }

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
