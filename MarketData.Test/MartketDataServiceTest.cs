using MarketData.Application;
using MarketData.Model;
using MarketData.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;

namespace MarketData.Test
{
    [TestClass]
    public class MartketDataServiceTest
    {
        private readonly IConfigurationRoot _config;
        private readonly HttpClient _client;
        private readonly IMarketDataRepository _repo;
        private readonly IMarketDataService _service;
        public MartketDataServiceTest()
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
                _service.Import(sDate);
                storeDate = storeDate.AddDays(1);
            }
        }


        [TestMethod]
        public void ImportTest()
        {

            var config = TestHelper.ReadFromAppSettings();
            var client = new HttpClient { BaseAddress = new Uri(config["MarketDataUrl"]) };

            var repo = new MarketDataRepository(config);

            var storeDate = "20210629";
            var service = new MarketDataService(config, client, repo);
            var result = service.Import(storeDate).Result;

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetLatestDataTest()
        {

            var stockCode = "1101";
            var result = _service.GetLatestData(stockCode, 10).Result;

            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]

        public void GetLatestTopPERatioDataTest()
        {

            var storeDate = "20210628";
            var tops = 10;
            var result =
                _service.GetLatestTopPERatioData(storeDate, tops).Result;

            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetStrictYieldRateRangeTest()
        {
            var stockCode = "1108";
            var fromDate = "20210601";
            var endDate = "20210628";
            var result = _service.GetStrictYieldRateRange(stockCode,
                    fromDate, endDate).Result;


            Assert.IsTrue(result.StartDate == "20210608");
            Assert.IsTrue(result.EndDate == "20210610");

            Assert.IsTrue(result.Lists.Count == 3);

        }




    }
}
