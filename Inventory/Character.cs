using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoulDrop
{
    /// <summary>
    /// It's like CharacterStats.cs. We need to merge the too and probably organize this better.
    /// Controls what happens when we equip or unequip an item.
    /// </summary>
    public class Character : MonoBehaviour
    {
        // 3 primary stats.
        public CharacterStat MeleeDamage;
        public CharacterStat RangedDamage;
        public CharacterStat Health;

        // The panels.
        [SerializeField] Inventory inventory;
        [SerializeField] EquipmentPanel equipmentPanel;
        [SerializeField] StatPanel statPanel;
        [SerializeField] ItemTooltip itemTooltip;

        // This was taken from the tutorial. For having a draggable image be a visual cue.
        // We use a draggableItem Image, so that we don't have overlapping images when dragging
        // I'm not using it, but keeping it for reference right now.
        [SerializeField] Image draggableItem;

        // The slot that we're currently dragging.
        private ItemSlot draggedSlot;

        #region INITIALIZIATION
        private void OnValidate()
        {
            if (itemTooltip == null)
            {
                // Find the tooltip. Only run in the editor.
                // Normally, it would be SLOW, but since it's just in the editor, it's OK.
                itemTooltip = FindObjectOfType<ItemTooltip>();
            }
        }

        private void Awake()
        {
            statPanel.SetStats(MeleeDamage, RangedDamage, Health);
            statPanel.UpdateStatValues();

            // Setup Events:
            // Click
            inventory.OnPointerClickEvent += Equip;
            equipmentPanel.OnPointerClickEvent += Unequip;
            // Pointer Enter
            inventory.OnPointerEnterEvent += ShowTooltip;
            equipmentPanel.OnPointerEnterEvent += ShowTooltip;
            // Pointer Exit
            inventory.OnPointerExitEvent += HideTooltip;
            equipmentPanel.OnPointerExitEvent += HideTooltip;
            // Begin Drag
            inventory.OnBeginDragEvent += BeginDrag;
            equipmentPanel.OnBeginDragEvent += BeginDrag;
            // End Drag
            inventory.OnEndDragEvent += EndDrag;
            equipmentPanel.OnEndDragEvent += EndDrag;
            // Drag
            inventory.OnDragEvent += Drag;
            equipmentPanel.OnDragEvent += Drag;
            // Drop
            inventory.OnDropEvent += Drop;
            equipmentPanel.OnDropEvent += Drop;
        }
        #endregion

        #region ItemSlot TRIGGERS THESE FUNCTIONS THROUGH LISTENERS.
        /// <summary>
        /// OnPointerClick.
        /// </summary>
        /// <param name="itemSlot"></param>
        private void Equip(ItemSlot itemSlot)
        {
            // Check if the itemSlot is an Equippable one.
            EquippableItem equippableItem = itemSlot.Item as EquippableItem;
            if (equippableItem != null)
            {
                Equip(equippableItem);
            }
        }

        /// <summary>
        /// OnPointerClick.
        /// </summary>
        /// <param name="itemSlot"></param>
        private void Unequip(ItemSlot itemSlot)
        {
            EquippableItem equippableItem = itemSlot.Item as EquippableItem;
            if (equippableItem != null)
            {
                Unequip(equippableItem);
            }
        }

        /// <summary>
        /// OnPointerEnter.
        /// </summary>
        /// <param name="itemSlot"></param>
        private void ShowTooltip(ItemSlot itemSlot)
        {
            EquippableItem equippableItem = itemSlot.Item as EquippableItem;
            if (equippableItem != null)
            {
                itemTooltip.ShowTooltip(equippableItem);
            }
        }

        /// <summary>
        /// OnPointerExit. This trigger is offline for now.
        /// </summary>
        /// <param name="itemSlot"></param>
        private void HideTooltip(ItemSlot itemSlot)
        {
            itemTooltip.HideTooltip();
        }

        /// <summary>
        /// We need to fix the drag functions because the VRTK_Draggable is a little weird.
        /// </summary>
        /// <param name="itemSlot"></param>
        private void BeginDrag(ItemSlot itemSlot)
        {
            if (itemSlot.Item != null)
            {
                draggedSlot = itemSlot;
                draggableItem.sprite = itemSlot.Item.Icon;
                // Usually, the position would be set = Input.mousePosition.
                // I'm trying to figure out the best way to get the VRTK Pointer collision.
                
                // Tutorial code. Need to reimplement how VRTK handles Draggables.
                // draggableItem.transform.position = itemSlot.transform.position;
                // draggableItem.enabled = true;
            }
        }

        private void EndDrag(ItemSlot itemSlot)
        {
            // Disable the draggable item image.
            draggedSlot = null;
            draggableItem.enabled = false;
        }

        private void Drag(ItemSlot itemSlot)
        {
            if (draggableItem.enabled)
            {
                // In a normal game, the position would = Input.mousePosition.
                // Tutorial code. Need to reimplement how VRTK handles Draggables.
                draggableItem.transform.position = itemSlot.transform.position;
            }
        }

        private void Drop(ItemSlot dropItemSlot)
        {
            // Avoid the null reference if you're dragging an empty Item Slot.
            if (draggedSlot == null)
            {
                return;
            }

            // Check if the DROP LOCATION CAN RECEIVE THE DRAGGED ITEM and vice versa.
            // We're checking if a swap is possible.
            // If there was no item in the DROP LOCATION, then, it will be true.
            if (dropItemSlot.CanReceiveItem(draggedSlot.Item) && draggedSlot.CanReceiveItem(dropItemSlot.Item))
            {
                EquippableItem dragItem = draggedSlot.Item as EquippableItem;
                EquippableItem dropItem = dropItemSlot.Item as EquippableItem;

                // DRAGGED ITEM IS EquipmentSlot, meaning the Origin was an EquipmentSlot.
                if (draggedSlot is EquipmentSlot)
                {
                    if (dragItem != null)
                    {
                        dragItem.Unequip(this);
                    }
                    if (dropItem != null)
                    {
                        dropItem.Equip(this);
                    }
                }
                // DROP ITEM IS EquipmentSlot, meaning the Destination is an EquipmentSlot.
                if (dropItemSlot is EquipmentSlot)
                {
                    if (dragItem != null)
                    {
                        dragItem.Equip(this);
                    }
                    if (dropItem != null)
                    {
                        dropItem.Unequip(this);
                    }
                }
                // Update the stat values.
                statPanel.UpdateStatValues();

                // Swap items between the DRAGGED ITEM and the DROP LOCATION ITEM.
                Item draggedItem = draggedSlot.Item;

                // Temporary variable.
                int draggedItemAmount = draggedSlot.Amount;

                // SWITCH BOTH THE ITEM AND THE AMOUNT.
                draggedSlot.Item = dropItemSlot.Item;
                draggedSlot.Amount = dropItemSlot.Amount;

                dropItemSlot.Item = draggedItem;
                dropItemSlot.Amount = draggedItemAmount;
            }
        }
        #endregion

        #region EQUIPS and UNEQUIPS helpers.
        /// <summary>
        /// Only equip items that are equippable.
        /// </summary>
        /// <param name="item"></param>
        private void EquipFromInventory(Item item)
        {
            if (item is EquippableItem)
            {
                Equip((EquippableItem)item);
            }
        }

        /// <summary>
        /// Pointer clicked on the already equipped item.
        /// </summary>
        /// <param name="item"></param>
        private void UnequipFromEquipPanel(Item item)
        {
            if (item is EquippableItem)
            {
                Unequip((EquippableItem)item);
            }
        }

        public void Equip(EquippableItem item)
        {
            // Equip the item. Remove from inventory.
            if (inventory.RemoveItem(item))
            {
                // Hide the Stat tool bar if you manage to equip something.
                StatTooltip.Instance.HideTooltip();

                EquippableItem previousItem;
                // If there was a previousItem, the out variable takes the previousItem if set inside AddItem.
                if (equipmentPanel.AddItem(item, out previousItem))
                {
                    if (previousItem != null)
                    {
                        inventory.AddItem(previousItem);
                        // An item is currently equipped.
                        previousItem.Unequip(this);
                        statPanel.UpdateStatValues();
                    }
                    // Equip the item.
                    item.Equip(this);
                    statPanel.UpdateStatValues();
                }
                else
                {
                    inventory.AddItem(item);
                }
            }
        }

        public void Unequip(EquippableItem item)
        {
            // If inventory is not full and you had the item equipped.
            if (!inventory.IsFull() && equipmentPanel.RemoveItem(item))
            {
                // Hide the Stat tool bar if you manage to unequip something.
                StatTooltip.Instance.HideTooltip();

                // Unequip and update the stats.
                item.Unequip(this);
                statPanel.UpdateStatValues();
                inventory.AddItem(item);
            }
        }
        #endregion
    }
}
