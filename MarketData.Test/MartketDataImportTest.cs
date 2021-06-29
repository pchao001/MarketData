using MarketData.Application;
using MarketData.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;

namespace MarketData.Test
{
    [TestClass]
    public class MartketDataImportTest
    {
        private readonly IConfigurationRoot _config;
        private readonly HttpClient _client;
        private readonly IMarketDataRepository _repo;
        private readonly IMarketDataService _service;
        public MartketDataImportTest()
        {
            _config = TestHelper.ReadFromAppSettings();
            _client = new HttpClient { BaseAddress = new Uri(_config["MarketDataUrl"]) };

            _repo = new MarketDataRepository(_config);
            _service = new MarketDataService(_config, _client, _repo);

        }
        public void ImportRangeTest()
        {
            var length = 25;
            var storeDate = System.DateTime.Parse("2021/06/01");
            for (int i = 1; i < length; i++)
            {
                var sDate = storeDate.ToString("yyyyMMdd");
                System.Console.WriteLine(sDate);
                ImportTest(sDate);
                storeDate = storeDate.AddDays(1);
            }
        }
        //[TestMethod]
        public void ImportTest(string storeDate)
        {
           
            var config = TestHelper.ReadFromAppSettings();
            var client = new HttpClient { BaseAddress = new Uri(config["MarketDataUrl"]) };

            var repo = new MarketDataRepository(config);

            if (string.IsNullOrEmpty( storeDate))
                storeDate = "20210628";
            var service = new MarketDataService(config, client, repo);
            service.Import(storeDate).Wait();
        }

        public void GetLatestDataTest()
        {
            
            var stockCode = "1101";
            var result = _service.GetLatestData(stockCode, 10).Result;

            Assert.IsTrue(result.Count > 0);
        }

        public void GetLatestTopPERatioData()
        {

            var storeDate = "20210628";
            var tops = 10;
            var result = 
                _service.GetLatestTopPERatioData(storeDate, tops).Result;

            Assert.IsTrue(result.Count > 0);
        }
    }
}
