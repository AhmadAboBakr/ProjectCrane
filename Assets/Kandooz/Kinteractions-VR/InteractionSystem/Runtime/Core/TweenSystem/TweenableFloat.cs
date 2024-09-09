using System;
using UnityEngine;

namespace Kandooz.InteractionSystem.Core
{
    /// <summary>
    /// tweens a float between two values
    /// </summary>
    public class TweenableFloat : ITweenable
    {
        public event Action<float> OnChange;
        public event Action OnFinished;
        private float _start;
        private float _target;
        private float _value;
        private float _rate;
        private float _t;
        private readonly VariableTweener _tweener;
        public float Value
        {
            get
            {
                return _value;
            }

            set
            {
#if UNITY_EDITOR
                if (UnityEditor.EditorApplication.isPlaying)
#endif
                {
                    _t = 0;
                    _start = this._value;
                    _target = value;
                    _tweener.AddTweenable(this);
                }
#if UNITY_EDITOR
                else
                {
                    this._value = value;
                    OnChange?.Invoke(value);
                }
#endif
            }
        }

        public float Rate {  set => _rate = value; }

        public TweenableFloat(VariableTweener tweener,Action<float> onChange=null, float rate = 2f, float value = 0)
        {
            _start = _target = this._value = value;
            this._rate = rate;
            this._t = 0;
            this.OnChange = onChange;
            this._tweener = tweener;
        }


        public void Subscribe(Action<ITweenable> action)
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe(Action<ITweenable> action)
        {
            throw new NotImplementedException();
        }

        public bool Tween(float scaledDeltaTime)
        {
            _t += _rate * scaledDeltaTime;
            this._value = Mathf.Lerp(_start, _target, _t);
            OnChange(_value);

            if (_t >= 1)
            {
                OnFinished?.Invoke();
                return true;
            }
            return false;
        }

    }

}