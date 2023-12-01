using System;
using System.Collections.Generic;
using System.Linq;
using Behaviours.Player;
using ScriptableObjects.Clothing;
using TMPro;
using UI.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Abstractions
{
    public abstract class InventoryManagerBase : MonoBehaviour
    {
        protected DressableCharacter dressableChar;
                
        [SerializeField]
        protected Transform grid;
        
        [SerializeField]
        protected ItemThumbnail thumbnailPrefab;
        
        [Header("Labels")]
        [SerializeField]
        protected TMP_Text itemNameLabel;
        [SerializeField]
        protected TMP_Text itemDescLabel;
        
        [Header("Reference to Full-body art layers")]
        public Image hatLayer;
        public Image faceLayer;
        public Image torsoLayer;
        public Image legsLayer;
        
        protected List<ClothingItem> inventory;
        
        [Header("Reference to Player Movement")]
        public CharacterMovement playerMov;
        
        public Action<ClothingItem, int> OnItemClicked { get; set; }

        protected ClothingItem selectedItem = null;
        protected int selectedColor = 0;

        protected virtual void Awake()
        {
            dressableChar = FindObjectsOfType<DressableCharacter>().FirstOrDefault(d => d.isPlayer);

            if (dressableChar == null)
            {
                Debug.LogError(
                    "Couldn't find a Player-controlled DressableCharacter script. Make sure to mark one as IsPlayer.");
                return;
            }
            
            ShowEquippedItems();

            itemNameLabel.text = string.Empty;
            itemDescLabel.text = string.Empty;
        }

        private void OnEnable()
        {
            playerMov.canMove = false;
        }

        private void OnDisable()
        {
            playerMov.canMove = true;
        }

        protected void SetupGrid(DressableCharacter dressableChar = null)
        {
            if(inventory == null) return;
            
            inventory = inventory.OrderBy(i => i.id).ToList();
            
            foreach (ClothingItem item in inventory)
            {
                ItemThumbnail thumb = Instantiate(thumbnailPrefab, grid);
                thumb.Setup(item, this, dressableChar != null ? item.purchasedColors : null);
            }

            OnItemClicked += SelectItem;
            OnItemClicked += SetFullBodyArt;
            OnItemClicked += SetText;
        }

        private void OnDestroy()
        {
            OnItemClicked -= SelectItem;
            OnItemClicked -= SetFullBodyArt;
            OnItemClicked -= SetText;
        }

        protected abstract void SelectItem(ClothingItem item, int colorID);

        private void SetFullBodyArt(ClothingItem item, int colorID)
        {
            if(item == null) return;
            
            Image targetImg = item.category switch
            {
                ClothingCategory.Hat => hatLayer,
                ClothingCategory.Face => faceLayer,
                ClothingCategory.Torso => torsoLayer,
                ClothingCategory.Legs => legsLayer,
                _ => throw new ArgumentOutOfRangeException(nameof(item.category), item.category,
                    "Invalid item category.")
            };

            targetImg.sprite = item.fullBodyArt;
            targetImg.gameObject.SetActive(targetImg.sprite != null);
            targetImg.color = item.colors[colorID];
        }

        private void SetText(ClothingItem item, int colorID)
        {
            itemNameLabel.text = item.title;
            itemDescLabel.text = item.desc;
        }

        private void ShowEquippedItems()
        {
            SetFullBodyArt(dressableChar.hat.item, dressableChar.hat.equippedColor);
            SetFullBodyArt(dressableChar.face.item, dressableChar.face.equippedColor);
            SetFullBodyArt(dressableChar.torso.item, dressableChar.torso.equippedColor);
            SetFullBodyArt(dressableChar.legs.item, dressableChar.legs.equippedColor);
        }
    }
}
