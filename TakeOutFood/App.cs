namespace TakeOutFood
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;

    public class App
    {
        private IItemRepository itemRepository;
        private ISalesPromotionRepository salesPromotionRepository;
        public App(IItemRepository itemRepository, ISalesPromotionRepository salesPromotionRepository)
        {
            this.itemRepository = itemRepository;
            this.salesPromotionRepository = salesPromotionRepository;
        }
        private List<ItemWithCount> GetSelectedItems (List<string> inputs)
        {
            List<Item> foodDataBase = itemRepository.FindAll();
            List<ItemWithCount> selectedItems = new List<ItemWithCount>();
            foreach (var input in inputs)
            {
                var itemID = input.Substring(0, 8);
                foreach (var food in foodDataBase)
                {
                    if (itemID == food.Id)
                    {
                        int count = int.Parse(input.Substring(input.IndexOf("x") + 1));
                        selectedItems.Add(new ItemWithCount(food.Id, food.Name, food.Price, count));
                    }
                }
            }
            return selectedItems;
        }
        private List<FeasiblePromotion> GetPromotionScenarios (List<ItemWithCount> selectedItems)
        {
            List<SalesPromotion> promotionDataBase = salesPromotionRepository.FindAll();
            List<FeasiblePromotion> possiblePromotions = new List<FeasiblePromotion>();
            List<string> inputItems = new List<string>();
            foreach (var item in selectedItems)
            {
                inputItems.Add(item.Id);
            }
            foreach (var promotion in promotionDataBase)
            {
                if (promotion.RelatedItems.Intersect(inputItems).Count() != 0)
                {
                    FeasiblePromotion temp = new FeasiblePromotion();
                    temp.DiscountedIds = promotion.RelatedItems.Intersect(inputItems);
                    var discountItems = new List<ItemWithCount>();
                    foreach (var id in temp.DiscountedIds)
                    {
                        foreach (var j in selectedItems)
                        {
                            if (id == j.Id)
                            {
                                
                                discountItems.Add(j);
                            }
                        }
                    }
                    temp.DiscountedItems = discountItems;
                    temp.DisplayName = promotion.DisplayName;
                    temp.Discount = Convert.ToDouble(promotion.Type.Substring(0, 2)) / 100;
                    possiblePromotions.Add(temp);
                }
            }
            return possiblePromotions;
        }
        public string BestCharge(List<string> inputs)
        {
            var selectedItems = GetSelectedItems(inputs);
            var promotionScenarios = GetPromotionScenarios(selectedItems);
            if (promotionScenarios.Count() == 0)
            {
                StringBuilder output = new StringBuilder();
                output.Append("============= Order details =============\n");
                double totalCharge = 0;
                foreach(var selectedItem in selectedItems)
                {
                    totalCharge += selectedItem.ItemCharge;
                    output.Append(selectedItem.Name + " x " + selectedItem.Count.ToString() + " = " + selectedItem.ItemCharge.ToString() + " yuan\n");
                }
                output.Append("-----------------------------------\n");
                output.Append("Total：" + totalCharge.ToString() + " yuan\n");
                output.Append("===================================");
                return output.ToString();
            }
            else
            {
                var bestPromotion = new FeasiblePromotion();
                double bestMoneySaved = 0;
                foreach (var promotionScenario in promotionScenarios)
                {
                    double moneySaved = 0;
                    foreach (var discountedItem in promotionScenario.DiscountedItems)
                    {
                        moneySaved += discountedItem.ItemCharge * (1 - promotionScenario.Discount);
                    }
                    if (moneySaved > bestMoneySaved)
                    {
                        bestMoneySaved = moneySaved;
                        bestPromotion = promotionScenario;
                    }
                }
                StringBuilder output = new StringBuilder();
                output.Append("============= Order details =============\n");
                double totalCharge = 0;
                foreach (var selectedItem in selectedItems)
                {
                    totalCharge += selectedItem.ItemCharge;
                    output.Append(selectedItem.Name + " x " + selectedItem.Count.ToString() + " = " + selectedItem.ItemCharge.ToString() + " yuan\n");
                }
                StringBuilder discountDishes = new StringBuilder();
                discountDishes.Append(" (" + bestPromotion.DiscountedItems[0].Name);
                var itemCount = bestPromotion.DiscountedItems.Count();
                for (var i = 1; i < itemCount - 1; i++)
                {
                    discountDishes.Append(", " + bestPromotion.DiscountedItems[i].Name);
                }
                discountDishes.Append(", " + bestPromotion.DiscountedItems[itemCount-1].Name + "), ");
                output.Append("-----------------------------------\n");
                output.Append("Promotion used:\n");
                output.Append(bestPromotion.DisplayName + discountDishes.ToString() + "saving " + bestMoneySaved.ToString() + " yuan\n");
                output.Append("-----------------------------------\n");
                output.Append("Total：" + (totalCharge - bestMoneySaved).ToString() + " yuan\n");
                output.Append("===================================");
                return output.ToString();
            }
        }
    }
}
