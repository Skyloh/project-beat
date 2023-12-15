using UnityEngine;

/// <summary>
/// A test StateMachineBehaviour to see if using states is a good way to handle SSOs.
/// </summary>
public class SampleScript : StateMachineBehaviour
{
    [SerializeField] private string m_debug;

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log($"{m_debug} w/ elapsed time {stateInfo.normalizedTime}.");
    }

    public string GetDebug()
    {
        return name + ' ' + m_debug;
    }
}
