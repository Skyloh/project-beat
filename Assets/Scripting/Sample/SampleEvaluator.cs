using UnityEngine;

public class SampleEvaluator : MonoBehaviour, IEvaluator<string>
{
    public void Evaluate(string value)
    {
        Debug.Log("Received " + value);
    }
}
