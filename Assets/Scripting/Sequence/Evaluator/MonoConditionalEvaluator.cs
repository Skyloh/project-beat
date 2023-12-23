using System;
using TNRD;
using UnityEngine;

public class MonoConditionalEvaluator : MonoBehaviour, IEvaluator<LoopMarker>
{
    [SerializeField] private Animator m_animator;
    [SerializeField] private SerializableInterface<IComponentMap> m_map;

    private Action<int>[] m_jumpCallbacks; // not a great solution, but the only one i got

    private int m_previousHash;
    private int m_previousIterations;

    private void Awake()
    {
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
        // for counting the number of times we've evaluated this loop marker for this player.
        // if they go beyond the max for the marker, skip evaluation.
        int hash = value.GetHashCode();
        if (m_previousHash == hash && m_previousIterations >= value.maxLoopCount) return;
        ++m_previousIterations;
        m_previousHash = hash;
        
        foreach (var item in value.conditionals)
        {
            item.predicate.Retrieve(m_map.Value);
            if (item.predicate.Check() == item.not) return; // c = T/F, n = T/F -> true, break, c = F/T, n = T/F -> false, continue
        }

        // time it takes for a frame to elapse
        var clip = m_animator.GetCurrentAnimatorClipInfo(0)[0].clip;

        // have to use this instead of a parameter on motion time bc adding a param prevents the animator from updating the field itself
        m_animator.Play(clip.name, 0, value.gotoFrame * (1 / clip.frameRate)); // restarts the clip at the loop point
        InvokeCallbacks(value.gotoFrame); // manually perform JumpBack
    }

    private void InvokeCallbacks(int frames)
    {
        foreach (var action in m_jumpCallbacks) action.Invoke(frames);
    }


}
