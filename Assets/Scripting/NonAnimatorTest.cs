using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A test script to see if we can use serialized references to anim states.
/// </summary>
public class NonAnimatorTest : MonoBehaviour
{
    [SerializeField] private List<SampleScript> references;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var reference in references)
        {
            //Debug.Log(reference.GetDebug());
        }
    }
}
