using System;
using System.Collections.Generic;
using System.Linq;
using Behaviours.Player;
using ScriptableObjects.Clothing;
using UnityEngine;

namespace Abstractions
{
    public class DressableCharacter : MonoBehaviour
    {
        public bool isPlayer;
        public int money;
        public List<ClothingItem> Inventory
        {
            get;
            private set;
        }
    
        [Header("Equipped Clothing")]
        public ClothingEquipSlot hat;
        public ClothingEquipSlot face;
        public ClothingEquipSlot torso;
        public ClothingEquipSlot legs;
    
        [Header("Renderer References")]
        [SerializeField]private SpriteRenderer hatRend;
        [SerializeField]private SpriteRenderer faceRend;
        [SerializeField]private SpriteRenderer torsoRend;
        [SerializeField]private SpriteRenderer legsRend;

        [Header("AnimationController Reference")]
        [SerializeField] private DressableCharacterAnimationController animController;

        public Action<int> OnMoneyChanged;
        
        private void Awake()
        {
            Inventory = new List<ClothingItem>();
            
            //Equips all clothes that are already assigned via inspector.
            EquipClothing(hat.item, hat.equippedColor);
            EquipClothing(face.item, face.equippedColor);
            EquipClothing(torso.item, torso.equippedColor);
            EquipClothing(legs.item, legs.equippedColor);
        }

        public void PurchaseClothing(ClothingItem item, int colorIndex)
        {
            if (item == null) return;
        
            ClothingItem itemAlreadyBought = Inventory.FirstOrDefault(i => i.id == item.id);
            if (itemAlreadyBought == null)
            {
                item.purchasedColors = new List<int>();
                Inventory.Add(item);
                itemAlreadyBought = item;
            }
        
            if(!itemAlreadyBought.purchasedColors.Contains(colorIndex)) 
                itemAlreadyBought.purchasedColors.Add(colorIndex);

            OnMoneyChanged?.Invoke(-item.price);
            money -= item.price;
        
            //Equip instantly
            EquipClothing(item, colorIndex);
        }
        
        public bool IsItemAlreadyPurchased(ClothingItem item, int colorIndex)
        {
            ClothingItem itemAlreadyBought = Inventory.FirstOrDefault(i => i.id == item.id);
            return itemAlreadyBought != null && itemAlreadyBought.purchasedColors.Contains(colorIndex);
        }

        public void SellClothing(ClothingItem item, int colorIndex)
        {
            if (item == null) return;
        
            ClothingItem sellingItem = Inventory.FirstOrDefault(i => i.id == item.id);
            if (sellingItem == null)
            {
                Debug.Log("Attempting to sell an item that doesn't belong to the player. This should never happen.");
                return;
            }

            if (!sellingItem.purchasedColors.Contains(colorIndex))
            {
                Debug.Log("Attempting to sell an item color that doesn't belong to the player. This should never happen.");
                return;
            } 
            
            sellingItem.purchasedColors.Remove(colorIndex);
            
            if (sellingItem.purchasedColors.Count <= 0)
            {
                Inventory.Remove(sellingItem);
                
                //Unequips entire clothing if equipped
                switch (sellingItem.category)
                {
                    case ClothingCategory.Hat:
                        if(hat.item == sellingItem) UnequipClothing(ClothingCategory.Hat);
                        break;
                    case ClothingCategory.Face:
                        if(face.item == sellingItem) UnequipClothing(ClothingCategory.Face);
                        break;
                    case ClothingCategory.Torso:
                        if(torso.item == sellingItem) UnequipClothing(ClothingCategory.Torso);
                        break;
                    case ClothingCategory.Legs:
                        if(legs.item == sellingItem) UnequipClothing(ClothingCategory.Legs);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(item.category), item.category, "Invalid item category.");
                }
            }
            else
            {
                //Only unequips the color
                switch (sellingItem.category)
                {
                    case ClothingCategory.Hat:
                        if(hat.equippedColor == colorIndex) SetColor(ClothingCategory.Hat, sellingItem.purchasedColors[0]);
                        break;
                    case ClothingCategory.Face:
                        if(face.equippedColor == colorIndex) SetColor(ClothingCategory.Face, sellingItem.purchasedColors[0]);
                        break;
                    case ClothingCategory.Torso:
                        if(torso.equippedColor == colorIndex) SetColor(ClothingCategory.Torso, sellingItem.purchasedColors[0]);
                        break;
                    case ClothingCategory.Legs:
                        if(legs.equippedColor == colorIndex) SetColor(ClothingCategory.Legs, sellingItem.purchasedColors[0]);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(item.category), item.category, "Invalid item category.");
                }
            }

