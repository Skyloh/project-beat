using System;

[Serializable]
public struct LoopMarker
{
    public int gotoFrame;
    public Conditional[] conditionals;
    public int maxLoopCount;

    [Serializable]
    public struct Conditional
    {
        public PredicateSO predicate;
        public bool not;
    }
}
