using UnityEngine;

namespace Kandooz.InteractionSystem
{


    public class MinMaxAttribute : PropertyAttribute
    {
        public float MinLimit = 0;
        public float MaxLimit = 1;


        public MinMaxAttribute(int min, int max)
        {
            MinLimit = min;
            MaxLimit = max;
        }
    }
}