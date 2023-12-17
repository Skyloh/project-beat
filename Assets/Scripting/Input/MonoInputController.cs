using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MonoInputController : MonoBehaviour
{
    [SerializeField] private PriorityListingSO m_priorityListing;
    [SerializeField] private float m_defaultExpirationTime;

    private InputMapping m_mapping;
    private InputBuffer m_buffer;

    private float m_timeElapsed;

    private void Awake()
    {
        // next steps:
        // create system to hook up the IInput Features interfaces into Controls
        // implement IEBroadcast w/ cycle buffer calls
        // add means to subscribe to buffer
        m_buffer = new InputBuffer(m_priorityListing, m_defaultExpirationTime);
        m_mapping = new InputMapping();
    }


    private IEnumerator IEBroadcast()
    {
        // TODO
        yield return null;
    }

    public bool IsInputHeld(InputType type) => m_mapping.IsInputHeld(type);
}

/*
 * Input / Input Buffer Wishlist:
 * 
 * an input buffer for every player
 * emitting signals about buffered inputs to listeners
 * means to externally extend extinction window of inputs
 *  e.g. if a move has a long end period, we should be able to make an event flag that extends extinction period for any input registered in it
 *  player-unique, ofc
 * custom yield instruction to wait for input/while input held/etc.
 * 
 */ 
