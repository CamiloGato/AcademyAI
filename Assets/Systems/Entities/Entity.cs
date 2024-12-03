using Systems.Addons;
using UnityEngine;

namespace Systems.Entities
{
    public abstract class Entity : MonoBehaviour
    {
        public static int MaxHealth = 100;
        public static int MaxEnergy = 100;
        
        public ReactiveVariable<int> health;
        public ReactiveVariable<int> energy;

        public void TakeDamage(int damage)
        {
            int currentHealth = health.Value - damage;
            health.Value = Mathf.Clamp(currentHealth, 0, MaxHealth);
        }

        public void AddHealth(int value)
        {
            int currentHealth = health.Value + value;
            health.Value = Mathf.Clamp(currentHealth, 0, MaxHealth);
        }

        public void AddEnergy(int value)
        {
            int currentEnergy = energy.Value + value;
            energy.Value = Mathf.Clamp(currentEnergy, 0, MaxEnergy);
        }

        public void UseEnergy(int value)
        {
            int currentEnergy = energy.Value - value;
            energy.Value = Mathf.Clamp(currentEnergy, 0, MaxEnergy);
        }
    }
}