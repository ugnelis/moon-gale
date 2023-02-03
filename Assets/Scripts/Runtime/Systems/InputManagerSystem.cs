using System.Collections.Generic;
using MoonGale.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonGale.Runtime.Systems
{
    internal sealed class InputManagerSystem : MonoBehaviour, ISystem, IInputManagerSystem
    {
        [SerializeField]
        private List<InputActionReference> inputActionReferences;

        private void Start()
        {
            EnableInputs();
        }

        public void EnableInputs()
        {
            foreach (var inputActionReference in inputActionReferences)
            {
                inputActionReference.action.Enable();
            }
        }

        public void DisableInputs()
        {
            foreach (var inputActionReference in inputActionReferences)
            {
                inputActionReference.action.Disable();
            }
        }
    }
}
