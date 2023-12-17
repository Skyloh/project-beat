using System.Collections.Generic;
using UnityEngine.InputSystem;

public class InputBuffer
{
    // b/c 60 frames per second.
    private const float ONE_SIXTIETH = 1f / 60f;


    private readonly PriorityQueue<(InputType type, InputAction.CallbackContext data), int> m_buffer;

    // b/c ImmutableDictionary doesn't exist in this .NET version.
    // note that this doesn't prevent mutation, only reassignment. Ah, well.
    private readonly Dictionary<InputType, int> m_priorityTable;

    private readonly float m_originalExpirationPeriod;


    private float m_expirationPeriod;

    public InputBuffer(PriorityListingSO listing_data, float expiration_period = ONE_SIXTIETH)
    {
        m_buffer = new PriorityQueue<(InputType type, InputAction.CallbackContext data), int>();
        m_priorityTable = new Dictionary<InputType, int>();
        m_expirationPeriod = m_originalExpirationPeriod = expiration_period;

        var items = listing_data.GetPriorities();
        foreach (InputType type in items) m_priorityTable.Add(type, listing_data.GetPriorityOf(type));
    }

    public bool Empty() => m_buffer.Count == 0;

    public void Push(InputType type, InputAction.CallbackContext context) => m_buffer.Enqueue((type, context), m_priorityTable[type]);

    public (InputType type, InputAction.CallbackContext data) Pop() => m_buffer.Dequeue();

    public void ResetExpirationPeriod() => SetExpirationPeriod(m_originalExpirationPeriod);

    public void SetExpirationPeriod(float time) => m_expirationPeriod = time;

    public float GetExpirationPeriod() => m_expirationPeriod;
}
