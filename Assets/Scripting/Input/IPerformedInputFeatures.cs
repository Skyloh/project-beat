using UnityEngine.InputSystem;

public interface IPerformedInputFeatures
{
    void PerformedMove(InputAction.CallbackContext context);
    void PerformedJump(InputAction.CallbackContext context);
    void PerformedButton1(InputAction.CallbackContext context);
    void PerformedButton2(InputAction.CallbackContext context);
    void PerformedButton3(InputAction.CallbackContext context);
    void PerformedButton4(InputAction.CallbackContext context);
}