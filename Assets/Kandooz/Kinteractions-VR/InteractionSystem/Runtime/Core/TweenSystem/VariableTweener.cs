using System.Collections.Generic;
using UnityEngine;

namespace Kandooz.InteractionSystem.Core
{
    /// <summary>
    /// Tweens multiple tweenable objects and adds them to 
    /// </summary>
    public class VariableTweener : MonoBehaviour
    {
        public float tweenScale = 12f;
        private List<ITweenable> _values= new();

        private void OnEnable()
        {
            _values = new List<ITweenable>();
        }
        public void AddTweenable(ITweenable value)
        {
            _values.Add(value);
        }

        public void RemoveTweenable(ITweenable value)
        {
            try
            {
                _values.Remove(value);
            }
            catch
            {
                // ignored
            }
        }
        void Update()
        {
            for (int i = _values.Count - 1; i >= 0; i--)
            {
                if (_values[i].Tween(Time.deltaTime * tweenScale))
                {
                    _values.RemoveAt(i);
                }
            }
        }
    }
}