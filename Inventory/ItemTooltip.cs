using UnityEngine;
using UnityEngine.UI;
using System.Text;

namespace SoulDrop
{
    public class ItemTooltip : MonoBehaviour
    {
        // Static instance.
        public static ItemTooltip Instance;

        [SerializeField]
        Text ItemNameText;

        [SerializeField]
        Text ItemSlotText;

        [SerializeField]
        Text ItemStatsText;

        private StringBuilder sb = new StringBuilder();

        private void Awake()
        {
            // Singleton.
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
            gameObject.SetActive(false);
        }

        #region SHOW and HIDE ITEM TOOLTIP
        public void ShowTooltip(EquippableItem item)
        {
            ItemNameText.text = item.ItemName;
            ItemSlotText.text = item.EquipmentType.ToString();

            // Stringbuilder does not re-create the string when concatenating.
            sb.Length = 0;
            AddStat(item.MeleeDamageBonus, " MeleeDamage");
            AddStat(item.RangedDamageBonus, " RangedDamage");
            AddStat(item.HealthBonus, " Health");

            AddStat(item.MeleeDamagePercentBonus * 100, "% MeleeDamage");
            AddStat(item.RangedDamagePercentBonus * 100, "% RangedDamage");
            AddStat(item.HealthPercentBonus * 100, "% Health");

            // Assign text.
            ItemStatsText.text = sb.ToString();

            gameObject.SetActive(true);
        }

        public void HideTooltip()
        {
            gameObject.SetActive(false);
        }
        #endregion

        /// <summary>
        /// Arrange the text for EACH LINE.
        /// </summary>
        /// <param name="statBonus"></param>
        /// <param name="statName"></param>
        private void AddStat(float statBonus, string statName)
        {
            if (statBonus != 0)
            {
                if (sb.Length > 0)
                {
                    sb.AppendLine();
                }

                if (statBonus > 0)
                {
                    sb.Append("+");
                }

                sb.Append(statBonus);
                sb.Append(statName);
            }
        }
    }
}
