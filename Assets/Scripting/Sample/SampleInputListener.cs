using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A sample input listener that "blocks" input until 3 seconds after start. Tests to see if input is correct buffered in priority
/// order, and if input expires correctly after a period of time elapses.
/// </summary>
public class SampleInputListener : MonoBehaviour, IInputListener
{
    private bool m_ready = false;

    void Start()
    {
        Debug.Log("Waiting...");
        Invoke(nameof(ToggleReady), 3f);
    }

    private void ToggleReady()
    {
        Debug.Log("Go!");
        m_ready = true;
    }

    public void Accept(InputType type, InputAction.CallbackContext context)
    {
        Debug.Log(type);
    }

    public bool IsReady()
    {
        return m_ready;
    }
}
