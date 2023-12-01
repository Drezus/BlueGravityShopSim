using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Clothing;
using UnityEngine;

namespace UI.Shop
{
    public class ShopManager : MonoBehaviour
    {
        [SerializeField]
        private List<ClothingItem> catalog;

        [SerializeField]
        private ItemThumbnail thumbnailPrefab;
        
        [SerializeField]
        private Transform grid;
        
        public Action<ClothingItem, int> OnItemClicked { get; set; }

        private void Awake()
        {
            foreach (ClothingItem item in catalog)
            {
                ItemThumbnail thumb = Instantiate(thumbnailPrefab, grid);
                thumb.Setup(item, this);
            }
        }
    }
}
