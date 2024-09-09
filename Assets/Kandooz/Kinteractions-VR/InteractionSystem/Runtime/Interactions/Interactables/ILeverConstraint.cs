using UnityEngine;

namespace Kandooz.InteractionSystem.Interactions
{
    public interface ILeverConstraint
    {
        Quaternion Rotate(Vector3 direction);
    }
}