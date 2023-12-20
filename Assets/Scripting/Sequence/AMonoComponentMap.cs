using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AMonoComponentMap : MonoBehaviour, IComponentMap
{
    [SerializeField] private List<SerializeableKVPair<ComponentType, List<MonoBehaviour>>> m_serializedMap;
    private Dictionary<ComponentType, List<MonoBehaviour>> m_map;
    // private Dictionary<Type, List<MonoBehaviour>> m_map;

    void Awake()
    {
        // This code is a relic from when I used a dictionary with keys as raw types. This led to issues with polymorphism
        // w/ AMonoHitboxEvaluator. I did away with the types, but that makes the cast "look" more risky. In reality, it won't break.
        //
        // m_map = new Dictionary<Type, List<MonoBehaviour>>();

        // not very pretty, but 'tis the danger of dealing with literal types.
        // hey, if they didn't want us to use them they woulda not had them :>
        // foreach (var pair in m_serializedMap) m_map.Add(ComponentTypeHelpers.ConvertToType(pair.Key), pair.Value);

        m_map = new Dictionary<ComponentType, List<MonoBehaviour>>();
        foreach (var pair in m_serializedMap) m_map.Add(pair.Key, pair.Value);

        Init();
    }

    protected virtual void Init()
    {
        // pass
    }

    public List<TComponent> GetReference<TComponent>(ComponentType type) where TComponent : MonoBehaviour
    {
        if (m_map.TryGetValue(type, out List<MonoBehaviour> value))
        {
            return value.ConvertAll<TComponent>((MonoBehaviour m) => (TComponent)m); // very scary cast :(
        }

        return default;
    }
}
