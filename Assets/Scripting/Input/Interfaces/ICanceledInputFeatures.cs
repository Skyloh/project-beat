using UnityEngine.InputSystem;

public interface ICanceledInputFeatures
{
    void CanceledMove(InputAction.CallbackContext context);
    void CanceledJump(InputAction.CallbackContext context);
    void CanceledButton1(InputAction.CallbackContext context);
    void CanceledButton2(InputAction.CallbackContext context);
    void CanceledButton3(InputAction.CallbackContext context);
    void CanceledButton4(InputAction.CallbackContext context);
}