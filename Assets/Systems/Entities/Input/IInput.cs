using UnityEngine;

namespace Systems.Entities.Input
{
    public interface IInput
    {
        Vector2 Direction();
        bool Jump();
        bool Attack();
        bool Interact();
    }
}