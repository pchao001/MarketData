using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MarketData.Model;

namespace MarketData.Model
{
    public interface IMarketDataRepository
    {
        #region Query
        /// <summary>
        /// 依照證券代號 搜尋最近n天的資料
        /// </summary>
        /// <param name="stockId"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        Task<List<MarketModel>> GetLatestData(string stockId, int days);
        Task<List<MarketModel>> GetLatestTopPERatioData(string storeDate, int tops);

        Task<List<MarketModel>> GetDataByRange(string stockCode, string fromDate, string endDate);

        #endregion

        #region CRUD
        Task<bool> Create(MarketModel model);
        Task<bool> Delete(string storeDate);
        #endregion

    }
}
