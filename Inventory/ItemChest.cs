using UnityEngine;
using UnityEngine.UI;
using VRTK.Controllables;

namespace SoulDrop
{
    public class ItemChest : MonoBehaviour
    {
        [SerializeField]
        Item item;
        // Holds amount of items in the chest.
        [SerializeField]
        int amount = 1;
        [SerializeField]
        Inventory inventory;
        [SerializeField]
        SpriteRenderer spriteRenderer;
        [SerializeField]
        Color emptyColor;

        private bool isEmpty;

        // VRTK Physics Chest example.
        public VRTK_BaseControllable controllable;
        public Text displayText;
        public string outputOnMax = "Maximum Reached";
        public string outputOnMin = "Minimum Reached";

        private void OnValidate()
        {
            if (inventory == null)
            {
                inventory = FindObjectOfType<Inventory>();
            }

            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }

            spriteRenderer.sprite = item.Icon;
            spriteRenderer.enabled = false;

            if (item != null)
            {
                isEmpty = false;
            }
        }

        protected virtual void OnEnable()
        {
            controllable = (controllable == null ? GetComponent<VRTK_BaseControllable>() : controllable);
            controllable.ValueChanged += ValueChanged;
            controllable.MaxLimitReached += MaxLimitReached;
            controllable.MinLimitReached += MinLimitReached;
        }

        protected virtual void ValueChanged(object sender, ControllableEventArgs e)
        {
            if (displayText != null)
            {
                displayText.text = e.value.ToString("F1");
            }
        }

        protected virtual void MaxLimitReached(object sender, ControllableEventArgs e)
        {
            if (outputOnMax != "")
            {
                Debug.Log(outputOnMax);

                // Add the item when the chest is fully opened.
                if (!isEmpty)
                {
                    Item itemCopy = item.GetCopy();

                    while (amount != 0)
                    {
                        if (inventory.AddItem(itemCopy))
                        {
                            amount--;
                            if (amount == 0)
                            {
                                isEmpty = true;
                                // Faded color.
                                spriteRenderer.color = emptyColor;
                                spriteRenderer.enabled = true;
                            }
                        }
                        else
                        {
                            itemCopy.Destroy();
                            break;
                        }
                    }
                }
            }
        }

        protected virtual void MinLimitReached(object sender, ControllableEventArgs e)
        {
            if (outputOnMin != "")
            {
                Debug.Log(outputOnMin);

                // No sprite is viewable for the environment.
                spriteRenderer.enabled = false;
            }
        }
    }
}