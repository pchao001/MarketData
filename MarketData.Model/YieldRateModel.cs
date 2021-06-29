using System;
using System.Collections.Generic;
using System.Text;

namespace MarketData.Model
{
    /// <summary>
    /// 嚴格遞增的最長天數並顯示開始、結束日期
    /// </summary>
    public class YieldRateModel
    {
        public string StoreDate { get; set; }
        public string StockCode { get; set; }
        public decimal YeildRate { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }

    }
}
