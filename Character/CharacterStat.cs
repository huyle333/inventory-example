using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SoulDrop
{
    [Serializable]
    public class CharacterStat
    {
        public float BaseValue;

        // Do we need to recalculate the value?
        protected bool isDirty = true;
        protected float lastBaseValue;

        // Most recent final calculation.
        protected float _value;
        public virtual float Value
        {
            get
            {
                // Only calculate the value if changes have been made.
                if (isDirty || lastBaseValue != BaseValue)
                {
                    lastBaseValue = BaseValue;
                    _value = CalculateFinalValue();
                    isDirty = false;
                }
                return _value;
            }
        }

        // Modifier adjustments.
        protected readonly List<StatModifier> statModifiers;
        // We don't want to change the instantiation of the list.
        public readonly ReadOnlyCollection<StatModifier> StatModifiers;

        #region CONSTRUCTORS
        public CharacterStat()
        {
            statModifiers = new List<StatModifier>();
            StatModifiers = statModifiers.AsReadOnly();
        }

        /// <summary>
        /// When using this(), it will trigger the other constructor.
        /// </summary>
        /// <param name="baseValue"></param>
        public CharacterStat(float baseValue) : this()
        {
            BaseValue = baseValue;
        }
        #endregion

        #region ADD OR REMOVE MODIFIERS
        public virtual void AddModifier(StatModifier mod)
        {
            isDirty = true;
            statModifiers.Add(mod);
            // Sort by a comparison function.
            statModifiers.Sort(CompareModifierOrder);
        }

        public virtual bool RemoveModifier(StatModifier mod)
        {
            if (statModifiers.Remove(mod))
            {
                isDirty = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Return ture if removed a modifier from the source.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public virtual bool RemoveAllModifiersFromSource(object source)
        {
            bool didRemove = false;

            for (int i = statModifiers.Count - 1; i >= 0; i--)
            {
                if (statModifiers[i].Source == source)
                {
                    isDirty = true;
                    didRemove = true;
                    statModifiers.RemoveAt(i);
                }
            }

            return didRemove;
        }
        #endregion

        #region HELPER
        /// <summary>
        /// statModifiers.Sort will compare the a and b.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
        {
            // A BEFORE B if a < b.
            if (a.Order < b.Order)
            {
                return -1;
            }
            // A IN FRONT of B if a > b.
            else if (a.Order > b.Order)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region FINAL CALCULATION
        protected virtual float CalculateFinalValue()
        {
            float finalValue = BaseValue;
            float sumPercentAdd = 0;

            // Add up all the statModifiers.
            for (int i = 0; i < statModifiers.Count; i++)
            {
                StatModifier mod = statModifiers[i];

                // Change by number.
                if (mod.Type == StatModType.Flat)
                {
                    finalValue += statModifiers[i].Value;
                }
                // Add percentage.
                else if (mod.Type == StatModType.PercentAdd)
                {
                    sumPercentAdd += mod.Value;

                    if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }
                }
                // Change by multiplied percentage.
                else if (mod.Type == StatModType.PercentMult)
                {
                    finalValue *= 1 + mod.Value;
                }
            }

            // 12.0001f != 12f
            // Round to 4 significant digits.
            return (float)Math.Round(finalValue, 4);
        }
        #endregion
    }
}