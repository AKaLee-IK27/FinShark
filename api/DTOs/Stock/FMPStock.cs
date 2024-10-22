namespace api.Dtos.Stock;

public class FMPStock
    {
        public required string symbol { get; set; }
        public double price { get; set; }
        public double beta { get; set; }
        public int volAvg { get; set; }
        public long mktCap { get; set; }
        public double lastDiv { get; set; }
        public required string range { get; set; }
        public double changes { get; set; }
        public required string companyName { get; set; }
        public required string currency { get; set; }
        public required string cik { get; set; }
        public required string isin { get; set; }
        public required string cusip { get; set; }
        public required string exchange { get; set; }
        public required string exchangeShortName { get; set; }
        public required string industry { get; set; }
        public required string website { get; set; }
        public required string description { get; set; }
        public required string ceo { get; set; }
        public required string sector { get; set; }
        public required string country { get; set; }
        public required string fullTimeEmployees { get; set; }
        public required string phone { get; set; }
        public required string address { get; set; }
        public required string city { get; set; }
        public required string state { get; set; }
        public required string zip { get; set; }
        public double dcfDiff { get; set; }
        public double dcf { get; set; }
        public required string image { get; set; }
        public required string ipoDate { get; set; }
        public bool defaultImage { get; set; }
        public bool isEtf { get; set; }
        public bool isActivelyTrading { get; set; }
        public bool isAdr { get; set; }
        public bool isFund { get; set; }
    }