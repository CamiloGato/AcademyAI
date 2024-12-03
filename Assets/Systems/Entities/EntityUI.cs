using UnityEngine;
using UnityEngine.UI;

namespace Systems.Entities
{
    public class EntityUI : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider energySlider;
        
        private Entity _entity;
        
        private void Awake()
        {
            _entity = GetComponent<Entity>();
        }

        private void OnEnable()
        {
            _entity.health.OnValueChanged += OnHealthChanged;
            _entity.energy.OnValueChanged += OnEnergyChanged;
        }

        private void OnDisable()
        {
            _entity.health.OnValueChanged -= OnHealthChanged;
            _entity.energy.OnValueChanged -= OnEnergyChanged;
        }

        private void OnEnergyChanged(int value)
        {
            float percent = (float) value / Entity.MaxEnergy;
            energySlider.value = percent;
        }

        private void OnHealthChanged(int value)
        {
            float percent = (float) value / Entity.MaxHealth;
            healthSlider.value = percent;
        }
    }
}