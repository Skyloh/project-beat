using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MonoInputController : MonoBehaviour
{
    public delegate void InputEvent(InputType type, InputAction.CallbackContext context);
    private event InputEvent OnBroadcastInput;

    [SerializeField] private PriorityListingSO m_priorityListing;
    [SerializeField] private float m_defaultExpirationTime;

    private InputMapping m_mapping;
    private InputBuffer m_buffer;

    private float m_timeElapsed;

    private void Awake()
    {
        m_buffer = new InputBuffer(m_priorityListing, m_defaultExpirationTime);
        m_mapping = new InputMapping(new Controls(), m_buffer.Push);
    }

    private void OnEnable() => m_mapping.Register();

    private void OnDisable() => m_mapping.Deregister();


    private IEnumerator IEBroadcast()
    {
        // TODO start here:
        // I think i need to rethink what i've been doing.
        // How does this work as an input buffer? An input buffer is not just a perma-building stack of inputs that gets popped through.
        // Some inputs never reach the front, "expiring" before they do. Where is that behavior in this? There is no way to remove just an
        // item from a priority queue, currently.
        //
        // or maybe it is just that. If I press A, then B just before the attack comes out, it should remain in the buffer until consumed.
        // Now what does that mean? "Consumed" implies that other things pull from the buffer when they can (the buffer doesn't manually emit)
        // OR the buffer emits signals when allowed.
        // In this case, the manual decay works as an "auto-popper". If nothing consumes the input before the decay time is up, the input vanishes.
        //
        // thoughts: mash a ton of inputs, then do a quick attack
        // the quick attack will decay some inputs one-by-one (since we can only pop the first), but some will be left over and then consumed. Rinse
        // and repeat until all inputs are decayed. This is wrong behavior, no? What should happen is that all inputs have their own decay timers rather
        // than decaying in line. This requires an overhaul of the implementation.
        yield return null;
    }


    public void AddListener(IInputListener listener) => OnBroadcastInput += listener.HandleInput;
    public void RemoveListener(IInputListener listener) => OnBroadcastInput -= listener.HandleInput;

    public bool IsInputHeld(InputType type) => m_mapping.IsInputHeld(type);
}

/*
 * Input / Input Buffer Wishlist:
 * 
 * an input buffer for every player
 * emitting signals about buffered inputs to listeners
 * priority queue of inputs (w/ an SO to store priority lists of inputs, so it can be unique per character)
 * means to externally extend extinction window of inputs
 *  e.g. if a move has a long end period, we should be able to make an event flag that extends extinction period for any input registered in it
 *  player-unique, ofc
 * custom yield instruction to wait for input/while input held/etc.
 * 
 * 
 */ 
