using System;
using UnityEngine;

[RequireComponent(typeof(MonoInputController))] // TODO fix. very bad; promotes super-tight coupling
public class MonoInputConditionalEvaluator : MonoBehaviour, IEvaluator<LoopMarker>
{
    [SerializeField] private Animator m_animator;

    private Func<InputType, bool> m_mappingCallback; // TODO abstract into "conditionals". Tried using regular expressions, but that's too open-ended.
    private Action<int>[] m_jumpCallbacks; // not a great solution, but the only one i got

    private int m_previousHash;
    private int m_previousIterations;

    private void Awake()
    {
        // this makes me sad
        var mono_input = GetComponent<MonoInputController>();
        m_mappingCallback = mono_input.GetMappingCallback();

        var behaviours = m_animator.GetBehaviours<SequenceBehaviour>();
        m_jumpCallbacks = new Action<int>[behaviours.Length];

        int index = 0;
        foreach (var item in behaviours)
        {
            m_jumpCallbacks[index] = item.LoopBackIfActive;
            ++index;
        }
    }

    public void Evaluate(LoopMarker value)
    {
        int hash = value.GetHashCode();
        if (m_previousHash == hash && m_previousIterations >= value.maxLoopCount) return;
        ++m_previousIterations;
        m_previousHash = hash;

        foreach (var item in value.conditionals)
        {
            bool is_input_held = m_mappingCallback(item.heldType);

            if (is_input_held != item.whileHeld)
            {
                return;
            }
        }

        // time it takes for a frame to elapse
        var clip = m_animator.GetCurrentAnimatorClipInfo(0)[0].clip;

        // have to use this instead of a parameter on motion time bc adding a param prevents the animator from updating the field itself
        m_animator.Play(clip.name, 0, value.frame * (1 / clip.frameRate)); // restarts the clip at the loop point
        InvokeCallbacks(value.frame); // manually perform JumpBack
    }

    private void InvokeCallbacks(int frames)
    {
        foreach (var action in m_jumpCallbacks) action.Invoke(frames);
    }


}
