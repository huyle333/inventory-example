using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulDrop
{
    [CreateAssetMenu]
    public class UsableItem : ScriptableObject
    {
        public bool IsConsumable;

        public virtual void Use(Character character)
        {

        }
    }
}
