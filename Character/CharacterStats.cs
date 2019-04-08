using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulDrop
{
    public class CharacterStats : MonoBehaviour
    {
        public CharacterStat CurrentHealth;
        public CharacterStat MaxHealth;

        public delegate void OnCurrentHealthChanged();
        public OnCurrentHealthChanged onCurrentHealthChangedCallback;

        // Give some time for the player/enemy to be invulnerable.
        // To prevent double hits.
        protected float invulnerabilityLength = 0.25f;
        protected float invulnerabilityTimeUp;
        protected bool invulnerable = false;

        private void Update()
        {
            // Use T key to inflict damage.
            if (Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log("DAMAGE TAKEN.");
            }

            if (invulnerable)
            {
                // After the invulnerabilityLength, you can be hit.
                if (Time.time > invulnerabilityTimeUp)
                {
                    invulnerable = false;
                }
            }
        }

        /// <summary>
        /// The player or enemy is getting damaged.
        /// </summary>
        /// <param name="damage"></param>
        public virtual void TakeDamage(int damage)
        {
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            // Subtract damage from health.
            CurrentHealth.BaseValue -= damage;
            Debug.Log(transform.name + " took " + damage + " damage!");

            // Update the UI using the listener.
            onCurrentHealthChangedCallback.Invoke();

            if (CurrentHealth.BaseValue <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// The player or enemy dies.
        /// </summary>
        public virtual void Die()
        {
            Debug.Log(transform.name + " died.");
        }

        /// <summary>
        /// Players and enemies will treat triggers differently.
        /// </summary>
        /// <param name="other"></param>
        public virtual void OnTriggerEnter(Collider other)
        {
        }
    }
}