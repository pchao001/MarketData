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

        [HttpGet()]
        [Route("GetLatestData")]
        public async Task<ActionResult<List<MarketModel>>> GetLatestData(string stockId, int days)
        {
            var result = await _service.GetLatestData(stockId, days);
            return Ok(result);
        }

        [HttpGet()]
        [Route("GetLatestTopPERatioData")]
        public async Task<ActionResult<List<MarketModel>>> GetLatestTopPERatioData(string storeDate, int tops)
        {
            var result = await _service.GetLatestTopPERatioData(storeDate, tops);
            return Ok(result);
        }

    }
}
