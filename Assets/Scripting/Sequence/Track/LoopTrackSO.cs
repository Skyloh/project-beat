public class LoopTrackSO : TrackSO<LoopMarker, MonoInputConditionalEvaluator>
{
    protected override ComponentType GetComponentType()
    {
        return ComponentType.LoopEvaluator;
    }
}
