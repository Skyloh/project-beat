using UnityEngine.InputSystem;

public interface IInputListener
{
    void Accept(InputType type, InputAction.CallbackContext context);
    bool IsReady();
}
