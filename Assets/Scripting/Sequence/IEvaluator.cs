/// <summary>
/// Interface for components that take in values from Tracks/Sequences and process them.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public interface IEvaluator<TValue>
{
    void Evaluate(TValue value);
}
