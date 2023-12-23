using UnityEngine;

[SharedBetweenAnimators]
public class SequenceBehaviour : StateMachineBehaviour
{
    [SerializeField] private SequenceSO m_sequence;

    private AnimationClip m_clip;
    private bool m_entered;

    /// <summary>
    /// What number loop are we on? Only applies to looping animations.
    /// </summary>
    private int m_iterations;

    public void SetupSequence(IComponentMap map)
    {
        m_entered = false; // this is needed bc apparently setting the default value to false still doesn't work.
        m_sequence.SetupAll(map);
    }

    // I'm not exposing an entire sequence for a single method, nor am I serializing a reference to a sequence in a track's data block
    // that is contained within that sequence. I could add a Decorator to the emission in TrackSO, but that requires changing access types
    // on the class which is overkill and will look disgusting (since it's only used by one other class instance). I think this is the lesser
    // of two pretty annoying evils.
    public void LoopBackIfActive(int frames)
    {
        if (m_entered) m_sequence.LoopBack(frames); // if we're the active state and we receive the signal to loop back, do so.
    }

    // Called the first frame the state is evaluated
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // guard clause to prevent same-frame entering
        // apparently, a state can be entered multiple times on the same frame. I have no idea why.
        if (m_entered) return;
        m_entered = true;

        if (m_clip == null) m_clip = animator.GetCurrentAnimatorClipInfo(layerIndex)[0].clip;

        m_iterations = 0;

        m_sequence.ResetSequence();
        // m_sequence.Tick(0); // force evaluation of first frame (frame 0) upon state entry.
    }

    // Called on every Update frame excluding the first and last
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        /*
         * CeilToInt Version.
         * Not deleted in case the "skipping frames" issue comes up again.
         * This variant worked aside from the fact that it ticked frames one in advance to the ceil.
         * 
        // Debug.Log(stateInfo.normalizedTime + " " + m_iterations);
        // if we've wrapped past the end of the clip, reset
        if (stateInfo.normalizedTime > (m_iterations + 1))
        {
            // Debug.Log($"state time: {stateInfo.normalizedTime} iters: {m_iterations}");
            ++m_iterations;

            m_sequence.ResetSequence();
            m_sequence.Tick(0);

            // do not evaluate the current seq frame this Update cycle, which would always be >=1
            // this is bc the normalizedTime mod 1 ceiled is always non-0.
            // If I left this as is (w/o return), the 0th frame would always be evaluated on the
            // same cycle as the 1st frame on ANY MACHINE regardless of lag spikes or not.
            // With this return, I'm ensuring that the 0th frame gets its own cycle, always.
            // Future keyframes might get scrubbed past by lag spikes, but that's fine.
            return;
        }

        int current_frame = ConvertToFrames(stateInfo.normalizedTime);

        m_sequence.Tick(current_frame);
        */

        if (stateInfo.normalizedTime > (m_iterations + 1))
        {
            ++m_iterations;

            m_sequence.Progress(Mathf.CeilToInt(m_clip.length * m_clip.frameRate));
            m_sequence.ResetSequence();

            // see the commented-out version for similar logic on why this returns.
            // NOTE: this might result in skipped/inconsistent frames, so keep an eye on it.
            return;
        }

        int current_frame = ConvertToFrames(stateInfo.normalizedTime);

        m_sequence.Progress(current_frame);
    }

    // Add some sort of cleanup method to this exit.
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_sequence.ResetSequence(); // TODO is this right?
        m_entered = false;
    }

    private int ConvertToFrames(float time) => Mathf.FloorToInt(time % 1 * m_clip.length * m_clip.frameRate);
}
