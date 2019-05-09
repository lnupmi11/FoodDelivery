using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDelivery.DTO
{
    public class FilterMenuItem
    {
        public int Page { get; set; }
        public string SearchWord { get; set; }
        public string FilterOpt { get; set; }
        public string CategoryId { get; set; }
        public int ItemPerPage { get; set; }
    }
}
