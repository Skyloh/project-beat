using UnityEngine.InputSystem;

public interface IInputListener
{
    void HandleInput(InputType type, InputAction.CallbackContext context);
}
