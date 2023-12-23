using UnityEngine;

[CreateAssetMenu(fileName = "LoopTrack", menuName = "ScriptableObjects/Track/Loop", order = 1)]
public class LoopTrackSO : TrackSO<LoopMarker, MonoConditionalEvaluator>
{
    public override ComponentType GetComponentType() => ComponentType.LoopEvaluator;
}
