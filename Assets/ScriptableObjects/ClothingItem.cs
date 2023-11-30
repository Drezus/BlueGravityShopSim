using System;
using System.Collections.Generic;
using Abstractions;
using UnityEngine;

namespace ScriptableObjects.Clothing
{
    [CreateAssetMenu(fileName = "NewClothingItem", menuName = "Blue Gravity/New Clothing Item...")]
    public class ClothingItem : ScriptableObject
    {
        public string title;
        public string desc;
        public ClothingCategory category;
        public Sprite icon;
        public List<Color> colors;

        [HideInInspector]
        public List<int> purchasedColors;
        
        public List<ClothingFramesEntry> idleFrames;
        public List<ClothingFramesEntry> walkFrames;
    }
}
