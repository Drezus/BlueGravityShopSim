using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abstractions;
using ScriptableObjects.Clothing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class PlayerInventoryManager : InventoryManagerBase
    {
        [Header("Equip Button customizations")]
        public Button equipBtn;
        public Color equippableColor, equippedColor;

        protected override void OnEnable()
        {
            base.OnEnable();
            if(dressableChar == null) return;
            
            inventory = dressableChar.Inventory;
            SetupGrid(dressableChar);
            equipBtn.gameObject.SetActive(false);
        }

        protected override void SelectItem(ClothingItem item, int colorID)
        {
            dressableChar.EquipClothing(item, colorID);
        }
    }
}
