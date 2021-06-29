using System;

namespace MarketData.Model
{
    public class MarketModel
    {
        public string StoreDate { get; set; }

        /// <summary>
        /// 證券代號
        /// </summary>
        public string StockCode { get; set; }

        /// <summary>
        /// "證券名稱
        /// </summary>
        public string StockName { get; set; }

        /// <summary>
        /// 殖利率(%)
        /// </summary>
        public decimal YieldRate { get; set; }

        /// <summary>
        /// 股利年度
        /// </summary>
        public int DividendYear { get; set; }

        /// <summary>
        /// 本益比
        /// </summary>
        public decimal PERatio { get; set; }
        //

        /// <summary>
        /// 股價淨值比
        /// </summary>
        public decimal PBR { get; set; }

        //
        /// <summary>
        /// 財報年/季
        /// </summary>
        public string FiscalYearQ { get; set; }

        public override string ToString()
        {
            return $"[{nameof(StoreDate)}:{StoreDate},"
                + $"[{nameof(StockCode)}:{StockCode}," 
                + $"{nameof(StockName)}:{StockName}, "
                + $"{nameof(YieldRate)}:{YieldRate}, "
                + $"{nameof(DividendYear)}:{DividendYear}, "
                + $"{nameof(PERatio)}:{PERatio}, "
                + $"{nameof(PBR)}:{PBR}, "
                + $"{nameof(FiscalYearQ)}:{FiscalYearQ}, "
                + "]"; 
        }
        

    }
}
