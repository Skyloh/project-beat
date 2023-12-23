public interface ITrack
{
    void Setup(IComponentMap map);

    void ScrubForward(int frames_elapsed);

    void JumpBack(int frame);

    void Reset();
}
