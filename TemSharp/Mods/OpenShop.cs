using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace TemSharp
{
    public class OpenShop : MonoBehaviour
    {
        void OnEnable()
        {
            var allItems = typeof(Temtem.Core.ConfigReader).GetField<Temtem.Core.ConfigReader>().GetField<Temtem.Inventory.AllItemDefinitions>();
            var items = new List<Temtem.Inventory.ItemDefinition>();

            // @TODO: Add list to GUI for configurable Shops
            var shops = new string[] {
                "generalItems",
                "captureItems",
                "medicineItems",
                "heldItems",
                "courseItems",
                "keyItems",
                "cosmeticItems",
                "emoteItems",
                "tintItems",
                "tintBundleItems",
                "stickerItems",
                "bundleItems",
                "hardcodedPromoCodeItems",
            };

            foreach (string shop in shops)
            {
                items.AddRange(allItems.GetField<List<Temtem.Inventory.ItemDefinition>>(shop));
            }

            var itemList = new Temtem.Inventory.BuyableItemList();
            itemList.ShopItemIds = items.FindAll(i => i != null && i.Price > 0).Select(i => i.Id).ToList();
            Temtem.UI.InGameShopBuyUI.nkqrjhelndm.nnkdhejnfdp(itemList, false);
            typeof(Temtem.UI.UIManager).GetField<Temtem.UI.UIManager>().hrfjkefrolc();
        }
    }
}