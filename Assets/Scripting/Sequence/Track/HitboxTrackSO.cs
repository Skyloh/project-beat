using UnityEngine;

[CreateAssetMenu(fileName = "HitboxTrack", menuName = "ScriptableObjects/Track/Hitbox", order = 1)]
public class HitboxTrackSO : TrackSO<Hitbox, AMonoHitboxEvaluator> 
{
    protected override ComponentType GetComponentType() => ComponentType.HitboxVisualizer;
}
