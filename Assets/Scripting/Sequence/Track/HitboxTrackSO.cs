using UnityEngine;

[CreateAssetMenu(fileName = "HitboxTrack", menuName = "ScriptableObjects/Track/Hitbox", order = 1)]
public class HitboxTrackSO : TrackSO<Hitbox, AMonoHitboxEvaluator> 
{
    public override ComponentType GetComponentType() => ComponentType.HitboxVisualizer;
}
