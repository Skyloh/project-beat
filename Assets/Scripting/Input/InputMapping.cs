using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class InputMapping
{
    private readonly Controls m_linked;
    private readonly Action<InputType, InputAction.CallbackContext> m_registerAction;
    private readonly Dictionary<InputType, bool> m_inputHeldMap;

    public InputMapping(Controls controls, Action<InputType, InputAction.CallbackContext> register_action)
    {
        m_linked = controls;
        m_registerAction = register_action;

        m_inputHeldMap = new Dictionary<InputType, bool>() {
            { InputType.Movement, false },
            { InputType.Jump, false },
            { InputType.Button1, false },
            { InputType.Button2, false },
            { InputType.Button3, false },
            { InputType.Button4, false }
        };
    }

    public void Register()
    {
        m_linked.PlayerControls.Move.performed += ParseMove;
        m_linked.PlayerControls.Jump.performed += ParseJump;
        m_linked.PlayerControls.Button1.performed += ParseButton1;
        m_linked.PlayerControls.Button2.performed += ParseButton2;
        m_linked.PlayerControls.Button3.performed += ParseButton3;
        m_linked.PlayerControls.Button4.performed += ParseButton4;


        m_linked.PlayerControls.Move.canceled += LiftMove;
        m_linked.PlayerControls.Jump.canceled += LiftJump;
        m_linked.PlayerControls.Button1.canceled += LiftButton1;
        m_linked.PlayerControls.Button2.canceled += LiftButton2;
        m_linked.PlayerControls.Button3.canceled += LiftButton3;
        m_linked.PlayerControls.Button4.canceled += LiftButton4;
    }

    public void Deregister()
    {
        m_linked.PlayerControls.Move.performed -= ParseMove;
        m_linked.PlayerControls.Jump.performed -= ParseJump;
        m_linked.PlayerControls.Button1.performed -= ParseButton1;
        m_linked.PlayerControls.Button2.performed -= ParseButton2;
        m_linked.PlayerControls.Button3.performed -= ParseButton3;
        m_linked.PlayerControls.Button4.performed -= ParseButton4;


        m_linked.PlayerControls.Move.canceled -= LiftMove;
        m_linked.PlayerControls.Jump.canceled -= LiftJump;
        m_linked.PlayerControls.Button1.canceled -= LiftButton1;
        m_linked.PlayerControls.Button2.canceled -= LiftButton2;
        m_linked.PlayerControls.Button3.canceled -= LiftButton3;
        m_linked.PlayerControls.Button4.canceled -= LiftButton4;
    }

    public bool IsInputHeld(InputType type) => m_inputHeldMap[type];

    private void ProcessInput(InputType type, InputAction.CallbackContext context)
    {
        m_inputHeldMap[type] = true;
        m_registerAction(type, context);
    }

    private void InputLifted(InputType type) => m_inputHeldMap[type] = false;

    private void ParseMove(InputAction.CallbackContext context) => ProcessInput(InputType.Movement, context);
    private void ParseJump(InputAction.CallbackContext context) => ProcessInput(InputType.Jump, context);
    private void ParseButton1(InputAction.CallbackContext context) => ProcessInput(InputType.Button1, context);
    private void ParseButton2(InputAction.CallbackContext context) => ProcessInput(InputType.Button2, context);
    private void ParseButton3(InputAction.CallbackContext context) => ProcessInput(InputType.Button3, context);
    private void ParseButton4(InputAction.CallbackContext context) => ProcessInput(InputType.Button4, context);


    private void LiftMove(InputAction.CallbackContext _) => InputLifted(InputType.Movement);
    private void LiftJump(InputAction.CallbackContext _) => InputLifted(InputType.Jump);
    private void LiftButton1(InputAction.CallbackContext _) => InputLifted(InputType.Button1);
    private void LiftButton2(InputAction.CallbackContext _) => InputLifted(InputType.Button2);
    private void LiftButton3(InputAction.CallbackContext _) => InputLifted(InputType.Button3);
    private void LiftButton4(InputAction.CallbackContext _) => InputLifted(InputType.Button4);
}
