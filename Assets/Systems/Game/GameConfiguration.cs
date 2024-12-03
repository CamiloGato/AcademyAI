using UnityEngine;

namespace Systems.Game
{
    [CreateAssetMenu(fileName = "GameConfiguration", menuName = "Game/Configuration", order = 0)]
    public class GameConfiguration : ScriptableObject
    {
        [Header("Entities Configuration")]
        public int entitiesMaxHealth;
        public int entitiesMaxEnergy;
        public int entitiesMaxAmount;
        
    }
}