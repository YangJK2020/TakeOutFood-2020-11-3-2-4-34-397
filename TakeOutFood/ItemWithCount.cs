using System;
using System.Collections.Generic;
using System.Text;

namespace TakeOutFood
{
    class ItemWithCount : Item
    {
       // private double _itemCharge;
        public int Count { get; private set; }
        public ItemWithCount(string id, string name, double price, int count) : base(id, name, price)
        {
            this.Count = count;
        }
        public double ItemCharge 
        {
            //set { _itemCharge = value; }
            get { return Count * Price; }
        }
    }
}
