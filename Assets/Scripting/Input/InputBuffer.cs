using System.Collections.Generic;
using UnityEngine.InputSystem;

public class InputBuffer : IPerformedInputFeatures
{
    // b/c 60 frames per second.
    private const float ONE_SIXTIETH = 1f / 60f;


    private readonly List<(InputType type, InputAction.CallbackContext data)> m_buffer;
    private readonly List<float> m_bufferTimestamps;
    private readonly List<IInputListener> m_listeners;

    // b/c ImmutableDictionary doesn't exist in this .NET version.
    // note that this doesn't prevent mutation, only reassignment. Ah, well.
    private readonly Dictionary<InputType, int> m_priorityTable;

    private readonly float m_originalExpirationPeriod;


    private float m_expirationPeriod;

    public InputBuffer(PriorityListingSO listing_data, float expiration_period = ONE_SIXTIETH)
    {
        m_buffer = new();
        m_bufferTimestamps = new();
        m_listeners = new();

        m_priorityTable = new Dictionary<InputType, int>();
        m_expirationPeriod = m_originalExpirationPeriod = expiration_period;

        var items = listing_data.GetPriorities();
        foreach (InputType type in items) m_priorityTable.Add(type, listing_data.GetPriorityOf(type));
    }

    public void ResetExpirationPeriod() => SetExpirationPeriod(m_originalExpirationPeriod);

    public void SetExpirationPeriod(float time) => m_expirationPeriod = time;

    public float GetExpirationPeriod() => m_expirationPeriod;

    public void Subscribe(IInputListener listener)
    {
        if (!m_listeners.Contains(listener)) m_listeners.Add(listener);
    }

    public void Desubscribe(IInputListener listener) => m_listeners.Remove(listener);



    public bool IsEmpty() => m_buffer.Count == 0;

    private void Add(InputType type, InputAction.CallbackContext context, float timeStamp)
    {
        if (IsEmpty())
        {
            m_buffer.Add((type, context));
            m_bufferTimestamps.Add(timeStamp);

            return;
        }

        // binary insertion
        int min = 0;
        int max = m_buffer.Count - 1;
        int index = 0;
        bool offset = false;
        while (min <= max)
        {
            index = (min + max) / 2;

            if (IsOfHigherPriority(type, m_buffer[index].type))
            {
                max = index - 1;
                offset = false; // to place to the left (on top of) the index
            }
            else
            {
                min = index + 1;
                offset = true; // to place to the right of the index
            }
        }

        index += offset ? 1 : 0;

        m_buffer.Insert(index, (type, context));
        m_bufferTimestamps.Insert(index, timeStamp);
    }

    public void Cycle(float timeStamp)
    {
        if (IsEmpty()) return;

        TryBroadcast();
        Decay(timeStamp);
    }



    private void Decay(float timeStamp)
    {
        for (int index = m_buffer.Count; index >= 0; --index)
        {
            if (timeStamp - m_bufferTimestamps[index] > m_expirationPeriod) Remove(index);
        }
    }

    private void TryBroadcast()
    {
        // if not ready to broadcast, stop
        if (!m_listeners.TrueForAll((IInputListener listener) => listener.IsReady())) return;

        var (type, context) = Pop();

        m_listeners.ForEach((IInputListener listener) => listener.Accept(type, context));
    }

    private (InputType type, InputAction.CallbackContext context) Pop()
    {
        var cache = m_buffer[0];

        Remove(0);

        return cache;
    }

    private void Remove(int index)
    {
        m_buffer.RemoveAt(index);
        m_bufferTimestamps.RemoveAt(index);
    }

    private bool IsOfHigherPriority(InputType one, InputType two) => m_priorityTable[one] >= m_priorityTable[two];

    #region IPerformedInputFeatures
    // cursed, but as long as no one plays for several days straight I should be fine.
    public void PerformedMove(InputAction.CallbackContext context) => Add(InputType.Movement, context, (float)context.time);

    public void PerformedJump(InputAction.CallbackContext context) => Add(InputType.Jump, context, (float)context.time);

    public void PerformedButton1(InputAction.CallbackContext context) => Add(InputType.Button1, context, (float)context.time);

    public void PerformedButton2(InputAction.CallbackContext context) => Add(InputType.Button2, context, (float)context.time);

    public void PerformedButton3(InputAction.CallbackContext context) => Add(InputType.Button3, context, (float)context.time);

    public void PerformedButton4(InputAction.CallbackContext context) => Add(InputType.Button4, context, (float)context.time);
    #endregion
}
