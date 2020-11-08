using System;
using System.Collections.Generic;
using System.Text;

namespace TakeOutFood
{
    class FeasiblePromotion
    {
        public double Discount { get; set; }
        public string DisplayName { get; set; }
        public IEnumerable<string> DiscountedIds { get; set; }
        public List<ItemWithCount> DiscountedItems { get; set; }
    }
}
