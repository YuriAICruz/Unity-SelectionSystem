using System;
using Graphene.InputManager;
using UnityEngine;

namespace Graphene.SelectionSystem
{
    [Serializable]
    public class InputWrapper : InputSystem
    {
        [HideInInspector]
        public bool Add, Subtract;

        protected override void ExecuteCombo(int id)
        {
            switch (id)
            {
                case 50:
                    Add = true;
                    break;
                case 51:
                    Add = false;
                    break;
                case 55:
                    Subtract = true;
                    break;
                case 56:
                    Subtract = false;
                    break;
            }
        }
    }
}