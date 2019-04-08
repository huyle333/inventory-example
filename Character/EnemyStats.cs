using UnityEngine;
using TMPro;
using VRTK;

namespace SoulDrop
{
    public class EnemyStats : CharacterStats
    {
        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);

            // Create the damage text.
            GameObject damageText = Instantiate(Resources.Load("Text/DamageText") as GameObject);
            TextMeshPro textMeshPro = damageText.GetComponent<TextMeshPro>();
            textMeshPro.text = damage.ToString();
            Destroy(damageText, 1);
            damageText.transform.parent = this.transform;

        }

        public override void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Weapon")
            {
                TakeDamage(10);
            }

            // Shot by the wand.
            if (other.gameObject.name == "ExpelliarmusProjectile(Clone)")
            {
                // Create the impact effect at the location the spell hit.
                GameObject impact = Instantiate(Resources.Load("Effects/Wand/ExpelliarmusImpact"),
                    other.transform.position,
                    other.transform.rotation) as GameObject;
                Destroy(impact, 1f);

                // Destroy the ExpelliarmusProjectile.
                Destroy(other);

                // SFX impact.
                AudioManager.instance.Play("SpellImpact");
            }

            // invulnerability is inherited by CharacterStats.cs.
            invulnerable = true;
            invulnerabilityTimeUp = Time.time + invulnerabilityLength;
        }
    }
}