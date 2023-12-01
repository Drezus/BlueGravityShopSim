using System;
using ScriptableObjects.Clothing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop
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

        public void Setup(ClothingItem item, ShopManager manager)
        {
            itemIcon.sprite = item.icon;
            currentColor = 0;
            
            if (colorButtonsParent.childCount > 0)
            {
                foreach (Transform t in colorButtonsParent.transform.GetComponentsInChildren<Transform>())
                {
                    if (t == colorButtonsParent) continue;
                    Destroy(t.gameObject);
                }    
            }

            foreach (Color col in item.colors)
            {
                Button colorBtn = Instantiate(colorButtonPrefab, colorButtonsParent);
                colorBtn.GetComponent<Image>().color = col;
                colorBtn.GetComponent<Button>().onClick.AddListener(() =>
                {
                    currentColor = item.colors.IndexOf(col);
                    manager.OnItemClicked?.Invoke(item, currentColor);
                });
            }
            
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => manager.OnItemClicked?.Invoke(item, currentColor));
        }
    }
}