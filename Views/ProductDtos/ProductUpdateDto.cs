using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Zembil.Views
{
    public class ProductUpdateDto
    {
        private static CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
        private static TextInfo textInfo = cultureInfo.TextInfo;
        private string _condition;        
        public string ProductName { get; set; }
        public string Description { get; set; }        
        public int CategoryId { get; set; }
        public string Brand { get; set; }
        public int Price { get; set; }
        //used or new validator
        public string Condition
        {
            get { return _condition; }
            set
            {
                _condition = textInfo.ToTitleCase(value.ToLower());
            }
        }
        public string ImageUrl { get; set; }
        public bool DeliveryAvailable { get; set; }
        public int ProductCount { get; set; }
    }
}
