using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for a script that acts as a component dictionary so Tracks/Sequences can set themselves up.
/// </summary>
public interface IComponentMap
{
    /// <summary>
    /// Looks up the component by type for the reference and returns all of them.
    /// This is basically the observer pattern.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    /// <returns></returns>
    List<TComponent> GetReference<TComponent>(ComponentType type) where TComponent : MonoBehaviour; // type sourced from "typeof(TComponent)"
}
