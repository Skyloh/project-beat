using System;

// I coulda done this with reflection, but I've committed enough OoD sins for today.
public enum ComponentType
{
    Sample,
    HitboxVisualizer,
    LoopEvaluator,
    InputController
}

public static class ComponentTypeHelpers
{
    public static Type ConvertToType(ComponentType type)
    {
        return type switch
        {
            ComponentType.Sample => typeof(SampleEvaluator),
            ComponentType.HitboxVisualizer => typeof(MonoHitboxVisualizer),
            ComponentType.LoopEvaluator => typeof(MonoConditionalEvaluator),
            ComponentType.InputController => typeof(MonoInputController),
            _ => throw new ArgumentException($"Type {type} has not been defined!")
        };
    }
}