﻿using System;

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
    }
}
