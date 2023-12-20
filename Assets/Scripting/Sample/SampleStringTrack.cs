using UnityEngine;

[CreateAssetMenu(fileName = "StringTrack", menuName = "ScriptableObjects/Track/String", order = 1)]
public class SampleStringTrack : TrackSO<string, SampleEvaluator>
{
    protected override ComponentType GetComponentType() => ComponentType.Sample;
}
