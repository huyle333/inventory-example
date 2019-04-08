namespace SoulDrop
{
    /// <summary>
    /// Flat: 1 + 1.
    /// PercentAdd: 10% + 10%.
    /// PercentMult 100% * 2
    /// </summary>
    public enum StatModType
    {
        Flat = 100,
        PercentAdd = 200,
        PercentMult = 300,
    }

    public class StatModifier
    {
        public readonly float Value;
        public readonly StatModType Type;
        public readonly int Order;
        public readonly object Source;

        public StatModifier(float value, StatModType type, int order, object source)
        {
            Value = value;
            Type = type;
            Order = order;
            Source = source;
        }

        /// <summary>
        /// This format uses the above StatModifier constructor.
        /// Every enum value has an int value, so:
        /// we can just give a number for order.
        /// Do flats first. Then, percents afterwards.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public StatModifier(float value, StatModType type) : this(value, type, (int)type) { }

        // Value and Type are required.
        public StatModifier(float value, StatModType type, int order) : this(value, type, order, null) { }

        public StatModifier(float value, StatModType type, object source) : this(value, type, (int)type, source) { }
    }
}