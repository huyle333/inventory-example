using UnityEngine;

namespace SoulDrop
{
    /// <summary>
    /// Used in StatTooltip.cs because it's the StatPanel with StatDisplays that update
    /// on every change in equipment.
    /// </summary>
    public class StatPanel : MonoBehaviour
    {
        [SerializeField] StatDisplay[] statDisplays;
        [SerializeField] string[] statNames;

        private CharacterStat[] stats;

        private void OnValidate()
        {
            statDisplays = GetComponentsInChildren<StatDisplay>();
            UpdateStatNames();
        }

        /// <summary>
        /// Set the stats.
        /// </summary>
        /// <param name="charStats">Allows a variable number of parameters</param>
        public void SetStats(params CharacterStat[] charStats)
        {
            stats = charStats;

            // Check the length
            if (stats.Length > statDisplays.Length)
            {
                Debug.LogError("Not enough stat displays!");
            }

            for (int i = 0; i < statDisplays.Length; i++)
            {
                // Setup the stat connection.
                statDisplays[i].Stat = i < stats.Length ? stats[i] : null;
                statDisplays[i].gameObject.SetActive(i < statDisplays.Length);
            }
        }

        /// <summary>
        /// The number the stat has.
        /// </summary>
        public void UpdateStatValues()
        {
            for (int i = 0; i < stats.Length; i++)
            {
                statDisplays[i].ValueText.text = stats[i].Value.ToString();
            }
        }

        /// <summary>
        /// Melee Damage, Ranged Damage, and Health.
        /// </summary>
        public void UpdateStatNames()
        {
            for (int i = 0; i < statNames.Length; i++)
            {
                statDisplays[i].NameText.text = statNames[i];
            }
        }
    }
}
