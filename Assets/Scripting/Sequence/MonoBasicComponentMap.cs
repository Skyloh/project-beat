using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MonoBasicComponentMap : AMonoComponentMap
{
    protected override void Init()
    {
        Animator animator = GetComponent<Animator>();
        var behaviours = animator.GetBehaviours<SequenceBehaviour>();
        Array.ForEach<SequenceBehaviour>(behaviours, (SequenceBehaviour behaviour) => behaviour.SetupSequence(this));
    }
}
