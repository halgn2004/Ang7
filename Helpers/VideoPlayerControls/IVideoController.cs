    namespace Ang7.Helpers.VideoPlayerControls;
    public interface IVideoController
    {
        VideoStatus Status { get; set; }
        TimeSpan Duration { get; set; }
    }