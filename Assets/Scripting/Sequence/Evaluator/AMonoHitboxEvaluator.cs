using System.Collections.Generic;
using UnityEngine;

public class AMonoHitboxEvaluator : MonoBehaviour, IEvaluator<Hitbox>
{
    protected List<Hitbox> m_activeHitboxes;

    void Awake() => m_activeHitboxes = new List<Hitbox>();

    public void Evaluate(Hitbox item)
    {
        if (item.IsDisabler()) m_activeHitboxes = item.FilterHitboxes(m_activeHitboxes);
        else m_activeHitboxes.Add(item);
    }
}
