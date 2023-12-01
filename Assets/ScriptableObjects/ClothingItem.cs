using System;
using System.Collections.Generic;
using Abstractions;
using UnityEngine;

namespace ScriptableObjects.Clothing
{
    [CreateAssetMenu(fileName = "NewClothingItem", menuName = "Blue Gravity/New Clothing Item...")]
    public class ClothingItem : ScriptableObject
    {
        public int id;
        public string title;
        public string desc;
        public ClothingCategory category;
        public int price;
        public Sprite thumbnailIcon;
        public Sprite fullBodyArt;
        public List<Color> colors;

        [HideInInspector]
        public List<int> purchasedColors;
        
        public List<ClothingFramesEntry> idleFrames;
        public List<ClothingFramesEntry> walkFrames;
    }
}
