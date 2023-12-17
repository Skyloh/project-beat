using UnityEngine.InputSystem;

/// <summary>
/// An interface for scripts that get input from input events.
/// </summary>
public interface IInputListener
{
    /// <summary>
    /// Passes the given input event (of a type and context) to the script.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="context"></param>
    void Accept(InputType type, InputAction.CallbackContext context);

    /// <summary>
    /// Is this listener ready to take input?
    /// </summary>
    /// <returns></returns>
    bool IsReady();
}
