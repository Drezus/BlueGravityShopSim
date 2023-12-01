
using System.Collections.Generic;
using Abstractions;
using ScriptableObjects.Clothing;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    [RequireComponent(typeof(Button))]
    public class ItemThumbnail : MonoBehaviour
    {
        private ClothingItem thisItem;
        private Button btn;

        [SerializeField]
        private Image itemIcon;
        
        [SerializeField]
        private Button itemButton;
        
        [SerializeField]
        private Button colorButtonPrefab;
        
        [SerializeField]
        private Transform colorButtonsParent;

        private int currentColor = 0;

        private void Awake()
        {
            btn = GetComponent<Button>();
        }

        public void Setup(ClothingItem item, InventoryManagerBase inventory, List<int> purchasedColors = null)
        {
            thisItem = item;
            itemIcon.sprite = item.thumbnailIcon;
            
            if (colorButtonsParent.childCount > 0)
            {
                foreach (Transform t in colorButtonsParent.transform.GetComponentsInChildren<Transform>())
                {
                    if (t == colorButtonsParent) continue;
                    Destroy(t.gameObject);
                }    
            }

            foreach (Color col in thisItem.colors)
            {
                if (purchasedColors != null && !purchasedColors.Contains(thisItem.colors.IndexOf(col))) continue;
                
                Button colorBtn = Instantiate(colorButtonPrefab, colorButtonsParent);
                colorBtn.GetComponent<Image>().color = col;
                colorBtn.GetComponent<Button>().onClick.AddListener(() =>
                {
                    currentColor = thisItem.colors.IndexOf(col);
                    inventory.OnItemClicked?.Invoke(thisItem, currentColor);
                    RecolorIcon();
                });
            }
            
            currentColor = purchasedColors != null ? purchasedColors[0] : 0;
            RecolorIcon();
            
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => inventory.OnItemClicked?.Invoke(thisItem, currentColor));
        }

        private void RecolorIcon()
        {
            itemIcon.color = thisItem.colors[currentColor];
        }
    }
}