using System.Collections.Generic;
using UnityEngine.InputSystem;

/// <summary>
/// A class that represents a buffer for player input. This buffer exhausts by popping a sorted queue of inputs to its listeners when they're
/// ready to consume input. It also decays input if it remains in the buffer for too long w/o being consumed. Hooks into the Performed event
/// of input.
/// </summary>
public class InputBuffer : IPerformedInputFeatures
{
    /// <summary>
    /// Default decay time is a 60th of a second i.e. inputs decay instantly.
    /// </summary>
    private const float ONE_SIXTIETH = 1f / 60f;

    /// <summary>
    /// Stores the input buffer w/ the associated context for ease of access.
    /// </summary>
    private readonly List<(InputType type, InputAction.CallbackContext data)> m_buffer;

    /// <summary>
    /// Stores the timestamps for every value in the buffer.
    /// INVARIANT: Each timestamp corresponds to an input in the buffer at the same index.
    /// Also implies they are always the same length.
    /// </summary>
    private readonly List<float> m_bufferTimestamps;

    /// <summary>
    /// A list of listeners for the buffer's input broadcast event.
    /// </summary>
    private readonly List<IInputListener> m_listeners;

    // b/c ImmutableDictionary doesn't exist in this .NET version.
    // note that this doesn't prevent mutation, only reassignment. Ah, well.
    /// <summary>
    /// A dictionary for mapping input types to their priorities when adding them to the buffer.
    /// Effectively immutable.
    /// </summary>
    private readonly Dictionary<InputType, int> m_priorityTable;

    /// <summary>
    /// The original expieration period for inputs, assigned upon construction.
    /// </summary>
    private readonly float m_originalExpirationPeriod;

    /// <summary>
    /// The current (mutable) expiration period. This is for the odd case where an action might need a larger
    /// buffer in order to improve game feel.
    /// </summary>
    private float m_expirationPeriod;

    /// <summary>
    /// Basic constructor that creates the buffer with the given priority and expiration period.
    /// </summary>
    /// <param name="listing_data"></param>
    /// <param name="expiration_period"></param>
    public InputBuffer(PriorityListingSO listing_data, float expiration_period = ONE_SIXTIETH)
    {
        m_buffer = new();
        m_bufferTimestamps = new();
        m_listeners = new();

        m_priorityTable = new Dictionary<InputType, int>();
        m_expirationPeriod = m_originalExpirationPeriod = expiration_period;

        // fill priority dictionary
        var items = listing_data.GetPriorities();
        foreach (InputType type in items) m_priorityTable.Add(type, listing_data.GetPriorityOf(type));
    }

    /// <summary>
    /// Resets the expiration period back to the value passed in at construction.
    /// </summary>
    public void ResetExpirationPeriod() => SetExpirationPeriod(m_originalExpirationPeriod);

    /// <summary>
    /// Sets the expiration period to an arbitrary time amount.
    /// </summary>
    /// <param name="time"></param>
    public void SetExpirationPeriod(float time) => m_expirationPeriod = time;

    /// <summary>
    /// Returns the current expiration period duration.
    /// </summary>
    /// <returns></returns>
    public float GetExpirationPeriod() => m_expirationPeriod;

    /// <summary>
    /// Subscribes the given listener to the event so that they receive input broadcasts.
    /// Will not add duplicates.
    /// </summary>
    /// <param name="listener"></param>
    public void Subscribe(IInputListener listener)
    {
        if (!m_listeners.Contains(listener)) m_listeners.Add(listener);
    }

    /// <summary>
    /// Removes the given listener from the event.
    /// </summary>
    /// <param name="listener"></param>
    public void Desubscribe(IInputListener listener) => m_listeners.Remove(listener);


    /// <summary>
    /// Is there nothing in the buffer?
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty() => m_buffer.Count == 0;

    /// <summary>
    /// Given input type, context, and a timestamp, adds the input instance to the buffer in proper priority order.
    /// To maintain invariant, adds timeStamp to the same index in the timestamp list.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="context"></param>
    /// <param name="timeStamp"></param>
    private void Add(InputType type, InputAction.CallbackContext context, float timeStamp)
    {
        // if empty, just add. There's no wrong position.
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

        index += offset ? 1 : 0; // needed in order to handle proper insertion

        m_buffer.Insert(index, (type, context));
        m_bufferTimestamps.Insert(index, timeStamp);
    }

    /// <summary>
    /// Runs a buffer cycle of attempting to broadcast input and decaying.
    /// No events occur if buffer is empty.
    /// </summary>
    /// <param name="timeStamp"></param>
    public void Cycle(float timeStamp)
    {
        if (IsEmpty()) return;

        TryBroadcast();
        Decay(timeStamp);
    }


    /// <summary>
    /// Filters the input buffer by input that has not decayed.
    /// </summary>
    /// <param name="timeStamp"></param>
    private void Decay(float timeStamp)
    {
        for (int index = m_buffer.Count - 1; index >= 0; --index)
        {
            if (timeStamp - m_bufferTimestamps[index] > m_expirationPeriod) Remove(index);
        }
    }

    /// <summary>
    /// Attempts to broadcast an input event from the front of the buffer. If not all listeners are ready,
    /// the event is not broadcast nor popped.
    /// </summary>
    private void TryBroadcast()
    {
        // if not ready to broadcast, stop
        if (!m_listeners.TrueForAll((IInputListener listener) => listener.IsReady())) return;

        var (type, context) = Pop();

        m_listeners.ForEach((IInputListener listener) => listener.Accept(type, context));
    }

    /// <summary>
    /// Removes the front input event and returns it.
    /// </summary>
    /// <returns></returns>
    private (InputType type, InputAction.CallbackContext context) Pop()
    {
        var cache = m_buffer[0];

        Remove(0);

        return cache;
    }

    /// <summary>
    /// Removes the input event at index, as well as the timestamp at the same index.
    /// </summary>
    /// <param name="index"></param>
    private void Remove(int index)
    {
        m_buffer.RemoveAt(index);
        m_bufferTimestamps.RemoveAt(index);
    }

    /// <summary>
    /// Helper method to compare two priorities by index. Returns true if the left is of a higher or equal priority than the right.
    /// </summary>
    /// <param name="one"></param>
    /// <param name="two"></param>
    /// <returns></returns>
    private bool IsOfHigherPriority(InputType one, InputType two) => m_priorityTable[one] >= m_priorityTable[two];

    /// <summary>
    /// All of these hooks perform the same; add their respective inputtype to the buffer.
    /// The casting of double to float is concerning, but as long as no one plays for several days straight I should be fine.
    /// </summary>
    /// <param name="context"></param>
    #region IPerformedInputFeatures
    public void PerformedMove(InputAction.CallbackContext context) => Add(InputType.Movement, context, (float)context.time);

    public void PerformedJump(InputAction.CallbackContext context) => Add(InputType.Jump, context, (float)context.time);

    public void PerformedButton1(InputAction.CallbackContext context) => Add(InputType.Button1, context, (float)context.time);

    public void PerformedButton2(InputAction.CallbackContext context) => Add(InputType.Button2, context, (float)context.time);

    public void PerformedButton3(InputAction.CallbackContext context) => Add(InputType.Button3, context, (float)context.time);

    public void PerformedButton4(InputAction.CallbackContext context) => Add(InputType.Button4, context, (float)context.time);
    #endregion
}
