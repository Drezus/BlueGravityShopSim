using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects.Clothing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop
{
    public class ShopManager : MonoBehaviour
    {
        [Header("Reference to Player Inventory")]
        public DressableCharacter dressable;
        
        [SerializeField]
        private List<ClothingItem> catalog;

        [SerializeField]
        private ItemThumbnail thumbnailPrefab;
        
        [SerializeField]
        private Transform grid;

        [Header("Buy Button customizations")]
        public Button buyBtn;
        public Color buyableColor, noFundsColor, soldOutColor;
        public TMP_Text buyBtnTxt;
        
        public Action<ClothingItem, int> OnItemClicked { get; set; }

        private ClothingItem selectedItem = null;
        private int selectedColor = 0;

        private void Awake()
        {
            buyBtn.gameObject.SetActive(false);
            catalog = catalog.OrderBy(i => i.id).ToList();
            
            foreach (ClothingItem item in catalog)
            {
                ItemThumbnail thumb = Instantiate(thumbnailPrefab, grid);
                thumb.Setup(item, this);
            }

            OnItemClicked += SelectItem;
        }

        private void OnDestroy()
        {
            OnItemClicked -= SelectItem;
        }

        private void SelectItem(ClothingItem item, int colorID)
        {
            if(!buyBtn.gameObject.activeSelf) buyBtn.gameObject.SetActive(true);
            
            selectedItem = item;
            selectedColor = colorID;

            bool alreadyPurchased = dressable.AlreadyPurchasedItem(item, colorID);
            
            if (alreadyPurchased)
            {
                buyBtn.interactable = false;
                buyBtn.GetComponent<Image>().color = soldOutColor;
                buyBtnTxt.text = "SOLD OUT";
            }
            else
            {
                buyBtn.interactable = dressable.coins >= item.price;
                buyBtnTxt.text = $"BUY (${item.price})";
                buyBtn.GetComponent<Image>().color = dressable.coins >= item.price ? buyableColor : noFundsColor;
            }
        }

        public void BuySelectedItem()
        {
            dressable.PurchaseClothing(selectedItem, selectedColor);
            SelectItem(selectedItem, selectedColor);
        }
    }
}
