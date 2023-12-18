using System.Collections.Generic;
using UnityEngine;

public abstract class TrackSO<TValue, TComponent> : ScriptableObject, ITrack where TComponent : MonoBehaviour, IEvaluator<TValue>
{
    // frames - value
    [SerializeField] private List<SerializeableKVPair<int, TValue>> m_keyframes;

    private List<TComponent> m_listeners;

    private int m_index;

    public void Reset() => m_index = 0;

    public void Scrub(int frames_elapsed)
    {
        // while instead of if here because it catches the case where multiple keyframes are passed after, say, a lag spike.
        while (m_index < m_keyframes.Count && m_keyframes[m_index].Key <= frames_elapsed)
        {
            m_listeners.ForEach((TComponent eval) => eval.Evaluate(m_keyframes[m_index].Value));

            ++m_index;
        }
    }

    public void Setup(IComponentMap map) => m_listeners = map.GetReference<TComponent>();
}
