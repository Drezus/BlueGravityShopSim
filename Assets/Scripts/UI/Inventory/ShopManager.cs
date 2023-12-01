using System.Collections.Generic;
using System.Linq;
using Abstractions;
using ScriptableObjects.Clothing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class ShopManager : InventoryManagerBase
    {
        [Header("Buy Button customizations")]
        public Button buyBtn;
        public Color buyableColor, noFundsColor, soldOutColor;
        public TMP_Text buyBtnTxt;

        protected override void Awake()
        {
            base.Awake();
            inventory = Resources.LoadAll<ClothingItem>("Items").ToList();
            SetupGrid();
            buyBtn.gameObject.SetActive(false);
        }

        protected override void SelectItem(ClothingItem item, int colorID)
        {
            if(!buyBtn.gameObject.activeSelf) buyBtn.gameObject.SetActive(true);
            
            selectedItem = item;
            selectedColor = colorID;

            bool alreadyPurchased = dressableChar.IsItemAlreadyPurchased(item, colorID);
            
            if (alreadyPurchased)
            {
                buyBtn.interactable = false;
                buyBtn.GetComponent<Image>().color = soldOutColor;
                buyBtnTxt.text = "SOLD OUT";
            }
            else
            {
                buyBtn.interactable = dressableChar.coins >= item.price;
                buyBtnTxt.text = $"BUY (${item.price})";
                buyBtn.GetComponent<Image>().color = dressableChar.coins >= item.price ? buyableColor : noFundsColor;
            }
        }

        public void BuySelectedItem()
        {
            dressableChar.PurchaseClothing(selectedItem, selectedColor);
            SelectItem(selectedItem, selectedColor);
        }
    }
}
