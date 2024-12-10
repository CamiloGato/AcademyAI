using UnityEngine;

namespace Systems.Entities.Input
{
    public class InputPlayer : IInput
    {
        public Vector2 Direction()
        {
            return new Vector2();
        }

        public bool Jump()
        {
            return true;
        }

        public bool Attack()
        {
            return true;
        }

        public bool Interact()
        {
            return true;
        }
    }
}