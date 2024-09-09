using UnityEngine;

namespace Kandooz.ScriptableSystem
{
    [CreateAssetMenu(menuName = "Kandooz/Scriptable System/Variables/InVariable")]
    public class IntVariable : ScriptableVariable<int>
    {
        public void Increment()
        {
            Value++;
        }
    }
}