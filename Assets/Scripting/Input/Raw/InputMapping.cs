using System.Collections.Generic;
using UnityEngine.InputSystem;

/// <summary>
/// Represents a mapping of inputs to the status of if they are being held or not. This is needed for the observation of
/// "is the player holding down a button" which is otherwise impossible to make w/ just an input buffer.
/// </summary>
public class InputMapping : IPerformedInputFeatures, ICanceledInputFeatures
{
    /// <summary>
    /// Maps input types to their held status.
    /// </summary>
    private readonly Dictionary<InputType, bool> m_inputHeldMap;

    /// <summary>
    /// Constructs a default input mapping with the entries all set to false.
    /// </summary>
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
    /// <summary>
    /// Returns true if the given input is currently being held.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool IsInputHeld(InputType type) => m_inputHeldMap[type];

    // Helpers to hook into the interface methods. Performed enables the entry, canceled disables it.
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
