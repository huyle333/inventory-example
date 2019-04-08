using UnityEngine;

namespace SoulDrop
{
    public class EquipmentSlot : ItemSlot
    {
        public EquipmentType EquipmentType;

        /// <summary>
        /// Rename the equipment slots.
        /// </summary>
        protected override void OnValidate()
        {
            base.OnValidate();
            gameObject.name = EquipmentType.ToString() + " Slot";
        }

        /// <summary>
        /// Equipment slots can only receive EquippableItem(s).
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool CanReceiveItem(Item item)
        {
            // If there was no item that was in the position,
            // then, the position can receive the new item for sure.
            if (item == null)
            {
                return true;
            }

            EquippableItem equippableItem = item as EquippableItem;
            // Only return true if the equippableItem is a valid equipment. Not just a regular item.
            return equippableItem != null && equippableItem.EquipmentType == EquipmentType;
        }
    }
}
