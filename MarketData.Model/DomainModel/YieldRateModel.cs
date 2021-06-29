using System;
using System.Collections.Generic;
using System.Text;

namespace MarketData.Model
{

    public class YieldPair
    {
        public string StoreDate { get; set; }
        public decimal YieldRate { get; set; }
    }
    /// <summary>
    /// 嚴格遞增的最長天數並顯示開始、結束日期
    /// </summary>
    public class YieldRateModel
    {
        public string StockCode { get; set; }

        public List<YieldPair> Lists { get; set; } = new List<YieldPair>();

      
        public string StartDate { get; set; }
        public string EndDate { get; set; }

    }
}
