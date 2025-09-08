    namespace Ang7.Helpers.VideoPlayerControls;
    public class VideoPositionEventArgs : EventArgs
    {
        public TimeSpan Position { get; private set; }

        public VideoPositionEventArgs(TimeSpan position)
        {
            Position = position;
        }
    }
