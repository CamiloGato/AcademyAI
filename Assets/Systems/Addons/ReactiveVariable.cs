using System;

namespace Systems.Addons
{
    [Serializable]
    public class ReactiveVariable<T>
    {
        public Action<T> OnValueChanged = delegate { };
        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                OnValueChanged?.Invoke(value);
            }
        }
    }
}