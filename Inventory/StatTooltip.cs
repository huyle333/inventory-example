using UnityEngine;
using UnityEngine.UI;
using System.Text;

namespace SoulDrop
{
    /// <summary>
    /// Equivalent to the ItemTooltip.cs but used for Stats.
    /// </summary>
    public class StatTooltip : MonoBehaviour
    {
        public static StatTooltip Instance;

        [SerializeField] Text statNameText;
        [SerializeField] Text finalValueText;
        [SerializeField] Text modifiersListText;

        private StringBuilder sb = new StringBuilder();

        private void Awake()
        {
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

        #region SHOW and HIDE TOOLTIP
        public void ShowTooltip(CharacterStat stat, string statName)
        {
            gameObject.SetActive(true);

            // Update the text on the tooltip.
            statNameText.text = FirstLetterToUpper(statName);
            finalValueText.text = GetValueText(stat);
            modifiersListText.text = GetModifiersText(stat);
        }

        public void HideTooltip()
        {
            gameObject.SetActive(false);
        }
        #endregion

        #region HELPERS
        /// <summary>
        /// 15 + (5). Shows the stat boosts given by armor or weapons.
        /// </summary>
        /// <param name="stat"></param>
        /// <returns></returns>
        private string GetValueText(CharacterStat stat)
        {
            sb.Length = 0;

            sb.Append(stat.Value);
            sb.Append(" (");
            sb.Append(stat.BaseValue);
            sb.Append(" + ");
            sb.Append((float)System.Math.Round(stat.Value - stat.BaseValue, 4));
            sb.Append(")");

            return sb.ToString();
        }

        /// <summary>
        /// The actual modifiers listened in a list order.
        /// </summary>
        /// <param name="stat"></param>
        /// <returns></returns>
        private string GetModifiersText(CharacterStat stat)
        {
            sb.Length = 0;

            for (int i = 0; i < stat.StatModifiers.Count; i++)
            {
                StatModifier mod = stat.StatModifiers[i];

                sb.Append(((Item)mod.Source).name);
                sb.Append(": ");

                if (mod.Value > 0)
                {
                    sb.Append("+");
                }

                if (mod.Type == StatModType.Flat)
                {
                    sb.Append(mod.Value);
                }
                else
                {
                    sb.Append(mod.Value * 100);
                    sb.Append("%");
                }

                if (i < stat.StatModifiers.Count - 1)
                {
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// The STAT NAME should be capitalized.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private string FirstLetterToUpper(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
        #endregion
    }
}