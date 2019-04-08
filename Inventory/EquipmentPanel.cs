using UnityEngine;
using System;

namespace SoulDrop
{
    /// <summary>
    /// Similar to how Inventory.cs works.
    /// </summary>
    public class EquipmentPanel : MonoBehaviour
    {
        [SerializeField]
        Transform equipmentSlotsParent;

        [SerializeField]
        EquipmentSlot[] equipmentSlots;

        // Inventory uses the same events for triggering ItemSlot and EquipmentSlot.
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
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                // Clicked on the EquipmentSlot.
                equipmentSlots[i].OnPointerClickEvent += slot => OnPointerClickEvent(slot);
                equipmentSlots[i].OnPointerEnterEvent += slot => OnPointerEnterEvent(slot);
                equipmentSlots[i].OnPointerExitEvent += slot => OnPointerExitEvent(slot);
                equipmentSlots[i].OnBeginDragEvent += slot => OnBeginDragEvent(slot);
                equipmentSlots[i].OnEndDragEvent += slot => OnEndDragEvent(slot);
                equipmentSlots[i].OnDragEvent += slot => OnDragEvent(slot);
                equipmentSlots[i].OnDropEvent += slot => OnDropEvent(slot);
            }
        }

        private void OnValidate()
        {
            equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
        }
        #endregion

        #region EQUIP and UNEQUIP.
        /// <summary>
        /// Equipped.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="previousItem">out parameter</param>
        /// <returns></returns>
        public bool AddItem(EquippableItem item, out EquippableItem previousItem)
        {
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                // The out parameter will be given back to the function call.
                if (equipmentSlots[i].EquipmentType == item.EquipmentType)
                {
                    // Send back the previousItem if there was something already equipped.
                    previousItem = (EquippableItem)equipmentSlots[i].Item;
                    equipmentSlots[i].Item = item;
                    return true;
                }
            }
            previousItem = null;
            return false;
        }

        /// <summary>
        /// Unequip.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool RemoveItem(EquippableItem item)
        {
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                if (equipmentSlots[i].Item == item)
                {
                    equipmentSlots[i].Item = null;
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
