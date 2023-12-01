using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behaviours.Player;
using ScriptableObjects.Clothing;
using UnityEngine;

public class DressableCharacter : MonoBehaviour
{
    private List<ClothingItem> inventory;
    
    [Header("Equipped Clothing")]
    public ClothingItem hat;
    public ClothingItem face;
    public ClothingItem torso;
    public ClothingItem legs;
    public ClothingItem feet;
    
    [Header("Renderer References")]
    [SerializeField]private SpriteRenderer hatRend;
    [SerializeField]private SpriteRenderer faceRend;
    [SerializeField]private SpriteRenderer torsoRend;
    [SerializeField]private SpriteRenderer legsRend;
    [SerializeField]private SpriteRenderer feetRend;

    [Header("AnimationController Reference")]
    [SerializeField] private PlayerAnimationController animController;

    private void Awake()
    {
        SetColor(ClothingCategory.Torso, 0);
    }

    public void PurchaseClothing(ClothingItem item, int colorIndex)
    {
        ClothingItem itemAlreadyBought = inventory.FirstOrDefault(i => i.id == item.id);
        if (itemAlreadyBought == null)
        {
            inventory.Add(item);
            itemAlreadyBought = item;
        }
        
        if(!itemAlreadyBought.purchasedColors.Contains(colorIndex)) 
            itemAlreadyBought.purchasedColors.Add(colorIndex);
        
        //Equip instantly
        switch (item.category)
        {
            case ClothingCategory.Hat:
                hat = item;
                break;
            case ClothingCategory.Face:
                face = item;
                break;
            case ClothingCategory.Torso:
                torso = item;
                break;
            case ClothingCategory.Legs:
                legs = item;
                break;
            case ClothingCategory.Feet:
                feet = item;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(item.category), item.category, "Invalid item category.");
        }
        
        SetColor(item.category, colorIndex);
    }
    
    private void SetColor(ClothingCategory cat, int colorIndex)
    {
        switch (cat)
        {
            case ClothingCategory.Hat:
                hatRend.color = hat.colors[colorIndex];
                break;
            case ClothingCategory.Face:
                faceRend.color = face.colors[colorIndex];
                break;
            case ClothingCategory.Torso:
                torsoRend.color = torso.colors[colorIndex];
                break;
            case ClothingCategory.Legs:
                legsRend.color = legs.colors[colorIndex];
                break;
            case ClothingCategory.Feet:
                feetRend.color = feet.colors[colorIndex];
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(cat), cat, "Invalid clothing slot.");
        }
    }
    
    public void SetAnimFrame(int frame)
    {
        Animations anim = animController.playerMov.moving ? Animations.Walk : Animations.Idle; //In a more robust setting, this would be replaced with a proper state machine.
        Directions dir = animController.lastDir;
        
        SetClothingFrame(ClothingCategory.Hat, anim, dir, frame);
        SetClothingFrame(ClothingCategory.Face, anim, dir, frame);
        SetClothingFrame(ClothingCategory.Torso, anim, dir, frame);
        SetClothingFrame(ClothingCategory.Legs, anim, dir, frame);
        SetClothingFrame(ClothingCategory.Feet, anim, dir, frame);
    }
    
    private void SetClothingFrame(ClothingCategory category, Animations anim, Directions dir, int frame)
    {
        SpriteRenderer rend;
        ClothingItem item;
        
        switch (category)
        {
            case ClothingCategory.Hat:
                rend = hatRend;
                item = hat;
                break;
            case ClothingCategory.Face:
                rend = faceRend;
                item = face;
                break;
            case ClothingCategory.Torso:
                rend = torsoRend;
                item = torso;
                break;
            case ClothingCategory.Legs:
                rend = legsRend;
                item = legs;
                break;
            case ClothingCategory.Feet:
                rend = feetRend;
                item = feet;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(category), category, "Invalid clothing slot.");
        }
        
        
        if(rend == null || item == null) return;

        switch (anim)
        {
            case Animations.Idle:
                rend.sprite = item.idleFrames[(int)dir].frames[frame];
                break;
            case Animations.Walk:
                rend.sprite = item.walkFrames[(int)dir].frames[frame];
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(anim), anim, "Invalid animation");
        }

    }
}
