using System.Collections.Generic;
using UnityEngine.InputSystem;

public class InputMapping : IPerformedInputFeatures, ICanceledInputFeatures
{
    private readonly Dictionary<InputType, bool> m_inputHeldMap;

    public InputMapping()
    {
        m_inputHeldMap = new Dictionary<InputType, bool>() {
            { InputType.Movement, false },
            { InputType.Jump, false },
            { InputType.Button1, false },
            { InputType.Button2, false },
            { InputType.Button3, false },
            { InputType.Button4, false }
        };
    }

    // TODO: How will the player access this?
    public bool IsInputHeld(InputType type) => m_inputHeldMap[type];


    private void InputDown(InputType type) => m_inputHeldMap[type] = true;

    private void InputLifted(InputType type) => m_inputHeldMap[type] = false;

    #region Interfaces
    public void PerformedMove(InputAction.CallbackContext _) => InputDown(InputType.Movement);
    public void PerformedJump(InputAction.CallbackContext _) => InputDown(InputType.Jump);
    public void PerformedButton1(InputAction.CallbackContext _) => InputDown(InputType.Button1);
    public void PerformedButton2(InputAction.CallbackContext _) => InputDown(InputType.Button2);
    public void PerformedButton3(InputAction.CallbackContext _) => InputDown(InputType.Button3);
    public void PerformedButton4(InputAction.CallbackContext _) => InputDown(InputType.Button4);


    public void CanceledMove(InputAction.CallbackContext _) => InputLifted(InputType.Movement);
    public void CanceledJump(InputAction.CallbackContext _) => InputLifted(InputType.Jump);
    public void CanceledButton1(InputAction.CallbackContext _) => InputLifted(InputType.Button1);
    public void CanceledButton2(InputAction.CallbackContext _) => InputLifted(InputType.Button2);
    public void CanceledButton3(InputAction.CallbackContext _) => InputLifted(InputType.Button3);
    public void CanceledButton4(InputAction.CallbackContext _) => InputLifted(InputType.Button4);
    #endregion
}
