using MarketData.Application;
using MarketData.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketData.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketDataController : ControllerBase
    {
        private readonly IMarketDataService _service;

        public MarketDataController(IMarketDataService service)
        {
            _service = service;
        }

        /// <summary>
        /// 依照證券代號 搜尋最近n天的資料
        /// </summary>
        /// <param name="stockId">證券代號</param>
        /// <param name="days">最近n天</param>
        /// <returns></returns>
        [HttpGet()]
        [Route("GetLatestData")]
        public async Task<ActionResult<List<MarketModel>>> GetLatestData(string stockId, int days)
        {
            var result = await _service.GetLatestData(stockId, days);
            return Ok(result);
        }

        /// <summary>
        /// 指定特定日期 顯示當天本益比前n名
        /// </summary>
        /// <param name="storeDate">特定日期</param>
        /// <param name="tops">前n名</param>
        /// <returns></returns>
        [HttpGet()]
        [Route("GetLatestTopPERatioData")]
        public async Task<ActionResult<List<MarketModel>>> GetLatestTopPERatioData(string storeDate, int tops)
        {
            var result = await _service.GetLatestTopPERatioData(storeDate, tops);
            return Ok(result);
        }

        /// <summary>
        /// 指定日期範圍、證券代號 顯示這段時間內殖利率 
        /// 為嚴格遞增的最長天數並顯示開始、結束日期
        /// </summary>
        /// <param name="stockCode">證券代號</param>
        /// <param name="fromDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet()]
        [Route("GetStrictYieldRateRange")]
        public async Task<ActionResult<YieldRateModel>> GetStrictYieldRateRange(
            string stockCode, string fromDate, string endDate)
        {
            var result = await _service.GetStrictYieldRateRange(stockCode, fromDate, endDate);

            return Ok(result);
        }

        /// <summary>
        /// 匯入Market Data
        /// </summary>
        /// <param name="storeDate"></param>
        /// <returns></returns>
        [HttpPost()]
        [Route("Import")]
        public async Task<ActionResult> Import(
           string storeDate)
        {
            var result = await _service.Import(storeDate);

            return Ok(result);
        }


        [HttpPost()]
        [Route("Create")]
        public async Task<ActionResult> Create(MarketModel model)
        {
            var result = await _service.Create(model);

            return Ok(result);
        }

    }
}
