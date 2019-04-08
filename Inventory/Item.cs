using UnityEngine;
using UnityEditor;

namespace SoulDrop
{
    /// <summary>
    /// Used by Inventory.cs and ItemSlot.cs to create quick items.
    /// Equippable and unequippables.
    /// </summary>
    [CreateAssetMenu]
    public class Item : ScriptableObject
    {
        // Every item is its own instance.
        [SerializeField]
        string id;
        public string ID {
            get
            {
                return id;
            }
        }

        public string ItemName;
        [Range(1, 999)]
        public int MaximumStacks = 1;
        public Sprite Icon;

        private void OnValidate()
        {
            string path = AssetDatabase.GetAssetPath(this);
            // Get the globally unique identifier that Unity generates.
            id = AssetDatabase.AssetPathToGUID(path);
        }

        public virtual Item GetCopy()
        {
            return this;
        }

        public virtual void Destroy()
        {

        }
    }
}
