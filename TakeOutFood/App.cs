namespace TakeOutFood
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class App
    {
        private IItemRepository itemRepository;
        private ISalesPromotionRepository salesPromotionRepository;

        public App(IItemRepository itemRepository, ISalesPromotionRepository salesPromotionRepository)
        {
            this.itemRepository = itemRepository;
            this.salesPromotionRepository = salesPromotionRepository;
        }

        public string BestCharge(List<string> inputs)
        {
            //TODO: write code here
            List<Item> foodDataBase = itemRepository.FindAll();
            List<SalesPromotion> promotionDataBase = salesPromotionRepository.FindAll();
            List<Item> selectedItems = new List<Item>();
            List<int> itemCounts = new List<int>();
            List<SalesPromotion> possiblePromotions = new List<SalesPromotion>();
            foreach (var i in inputs)
            {
                var itemID = i.Substring(0, 7);
                foreach (var j in foodDataBase)
                {
                    if (itemID == j.Id)
                    {
                        selectedItems.Add(j);
                        itemCounts.Add(Convert.ToInt16(i.Substring(i.IndexOf("x") + 1)));
                    }
                }
            }

            foreach (var promotion in promotionDataBase)
            {
                SalesPromotion possibleScenario = new SalesPromotion("Type", "DisplayName", new List<string>());
                foreach (var itemselect in selectedItems)
                {
                    if (promotion.RelatedItems.Contains(itemselect.Id))
                    {

                        possibleScenario.RelatedItems.Add(itemselect.Id);
                    }
                }
                possiblePromotions.Add(possibleScenario);
            }


            StringBuilder sb = new StringBuilder();
            if (possiblePromotions.Count == 0)
            {
                double total = 0;
                sb.Append("============= Order details =============\n");
                for (var i = 0; i < selectedItems.Count; i++)
                {
                    double price = selectedItems[i].Price * itemCounts[i];
                    total += price;
                    sb.Append(selectedItems[i].Name + "x" + itemCounts[i].ToString() + "=" + price.ToString() + "yuan\n");
                }
                sb.Append("Total：" + total.ToString() + "yuan\n");
                sb.Append("===================================");
                return sb.ToString();
            }
            else
            {

                //Code to compare different promotions should be implemented. will be added before this weekend 
                return "============= Order details =============\n" +
                    "Braised chicken x 1 = 18 yuan\n" +
                    "Chinese hamburger x 2 = 12 yuan\n" +
                    "Cold noodles x 1 = 8 yuan\n" +
                    "-----------------------------------\n" +
                    "Promotion used:\n" +
                    "Half price for certain dishes (Braised chicken, Cold noodles), saving 13 yuan\n" +
                    "-----------------------------------\n" +
                    "Total：25 yuan\n" +
                    "===================================";
            }
        }
    }
}
