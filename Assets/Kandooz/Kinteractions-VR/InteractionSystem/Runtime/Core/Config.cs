using System.Runtime.CompilerServices;
using Kandooz.InteractionSystem.Animations;
using UnityEngine;

[assembly: InternalsVisibleTo("Kandooz.Interactions.Editor")]

namespace Kandooz.InteractionSystem.Core
{
    /// <summary>
    /// The config class configures the layers and the default input manager to be used by the system
    /// </summary>
    [CreateAssetMenu(order = 10, menuName = "Kandooz/Interaction System/Config")]
    public class Config : ScriptableObject
    {
        [SerializeField] private HandData handData;
        [SerializeField] private LayerMask leftHandLayer;
        [SerializeField] private LayerMask rightHandLayer;
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private InputManagerType inputType;

        [Header("Hand Physics Data")] 
        [SerializeField] private float handMass = 30;

        [SerializeField] private float linearDamping = 5, angularDamping = 1;
        [Core.ReadOnly,SerializeField] private GameObject gameManager;

        [Core.ReadOnly] [SerializeField] private InputManagerBase inputManager;

        public int LeftHandLayer
        {
            get => (int)(Mathf.Log(leftHandLayer, 2) + .5f);
            internal set => leftHandLayer = value;
        }

        public int RightHandLayer
        {
            get => (int)(Mathf.Log(rightHandLayer, 2) + .5f);
            internal set => rightHandLayer = value;
        }

        public int InteractableLayer
        {
            get => (int)(Mathf.Log(interactableLayer, 2) + .5f);
            internal set => interactableLayer = value;
        }

     

        public int PlayerLayer
        {
            get => (int)(Mathf.Log(playerLayer, 2) + .5f);
            internal set => playerLayer = value;
        }

        public HandData HandData => handData;

        public InputManagerBase InputManager
        {
            get
            {
                if (inputManager) return inputManager;
                if (gameManager) return CreateInputManager();
                gameManager = new GameObject("VR Manager");
                DontDestroyOnLoad(gameManager);
                return CreateInputManager();
            }
        }

        public float HandMass => handMass;
        public float HandLinearDamping => linearDamping;
        public float HandAngularDamping => angularDamping;

        private InputManagerBase CreateInputManager()
        {
            switch (inputType)
            {
                case InputManagerType.UnityAxisBased:
                    if (inputManager != null && inputManager is AxisBasedInputManager) return inputManager;
                    if (inputManager) Destroy(inputManager);
                    inputManager = gameManager.AddComponent<AxisBasedInputManager>();
                    break;
                case InputManagerType.InputSystem:
                    if (inputManager != null && inputManager is InputSystemBased) return inputManager;
                    if (inputManager) Destroy(inputManager);
                    inputManager = gameManager.AddComponent<InputSystemBased>();
                    break;
                case InputManagerType.KeyboardMock:
                    if (inputManager != null && inputManager is KeyboardBasedInput) return inputManager;
                    if (inputManager) Destroy(inputManager);
                    inputManager = gameManager.AddComponent<KeyboardBasedInput>();

                    break;
            }

            return inputManager;
        }

        private enum InputManagerType
        {
            UnityAxisBased = 0,
            InputSystem = 1,
            KeyboardMock = -1
        }
    }

    public class InputSystemBased : InputManagerBase
    {
    }
}