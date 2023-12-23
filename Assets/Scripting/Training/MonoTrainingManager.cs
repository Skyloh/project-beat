using TMPro;
using TNRD;
using UnityEngine;
using UnityEngine.UI;

// Used to set up training.
// You have to manually set the clip and sequence due to the fact that you can't do this from code w/o editor scripts.
// I don't want this to be an editor-only thing :/
public class MonoTrainingManager : MonoBehaviour
{
    private const string STATE_NAME = "playback";

    [SerializeField] private Animator m_animator;
    [SerializeField] private SerializableInterface<IComponentMap> m_map;

    [SerializeField] private Slider m_scrubSlider;
    [SerializeField] private TextMeshProUGUI m_text;

    [Header("Data")]
    [SerializeField] private AnimationClip m_clip;
    [SerializeField] private SequenceSO m_sequence;

    private int m_hash;

    private void Start()
    {
        m_animator.enabled = false;
        m_hash = Animator.StringToHash(STATE_NAME);

        string cur_name = m_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;

        if (cur_name != m_clip.name)
        {
            Debug.LogError($"Playback clip {cur_name} does not match training clip {m_clip.name}!");
        }

        m_scrubSlider.maxValue = Mathf.FloorToInt(m_clip.length * m_clip.frameRate);

        m_sequence.SetupAll(m_map.Value);
        m_sequence.ResetSequence();

        UpdatePlayback(0);
    }

    private void UpdatePlayback(int frame)
    {
        m_sequence.ResetSequence();

        m_animator.Play(m_hash, 0, frame / (m_clip.length * m_clip.frameRate) + 0.01f); // minor offset to fix inexact frame issue
        m_animator.Update(0f); // force an animator update

        m_sequence.Progress(frame);
    }

    public void ScrubAnimation()
    {
        int frame = (int)m_scrubSlider.value;

        m_text.text = $"{frame}";

        UpdatePlayback(frame);
    }

}
