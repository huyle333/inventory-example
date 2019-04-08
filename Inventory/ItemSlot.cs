using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace SoulDrop {
    public class ItemSlot : MonoBehaviour {

        [SerializeField]
        Image Image;
        [SerializeField]
        ItemTooltip tooltip;
        [SerializeField]
        Text amountText;

        public event Action<ItemSlot> OnPointerClickEvent;
        public event Action<ItemSlot> OnPointerEnterEvent;
        public event Action<ItemSlot> OnPointerExitEvent;
        public event Action<ItemSlot> OnBeginDragEvent;
        public event Action<ItemSlot> OnEndDragEvent;
        public event Action<ItemSlot> OnDragEvent;
        public event Action<ItemSlot> OnDropEvent;

        // On disable.
        protected bool isPointerOver;

        // Change the image to transparent when no item is in the slot.
        private Color normalColor = Color.white;
        private Color disabledColor = new Color(1, 1, 1, 0);

        private Item _item;
        // Usisng a property because Image will be loaded by OnValidate.
        // And then, Item will be set automatically.
        public Item Item
        {
            get { return _item; }
            set
            {
                _item = value;
                // Set amount to 0 when null.
                if (_item == null && Amount != 0)
                {
                    // Use the public property because we want the setter to run.
                    Amount = 0;
                }

                // Item is a class that contains the Sprite and Name,
                // We don't need the Image if the Item is not set, so just make the Image disabledColor (transparent).
                if (_item == null)
                {
                    // Image.color = disabledColor;
                    Image.enabled = false;
                }
                // But if the item has an Image, then, use it.
                else
                {
                    Image.sprite = _item.Icon;
                    // Image.color = normalColor;
                    Image.enabled = true;
                }

                // Tooltip reset.
                // A different way to retrigger the tooltip active box, but I don't know.
                /*
                if (isPointerOver)
                {
                    OnPointerExit(null);
                    OnPointerEnter(null);
                }
                */
            }
        }

        // Stacking items like if we had 15 gold.
        private int _amount;
        public int Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                _amount = value;
                if (_amount < 0)
                {
                    _amount = 0;
                }
                if (_amount == 0 && Item != null)
                {
                    Item = null;
                }

                // Check the stacks. > 1.
                amountText.enabled = _item != null && _item.MaximumStacks > 1 && _amount > 1;
                if (amountText.enabled)
                {
                    amountText.text = _amount.ToString();
                }
            }
        }

        #region INTIALIZATION
        /// <summary>
        /// The public Item Item is set up by Inventory.cs as it loops its list of Items.
        /// If Item Item is declared, then, it also setups up the Image.
        /// </summary>
        protected virtual void OnValidate()
        {
            if (Image == null)
            {
                Image = this.GetComponent<Image>();
            }

            if (amountText == null)
            {
                amountText = this.GetComponentInChildren<Text>();
            }
        }

        /// <summary>
        /// Originally, this was used to reset the position of the mouse if the menu button
        /// was selected.
        /// </summary>
        protected virtual void OnDisable()
        {
            // Canvas has been set to inactive.
            // Tool tip reset.
            /*
            if (isPointerOver)
            {
                OnPointerExit(null);
            }
            */
        }

        private void Start()
        {
            EventTrigger trigger = this.GetComponent<EventTrigger>();
            // We want to keep track of a Trigger click.
            Helper.AddEventTriggerListener(trigger, EventTriggerType.PointerClick, OnPointerClick);
            Helper.AddEventTriggerListener(trigger, EventTriggerType.PointerEnter, OnPointerEnter);

            // Draggables still need work.
            // Helper.AddEventTriggerListener(trigger, EventTriggerType.PointerExit, OnPointerExit);
            // Helper.AddEventTriggerListener(trigger, EventTriggerType.BeginDrag, OnBeginDrag);
            // Helper.AddEventTriggerListener(trigger, EventTriggerType.EndDrag, OnEndDrag);
            // Helper.AddEventTriggerListener(trigger, EventTriggerType.Drag, OnDrag);
            // Helper.AddEventTriggerListener(trigger, EventTriggerType.Drop, OnDrop);
        }
        #endregion

        #region TRIGGERS THAT INVOKE THE LISTENERS
        /// <summary>
        /// Clicked on an ItemSlot.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(BaseEventData eventData)
        {
            // Invoking this event for all listeners only if they exist.
            // The ? checks if the event is null or not.
            OnPointerClickEvent?.Invoke(this);
        }

        public void OnPointerEnter(BaseEventData eventData)
        {
            isPointerOver = true;

            OnPointerEnterEvent?.Invoke(this);
        }

        public void OnPointerExit(BaseEventData eventData)
        {
            isPointerOver = false;

            // Tooltip logic works better for PC at the moment.
            // I actually think that it looks better when it stays up instead of disappearing.
            // OnPointerExitEvent?.Invoke(this);
        }

        #region DRAG LISTENERS.
        /// <summary>
        /// Drag functions need some work because of the nature of a VRIK_Draggable.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag(BaseEventData eventData)
        {
            OnBeginDragEvent?.Invoke(this);
        }

        public void OnEndDrag(BaseEventData eventData)
        {
            OnEndDragEvent?.Invoke(this);
        }

        public void OnDrag(BaseEventData eventData)
        {
            OnDragEvent?.Invoke(this);
        }

        public void OnDrop(BaseEventData eventData)
        {
            OnDropEvent?.Invoke(this);
        }
        #endregion
        #endregion

        #region HELPERS
        public virtual bool CanAddStack(Item item, int amount = 1)
        {
            // If we stacked the item with another item with the same ID, would double the amount fit?
            return Item != null && Item.ID == item.ID && Amount + amount <= item.MaximumStacks;
        }

        /// <summary>
        /// For regular item slots, item slots can always receive an item!
        /// Rather than like EquipmentSlots that can only receive Equippables.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual bool CanReceiveItem(Item item)
        {
            return true;
        }
        #endregion
    }
}
