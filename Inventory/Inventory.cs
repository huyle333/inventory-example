using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;

namespace SoulDrop
{
    public class Inventory : MonoBehaviour
    {
        // Starting items for testing.
        [SerializeField]
        List<Item> startingItems;

        [SerializeField]
        Transform itemsParent;

        [SerializeField]
        ItemSlot[] itemSlots;

        // Character.cs is using these triggers that are used as the ItemSlot.
        public event Action<ItemSlot> OnPointerClickEvent;
        public event Action<ItemSlot> OnPointerEnterEvent;
        public event Action<ItemSlot> OnPointerExitEvent;
        public event Action<ItemSlot> OnBeginDragEvent;
        public event Action<ItemSlot> OnEndDragEvent;
        public event Action<ItemSlot> OnDragEvent;
        public event Action<ItemSlot> OnDropEvent;

        #region INITIALIZATION
        private void Start()
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                // Clicked on an ItemSlot. Lambda expressions because we're assigning events to other events..
                itemSlots[i].OnPointerClickEvent += slot => OnPointerClickEvent(slot);
                itemSlots[i].OnPointerEnterEvent += slot => OnPointerEnterEvent(slot);
                itemSlots[i].OnPointerExitEvent += slot => OnPointerExitEvent(slot);
                itemSlots[i].OnBeginDragEvent += slot => OnBeginDragEvent(slot);
                itemSlots[i].OnEndDragEvent += slot => OnEndDragEvent(slot);
                itemSlots[i].OnDragEvent += slot => OnDragEvent(slot);
                itemSlots[i].OnDropEvent += slot => OnDropEvent(slot);
            }
        }

        private void OnValidate()
        {
            // The ItemSlots will automatically be filled if itemsParent is dragged into
            // the Transform itemsParent.
            if (itemsParent != null)
            {
                itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();
            }

            // Every time a change happens to the Inventory, we will update the ItemSlot with the Item.
            // EVEN IF WE'RE NOT IN PLAY MODE.
            SetStartingItems();
        }

        /// <summary>
        /// Fill in the starting items that we give the character.
        /// </summary>
        private void SetStartingItems()
        {
            // AddItem will do proper checks.
            Clear();
            
            // Go through the ItemSlots in the Inventory.
            // Fill the ItemSlots with the item.
            // Then ItemSlot.cs has an OnValidate that will update its Image with the one referenced in Item.cs class.
            for (int i = 0; i < startingItems.Count; i++)
            {
                if (startingItems[i] != null)
                {
                    AddItem(startingItems[i].GetCopy());
                }
            }
        }

        public virtual void Clear()
        {
            // Clear the item contents to be refilled up by SetStartingItems.
            for (int i = 0; i < itemSlots.Length; i++)
            {
                itemSlots[i].Item = null;
                itemSlots[i].Amount = 0;
            }
        }
        #endregion

        #region INVENTORY
        /// <summary>
        /// Put the item into the slot and return true if that was possible.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddItem(Item item)
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                // FILL THE EXISTING STACK IF POSSIBLE.
                if (itemSlots[i].CanAddStack(item))
                {
                    itemSlots[i].Item = item;
                    itemSlots[i].Amount++;
                    return true;
                }
            }

            // Then, check if you can add it to any empty slot.
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i].Item == null)
                {
                    itemSlots[i].Item = item;
                    itemSlots[i].Amount++;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Try to remove the item if the item has the item that we want to remove.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool RemoveItem(Item item)
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i].Item == item)
                {
                    itemSlots[i].Amount--;
                    // Only make null if the itemSlots has been reduced to 0.
                    if (itemSlots[i].Amount == 0)
                    {
                        itemSlots[i].Item = null;
                    }
                    itemSlots[i].Item = null;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Remove Item based on ID.
        /// Not really used at the moment.
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public Item RemoveItem(string itemID)
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                Item item = itemSlots[i].Item;
                if (item != null && item.ID == itemID)
                {
                    itemSlots[i].Amount--;
                    return item;
                }
            }
            return null;
        }
        #endregion

        /// <summary>
        /// If we find at least 1 slot that was empty, then, it's NOT FULL yet.
        /// Used when unequipping an item.
        /// </summary>
        /// <returns></returns>
        public bool IsFull()
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i].Item == null)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Returns the number of items in the inventory.
        /// Not really used at the moment.
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public int ItemCount(string itemID)
        {
            int number = 0;

            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i].Item.ID == itemID)
                {
                    // Increase by item by stacking.
                    number += itemSlots[i].Amount;
                }
            }

            return number;
        }
    }
}
