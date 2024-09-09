using UnityEngine;

namespace Kandooz.InteractionSystem.Interactions
{


    [System.Serializable]
    public class LeverConstraint1D : ILeverConstraint
    {
        public enum Axe
        {
            x,
            z
        }
        [SerializeField]private Axe axe = Axe.z;
        public Axe RotationAxe
        {
            set => axe = value;
            get => axe;
        }
        public Quaternion Rotate( Vector3 direction)
        {
            var (normal,zero) = axe switch
            {
                Axe.x => (Vector3.right,Vector3.up),
                Axe.z => (Vector3.forward,Vector3.up),
                _ => (Vector3.zero,Vector3.zero)
            };
            
            direction = Vector3.ProjectOnPlane(direction.normalized, normal);
            
            var angle = Vector3.SignedAngle(direction, zero,-normal);
            return CalculateQuaternion();

            Quaternion CalculateQuaternion()
            {
                return axe == Axe.x ? 
                    Quaternion.Euler(angle, 0, 0) : 
                    Quaternion.Euler(0, 0, angle);
            }
        }
    }
}