using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulDrop
{
    public enum EquipmentType
    {
        Helmet,
        Chest,
        Gloves,
        Boots,
        Weapon1,
        Weapon2,
        Accessory1,
        Accessory2,
    }

    [CreateAssetMenu]
    public class EquippableItem : Item
    {
        public int MeleeDamageBonus;
        public int RangedDamageBonus;
        public int HealthBonus;
        [Space]
        public float MeleeDamagePercentBonus;
        public float RangedDamagePercentBonus;
        public float HealthPercentBonus;
        [Space]
        public EquipmentType EquipmentType;

        // Formalities for instantiating a unique instance of an equippable.
        public override Item GetCopy()
        {
            // New instance of the item. (Chests)
            return Instantiate(this);
        }

        public override void Destroy()
        {
            Destroy(this);
        }

        #region EQUIP and UNEQUIP
        /// <summary>
        /// Add the STATS to the ACTUAL CHARACTER.
        /// </summary>
        /// <param name="c"></param>
        public void Equip(Character c)
        {
            if (MeleeDamageBonus != 0)
            {
                c.MeleeDamage.AddModifier(new StatModifier(MeleeDamageBonus, StatModType.Flat, this));
            }
            if (RangedDamageBonus != 0)
            {
                c.RangedDamage.AddModifier(new StatModifier(RangedDamageBonus, StatModType.Flat, this));
            }
            if (HealthBonus != 0)
            {
                c.Health.AddModifier(new StatModifier(HealthBonus, StatModType.Flat, this));
            }

            if (MeleeDamagePercentBonus != 0)
            {
                c.MeleeDamage.AddModifier(new StatModifier(MeleeDamagePercentBonus, StatModType.PercentMult, this));
            }
            if (RangedDamagePercentBonus != 0)
            {
                c.RangedDamage.AddModifier(new StatModifier(RangedDamagePercentBonus, StatModType.PercentMult, this));
            }
            if (HealthPercentBonus != 0)
            {
                c.Health.AddModifier(new StatModifier(HealthPercentBonus, StatModType.PercentMult, this));
            }
        }

        /// <summary>
        /// Remove the modifiers when unequipped the item.
        /// </summary>
        /// <param name="c"></param>
        public void Unequip(Character c)
        {
            c.MeleeDamage.RemoveAllModifiersFromSource(this);
            c.RangedDamage.RemoveAllModifiersFromSource(this);
            c.Health.RemoveAllModifiersFromSource(this);
        }
        #endregion
    }
}