            OnMoneyChanged?.Invoke(item.resellPrice);
            money += item.resellPrice;
        }

        public void EarnMoney(int val)
        {
            OnMoneyChanged?.Invoke(val);
            money += val;
        }
        
        public void EquipClothing(ClothingItem item, int colorIndex)
        {
            if(item == null) return;
            
            //Equip instantly
            switch (item.category)
            {
                case ClothingCategory.Hat:
                    hat.item = item;
                    break;
                case ClothingCategory.Face:
                    face.item = item;
                    break;
                case ClothingCategory.Torso:
                    torso.item = item;
                    break;
                case ClothingCategory.Legs:
                    legs.item = item;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(item.category), item.category, "Invalid item category.");
            }
        
            SetColor(item.category, colorIndex);
        }

        private void UnequipClothing(ClothingCategory category)
        {
            switch (category)
            {
                case ClothingCategory.Hat:
                    hat.item = null;
                    break;
                case ClothingCategory.Face:
                    face.item  = null;
                    break;
                case ClothingCategory.Torso:
                    torso.item  = null;
                    break;
                case ClothingCategory.Legs:
                    legs.item  = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(category), category, "Invalid item category.");
            }
        }
    
        private void SetColor(ClothingCategory cat, int colorIndex)
        {
            switch (cat)
            {
                case ClothingCategory.Hat:
                    hatRend.color = hat.item.colors[colorIndex];
                    hat.equippedColor = colorIndex;
                    break;
                case ClothingCategory.Face:
                    faceRend.color = face.item.colors[colorIndex];
                    face.equippedColor = colorIndex;
                    break;
                case ClothingCategory.Torso:
                    torsoRend.color = torso.item.colors[colorIndex];
                    torso.equippedColor = colorIndex;
                    break;
                case ClothingCategory.Legs:
                    legsRend.color = legs.item.colors[colorIndex];
                    legs.equippedColor = colorIndex;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(cat), cat, "Invalid clothing slot.");
            }
        }
    
        public void SetAnimFrame(int frame)
        {
            Animations anim = animController.characterMov.moving ? Animations.Walk : Animations.Idle; //In a more robust setting, this would be replaced with a proper state machine.
            Directions dir = animController.lastDir;
        
            SetClothingFrame(ClothingCategory.Hat, anim, dir, frame);
            SetClothingFrame(ClothingCategory.Face, anim, dir, frame);
            SetClothingFrame(ClothingCategory.Torso, anim, dir, frame);
            SetClothingFrame(ClothingCategory.Legs, anim, dir, frame);
        }
    
        private void SetClothingFrame(ClothingCategory category, Animations anim, Directions dir, int frame)
        {
            SpriteRenderer rend;
            ClothingItem item;
        
            switch (category)
            {
                case ClothingCategory.Hat:
                    rend = hatRend;
                    item = hat.item;
                    break;
                case ClothingCategory.Face:
                    rend = faceRend;
                    item = face.item;
                    break;
                case ClothingCategory.Torso:
                    rend = torsoRend;
                    item = torso.item;
                    break;
                case ClothingCategory.Legs:
                    rend = legsRend;
                    item = legs.item;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(category), category, "Invalid clothing slot.");
            }

            if (rend == null) return;
            rend.gameObject.SetActive(item != null);
            
            if(item == null) return;
            rend.sprite = anim switch
            {
                Animations.Idle => item.idleFrames[(int)dir].frames[frame],
                Animations.Walk => item.walkFrames[(int)dir].frames[frame],
                _ => throw new ArgumentOutOfRangeException(nameof(anim), anim, "Invalid animation")
            };
        }
    }
}
