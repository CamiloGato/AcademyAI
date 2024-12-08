using System;

namespace Tools.Addons
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
                if (Equals(_value, value)) return;
                _value = value;
                OnValueChanged?.Invoke(value);
            }
        }

        public static implicit operator T(ReactiveVariable<T> value)
        {
            return value.Value;
        }
    }
}