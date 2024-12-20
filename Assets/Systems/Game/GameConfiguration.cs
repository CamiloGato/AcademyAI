using Systems.Entities;
using Systems.Entities.Cloth;
using UnityEngine;

namespace Systems.Game
{
    [CreateAssetMenu(fileName = "GameConfiguration", menuName = "Game/Configuration", order = 0)]
    public class GameConfiguration : ScriptableObject
    {
        public ClothStorage clothStorage;
        public Entity entityPrefab;
        public int amountNpc;
    }
}