using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class TrackSO<TValue, TComponent> : ScriptableObject, ITrack where TComponent : MonoBehaviour, IEvaluator<TValue>
{
    // frames - value
    [SerializeField] private List<SerializeableKVPair<int, TValue>> m_keyframes;

    private List<TComponent> m_listeners;

    private int m_index;

    public void Reset() => m_index = 0;

    /// <summary>
    /// Direction is implied here bc they only work 1-way. If I wanted them to work bidirectionally (which I don't see a reason to), I would need
    /// to figure out how to send "undo" events. Probably possible to do neatly, but not a focus.
    /// </summary>
    /// <param name="frames_elapsed"></param>
    public void ScrubForward(int frames_elapsed)
    {
        // while instead of if here because it catches the case where multiple keyframes are passed after, say, a lag spike.
        while (m_index < m_keyframes.Count && m_keyframes[m_index].Key <= frames_elapsed) // TODO is the eq part correct?
        {
            m_listeners.ForEach((TComponent eval) => eval.Evaluate(m_keyframes[m_index].Value));

            ++m_index;
        }
    }

    /// <summary>
    /// Unlike Scrub, JumpBack moves directly to a prior frame without (un)evaluating intermittent keyframes
    /// </summary>
    /// <param name="frame"></param>
    public void JumpBack(int frame)
    {
        while (m_index > 0 && m_keyframes[m_index].Key >= frame) // see TODO above. Is the >0 here correct?
        {
            --m_index;
        }
    }

    public void Setup(IComponentMap map)
    {
        m_keyframes.Sort((SerializeableKVPair<int, TValue> v1, SerializeableKVPair<int, TValue> v2) => v1.Key - v2.Key);
        m_listeners = map.GetReference<TComponent>(GetComponentType());

        PostSetup();
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        m_keyframes.ForEach(h => builder.Append($"{h.Key}:{h.Value}\n"));
        return builder.ToString();
    }

    protected virtual void PostSetup() 
    { 
        // pass
    }

    protected abstract ComponentType GetComponentType();
}
