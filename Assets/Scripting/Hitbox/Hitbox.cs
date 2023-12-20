using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Hitbox // stub implementation; needs abstraction into an interface?
{
    [SerializeField] private int m_id;

    [Space(10)]

    [SerializeField] private bool m_worldSpaceHitbox = false;
    [SerializeField] private int m_priority;
    [SerializeField] private Vector3 m_posOffset;
    [SerializeField] private Vector3 m_bounds; // full-length units, not half

    public Hitbox(Vector3 pos, Vector3 bounds)
    {
        m_posOffset = pos;
        m_bounds = bounds;
    }

    /// <summary>
    /// Filters (w/o mutation) the input list of hitboxes and returns a list of items that do not match this hitbox's ID.
    /// </summary>
    /// <param name="hitboxes"></param>
    /// <returns></returns>
    public List<Hitbox> FilterHitboxes(List<Hitbox> hitboxes) => hitboxes.Where((Hitbox h) => h.m_id != this.m_id).ToList();

    /// <summary>
    /// Returns true if this hitbox event is meant to disable the hitboxes of the same ID.
    /// </summary>
    /// <returns></returns>
    public bool IsDisabler() => m_bounds == Vector3.zero || m_posOffset == Vector3.zero;

    // TODO change this to NonAlloc for optimization purposes. At the start of a game, change the value of a static variable that states the upper-bound
    // of hittable things in a single game. E.g. for 2 players it's 2, but keep in mind that projectiles might exist at some point.
    public GameObject[] GetOverlap(Vector3 position) 
        => Array.ConvertAll<Collider, GameObject>(
            Physics.OverlapBox(GetPosition(position), m_bounds / 2f), 
            (Collider c) => c.gameObject);

    public Vector3 GetPosition(Vector3 position) => m_worldSpaceHitbox ? m_posOffset : m_posOffset + position;
    public Vector3 GetBounds() => m_bounds;
    public int GetID() => m_id;
    public int GetPriority() => m_priority;
}
