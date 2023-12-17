using UnityEngine;
using System;

/// <summary>
/// Yields while a given input type is being held down.
/// </summary>
public class WaitWhileHeld : CustomYieldInstruction
{
    private InputType m_inputType;
    private Func<InputType, bool> m_pred;

    /// <summary>
    /// The predicate supplied should come from either MonoInputController or InputMapping (former being the easier to get).
    /// </summary>
    /// <param name="input"></param>
    /// <param name="pred"></param>
    public WaitWhileHeld(InputType input, Func<InputType, bool> pred)
    {
        m_inputType = input;
        m_pred = pred;
    }

    public override bool keepWaiting => m_pred(m_inputType);
}
