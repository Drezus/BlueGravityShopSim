using System;
using System.Collections.Generic;
using System.Linq;
using Abstractions;
using ScriptableObjects.Clothing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class ShopSellManager : InventoryManagerBase
    {
        [Header("Sell Button customizations")]
        public Button sellBtn;
        public TMP_Text sellBtnTxt;

        protected override void OnEnable()
        {
            base.OnEnable();
            inventory = dressableChar.Inventory;
            SetupGrid(dressableChar);
            sellBtn.gameObject.SetActive(false);
        }

        protected override void SelectItem(ClothingItem item, int colorID)
        {
            sellBtn.gameObject.SetActive(true);
            
            selectedItem = item;
            selectedColor = colorID;
            
            sellBtnTxt.text = $"SELL (${item.resellPrice})";
        }

        public void SellSelectedItem()
        {
            dressableChar.SellClothing(selectedItem, selectedColor);
            UnselectItem();
            sellBtn.gameObject.SetActive(false);
            
            inventory = dressableChar.Inventory;
            SetupGrid(dressableChar);
            UpdateEquippedArt();
        }
    }
}
