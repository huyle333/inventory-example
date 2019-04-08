using UnityEngine;
using VRTK;

namespace SoulDrop
{
    public class InventoryInput : MonoBehaviour
    {
        [SerializeField] GameObject characterPanelGameObject;
        [SerializeField] GameObject equipmentPanelGameObject;

        /// <summary>
        /// Used to close the entire menu.
        /// </summary>
        public void ToggleCharacterPanel()
        {
            characterPanelGameObject.SetActive(!characterPanelGameObject.activeSelf);

            // Dismiss the stat and item tooltips.
            StatTooltip.Instance.HideTooltip();
            ItemTooltip.Instance.HideTooltip();

            // Voice command is menu, so reposition the menu in front of you.
            Transform cameraEye = VRTK_DeviceFinder.HeadsetCamera();
            transform.parent.position = cameraEye.position + cameraEye.forward * 2;
            // Just change the Y Axis.
            transform.parent.rotation = Quaternion.Euler(0, cameraEye.rotation.eulerAngles.y, 0);
        }

        /// <summary>
        /// Used to just toggle out the equipment, so that you can just look at your inventory.
        /// No equipment. EQUIPMENT text is a button.
        /// </summary>
        public void ToggleEquipmentPanel()
        {
            equipmentPanelGameObject.SetActive(!equipmentPanelGameObject.activeSelf);

            // Hide the stat tool tip.
            StatTooltip.Instance.HideTooltip();
        }
    }
}
