using System.Collections.Generic;
using TNRD;
using UnityEngine;

[CreateAssetMenu(fileName = "Sequence", menuName = "ScriptableObjects/Sequence", order = 1)]
public class SequenceSO : ScriptableObject
{
    [SerializeField] private List<SerializableInterface<ITrack>> m_tracks;

    public void SetupAll(IComponentMap map) => m_tracks.ForEach((SerializableInterface<ITrack> track) => track.Value.Setup(map));

    public void ResetSequence() => m_tracks.ForEach((SerializableInterface<ITrack> track) => track.Value.Reset());
    
    public void Progress(int frames_elapsed) => m_tracks.ForEach((SerializableInterface<ITrack> track) => track.Value.ScrubForward(frames_elapsed));

    public void LoopBack(int to_frame) => m_tracks.ForEach((SerializableInterface<ITrack> track) => track.Value.JumpBack(to_frame));

    // public List<ITrack> GetTracks() => m_tracks.ConvertAll<ITrack>(i => i.Value);
}
