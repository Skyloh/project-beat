using System.Collections.Generic;
using UnityEngine;
using TNRD;
using System;

/// <summary>
/// The monobehaviour script for handling input. Links several raw C# classes together.
/// Primarily handles the coupling of classes and the Cycle invokations of the buffer.
/// </summary>
public class MonoInputController : MonoBehaviour
{
    /// <summary>
    /// A list of input listeners to register to the buffer upon awake.
    /// </summary>
    [SerializeField] private List<SerializableInterface<IInputListener>> m_listeners;

    /// <summary>
    /// The serialized priority listing to source priority orders from for the buffer.
    /// </summary>
    [SerializeField] private PriorityListingSO m_priorityListing;

    /// <summary>
    /// The default input expiration time passed to the buffer's constructor.
    /// </summary>
    [SerializeField] private float m_defaultExpirationTime;

    /// <summary>
    /// Instance of the input mapping used to register held inputs.
    /// </summary>
    private InputMapping m_mapping;

    /// <summary>
    /// The buffer used to store and broadcast input events.
    /// </summary>
    private InputBuffer m_buffer; // potential site of abstraction: generalize to be an "input source" rather than concrete input buffer
    
    /// <summary>
    /// The control handler used to register events to input delegates.
    /// </summary>
    private InputControl m_controls;

    /// <summary>
    /// Creates class instances and couples them to the controls.
    /// </summary>
    private void Awake()
    {
        m_buffer = new InputBuffer(m_priorityListing, m_defaultExpirationTime);
        m_mapping = new InputMapping();
        m_controls = new InputControl();

        m_controls.SubscribePerformed(m_buffer);
        m_controls.SubscribePerformed(m_mapping);
        m_controls.SubscribeCanceled(m_mapping);

        m_listeners.ForEach((SerializableInterface<IInputListener> item) => SubscribeToBuffer(item.Value));
    }

    // Handles toggling of the Controls instance.
    private void OnEnable() => m_controls.Instance.Enable();
    private void OnDisable() => m_controls.Instance.Disable();

    /// <summary>
    /// Probably not explicitly necessary, but I won't toy with possible memory leaks.
    /// </summary>
    private void OnDestroy()
    {
        m_controls.DesubscribePerformed(m_buffer);
        m_controls.DesubscribePerformed(m_mapping);
        m_controls.DesubscribeCanceled(m_mapping);

        m_listeners.ForEach((SerializableInterface<IInputListener> item) => DesubscribeFromBuffer(item.Value));
    }

    /// <summary>
    /// Manually cycles the buffer.
    /// </summary>
    private void Update()
    {
        // a coroutine that just invoked this method after waiting a frame is just an Update cycle :/
        m_buffer.Cycle(Time.realtimeSinceStartup);
    }

    public void SubscribeToBuffer(IInputListener listener) => m_buffer.Subscribe(listener);
    public void DesubscribeFromBuffer(IInputListener listener) => m_buffer.Desubscribe(listener);

    /// <summary>
    /// Returns true if the given input is held.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool IsInputHeld(InputType type) => m_mapping.IsInputHeld(type);

    public Func<InputType, bool> GetMappingCallback() => m_mapping.IsInputHeld;
}