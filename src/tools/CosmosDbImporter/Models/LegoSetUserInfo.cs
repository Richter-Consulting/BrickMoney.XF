using System;
using System.Collections.Generic;
using System.Text;

namespace BrickMoney.DB.Models
{
    public class LegoSetUserInfo
    {
        public int Id { get; set; }
        public int State { get; set; }
        public bool IsSelected { get; set; }
        public int LegoSetId { get; set; }
        //public int? LegoSetId { get; set; } Wenn LegoSetId nicht vorhanden -> null
        public LegoSetBasicInfo LegoSetBasic { get; set; }
        public double PurchasingPrice { get; set; }
        public string Seller { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double RetailPrice { get; set; }
        public DateTime SaleDate { get; set; }
        public string SoldOver { get; set; }
        public string Buyer { get; set; }

        public LegoSetUserInfo(int state, bool isSelected, int legoSetId, double purchasingPrice, string seller,
            DateTime purchaseDate, double retailPrice, DateTime saleDate, string soldOver, string buyer)
        {
            State = state;
            IsSelected = isSelected;
            LegoSetId = legoSetId;
            PurchasingPrice = purchasingPrice;
            Seller = seller;
            PurchaseDate = purchaseDate;
            RetailPrice = retailPrice;
            SaleDate = saleDate;
            SoldOver = soldOver;
            Buyer = buyer;

        }
    }
}
