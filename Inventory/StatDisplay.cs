using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SoulDrop
{
    /// <summary>
    /// This is the actual Stats underneath Character name Level details.
    /// The Individual StatDisplay Components!
    /// melee damage 15 (that stuff).
    /// </summary>
    public class StatDisplay : MonoBehaviour
    {
        public Text NameText;
        public Text ValueText;

        [NonSerialized]
        public CharacterStat Stat;

        private void OnValidate()
        {
            Text[] texts = GetComponentsInChildren<Text>();
            // Always appears in the order that they appear in the Editor hierarchy.
            NameText = texts[0];
            ValueText = texts[1];
        }

        private void Start()
        {
            EventTrigger trigger = this.GetComponent<EventTrigger>();
            // We want to keep track of a Hover.
            Helper.AddEventTriggerListener(trigger, EventTriggerType.PointerEnter, OnPointerEnter);
        }

        /// <summary>
        /// Show the tooltip when hovered on the Stat.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(BaseEventData eventData)
        {
            StatTooltip.Instance.ShowTooltip(Stat, NameText.text);
        }
    }
}
