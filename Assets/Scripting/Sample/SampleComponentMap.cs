using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SampleComponentMap : MonoBehaviour, IComponentMap
{
    [SerializeField] private List<SerializeableKVPair<ComponentType, List<MonoBehaviour>>> m_serializedMap;
    private Dictionary<Type, List<MonoBehaviour>> m_map;

    Animator temp;
    void Awake()
    {
        m_map = new Dictionary<Type, List<MonoBehaviour>>();

        // not very pretty, but 'tis the danger of dealing with literal types.
        // hey, if they didn't want us to use them they woulda not had them :>
        foreach (var pair in m_serializedMap)
        {
            switch (pair.Key)
            {
                case ComponentType.Sample:
                    m_map.Add(typeof(SampleEvaluator), pair.Value);
                    break;

                default:
                    break;
            }
        }

        Animator animator = GetComponent<Animator>();
        temp = animator;
        var behaviours = animator.GetBehaviours<SequenceBehaviour>();
        Array.ForEach<SequenceBehaviour>(behaviours, (SequenceBehaviour behaviour) => behaviour.SetupSequence(this));
    }

    public List<TComponent> GetReference<TComponent>() where TComponent : MonoBehaviour
    {
        if (m_map.TryGetValue(typeof(TComponent), out List<MonoBehaviour> value))
        {
            return value.ConvertAll<TComponent>((MonoBehaviour m) => (TComponent)m); // very scary cast :(
        }

        return default;
    }

    void Start()
    {
        //InvokeRepeating(nameof(Test), 0f, 1f);
    }
    void Test()
    {
        temp.Update(1f/12f);
    }
}
