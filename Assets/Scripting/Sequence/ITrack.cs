public interface ITrack
{
    void Setup(IComponentMap map);

    void Scrub(int frames_elapsed);

    void Reset();
}
