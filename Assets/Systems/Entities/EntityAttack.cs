using UnityEngine;

namespace Systems.Entities
{
    public class EntityAttack : MonoBehaviour
    {
        public void AttackEntity(Entity entity, int damage)
        {
            entity.TakeDamage(damage);
        }
    }
}