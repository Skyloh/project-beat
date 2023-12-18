using UnityEngine;

public class SequenceBehaviour : StateMachineBehaviour
{
    [SerializeField] private SequenceSO m_sequence;

    private float m_initialTime;
    private int m_initialFrame;
    private AnimationClip m_clip;

    public void SetupSequence(IComponentMap map) => m_sequence.SetupAll(map);

    // Called the first frame the state is evaluated
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_clip == null) m_clip = animator.GetCurrentAnimatorClipInfo(layerIndex)[0].clip;

        m_initialTime = Time.realtimeSinceStartup;
        m_initialFrame = Time.frameCount;
        m_sequence.ResetSequence();
        m_sequence.Tick(0);
    }

    // Called on every Update frame excluding the first and last
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // TODO Fix this.
        // It's printing out sets of 4 at once, which is most certainly wrong.
        // Note: elapsed frames is not in the same timescale as Update, maybe I should just use the TimeToFrames instead of frameCount?
        
        float time = Time.realtimeSinceStartup;
        float elapsed = time - m_initialTime;

        int frame_time = Time.frameCount;
        int elapsed_frames = frame_time - m_initialFrame;
        
        // if we've gone over the clip length, reset
        if (elapsed >= stateInfo.length)
        {
            // start index at 0
            m_sequence.ResetSequence();

            // find the "current" framecount
            elapsed_frames = TimeToFrames(elapsed);// goes beyond the clip bounds to evaluate all trailing keys
            elapsed %= stateInfo.length; // wraparound so we see how far into the next cycle we've already advanced by this point

            m_sequence.Tick(elapsed_frames); // evaluate beyond frames
            elapsed_frames = TimeToFrames(elapsed); // update elapsed frames so we can tick the keys we've advanced past
            m_sequence.Tick(elapsed_frames); // evaluate early frames 

            // update initials so we don't fire this forever
            m_initialTime = time - elapsed;
            m_initialFrame = frame_time - elapsed_frames;
        }
        else
        {
            m_sequence.Tick(elapsed_frames);
        }
    }

    private int TimeToFrames(float time) => Mathf.CeilToInt(time * m_clip.frameRate);

    /*
    // Called the last frame the state is evaluated before transition
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
    */
}
