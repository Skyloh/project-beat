using System.Collections.Generic;
using UnityEngine;

public class MonoHitboxVisualizer : AMonoHitboxEvaluator
{
    [SerializeField] private Material m_hitboxMaterial;
    [SerializeField] private Transform m_playbackTransform;

    private List<int> m_visualHitboxIDs = new List<int>();
    private List<Transform> m_visualHitboxes = new List<Transform>();

    /// <summary>
    /// A little scuffed, using the event execution order to ensure code works, but hey. This is an editor script :)
    /// </summary>
    public void OnScrub()
    {
        foreach (var t in m_visualHitboxes)
        {
            Destroy(t.gameObject);
        }

        m_activeHitboxes.Clear();
        m_visualHitboxes.Clear();
        m_visualHitboxIDs.Clear();
    }

    public void PostScrub()
    {
        foreach (Hitbox h in m_activeHitboxes)
        {
            if (!m_visualHitboxIDs.Contains(h.GetID()))
            {
                GameObject o = GameObject.CreatePrimitive(PrimitiveType.Cube);
                o.GetComponent<MeshRenderer>().sharedMaterial = m_hitboxMaterial;

                o.name = $"Hitbox {h.GetID()}";

                o.transform.position = h.GetPosition(m_playbackTransform.position);
                o.transform.localScale = h.GetBounds();

                m_visualHitboxes.Add(o.transform);
                m_visualHitboxIDs.Add(h.GetID());
            }
        }
    }

}
