using System;

[Serializable]
public struct LoopMarker
{
    public int frame;
    public Conditional[] conditionals;
    public int maxLoopCount;

    [Serializable]
    public struct Conditional
    {
        // I was considering making these tagged-structs (representing conditionals) by means of an enum, but that would lead to
        // fields being sometimes useless (like for a resource-conditional, input is meaningless). That's stupid and bad.
        public InputType heldType; // TODO currently hardcoded to input conditionals, but should be abstracted
        public bool whileHeld;
    }
}
