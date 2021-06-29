using MarketData.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarketData.Application
{
    public  interface IMarketDataService
    {
        Task Import(string storeDate);

        /// <summary>
        /// 依照證券代號 搜尋最近n天的資料
        /// </summary>
        /// <param name="stockId"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        Task<List<MarketModel>> GetLatestData(string stockId, int days);

        /// <summary>
        /// 指定特定日期 顯示當天本益比前n名
        /// </summary>
        /// <param name="storeDate"></param>
        /// <param name="tops"></param>
        /// <returns></returns>
        Task<List<MarketModel>> GetLatestTopPERatioData(string storeDate, int tops);
    }
}
