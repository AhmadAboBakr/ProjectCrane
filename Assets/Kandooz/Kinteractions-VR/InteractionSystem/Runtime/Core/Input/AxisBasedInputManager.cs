using UnityEngine;

namespace Kandooz.InteractionSystem.Core
{
    /// <summary>
    /// this Input managers rely on preset axis that are added automaticlly by the tool
    /// </summary>
    internal class AxisBasedInputManager : InputManagerBase
    {
        // TODO: Fix issue with buttons relying on axis
        private const string LEFT_TRIGGER_AXIS = "XRI_Left_Trigger";
        private const string LEFT_TRIGGER_BUTTON = "XRI_Left_TriggerButton";
        private const string LEFT_GRIP_AXIS = "XRI_Left_Grip";
        private const string LEFT_GRIP_BUTTON = "XRI_Left_GripButton";
        private const string XRI_LEFT_PRIMARY_TOUCH = "XRI_Left_PrimaryButton";
        private const string XRI_LEFT_SECONDARY_TOUCH = "XRI_Left_SecondaryButton";

        private const string RIGHT_TRIGGER_AXIS = "XRI_Right_Trigger";
        private const string RIGHT_TRIGGER_BUTTON = "XRI_Right_TriggerButton";
        private const string RIGHT_GRIP_AXIS = "XRI_Right_Grip";
        private const string RIGHT_GRIP_BUTTON = "XRI_Right_GripButton";
        private const string XRI_RIGHT_PRIMARY_TOUCH = "XRI_Right_PrimaryButton";
        private const string XRI_RIGHT_SECONDARY_TOUCH = "XRI_Right_SecondaryButton";


        private void Update()
        {
            var leftThumbPressed = Input.GetKey(KeyCode.Joystick1Button1);

            LeftHand[0] = leftThumbPressed ? 1 : 0;
            LeftHand[1] = Input.GetAxisRaw(LEFT_TRIGGER_AXIS);
            LeftHand[2] = LeftHand[3] = LeftHand[4] = Input.GetAxisRaw(LEFT_GRIP_AXIS);

            LeftHand.TriggerObserver.ButtonState = Input.GetAxisRaw(LEFT_TRIGGER_AXIS) > .5f || Input.GetKey(KeyCode.Z);
            LeftHand.GripObserver.ButtonState = Input.GetAxisRaw(LEFT_GRIP_AXIS) > .5 || Input.GetKey(KeyCode.X);

            var rightThumbPressed = Input.GetKey(KeyCode.Joystick1Button2);

            RightHand[0] = rightThumbPressed ? 1 : 0;
            RightHand[1] = Input.GetAxisRaw(RIGHT_TRIGGER_AXIS);
            RightHand[2] = RightHand[3] = RightHand[4] = Input.GetAxisRaw(RIGHT_GRIP_AXIS);
            RightHand.TriggerObserver.ButtonState = Input.GetAxisRaw(RIGHT_TRIGGER_AXIS) > .5 || Input.GetKey(KeyCode.M);
            RightHand.GripObserver.ButtonState = Input.GetAxisRaw(RIGHT_GRIP_AXIS) > .5f || Input.GetKey(KeyCode.N);
        }
    }
}