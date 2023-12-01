using System;
using ScriptableObjects.Clothing;

namespace Abstractions
{
    [Serializable]
    public struct ClothingEquipSlot
    {
        public ClothingItem item;
        public int equippedColor;
    }
}