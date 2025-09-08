using SkiaSharp;
using SkiaSharp.Views.Maui;
namespace Ang7.Views
{
    public partial class SS : ContentPage
    {
        private SKBitmap[] gifFrames;
        private int currentFrame;
        //private bool isAnimating;
        private NetworkAccess current;
        private IDispatcherTimer animationTimer;
        private string gifimage = DeviceInfo.Idiom==DeviceIdiom.Tablet ? "splashscreen960.gif" : "splashscreen480.gif";

        public SS()
        {
            InitializeComponent();
            current = Connectivity.Current.NetworkAccess;
            StartInitialization();
        }
        private async void StartInitialization()
        {
            var today = DateTime.Today;
            bool isSupportedVersion = Helpers.Settings.IsTheAppUpdated;

            if (current == NetworkAccess.Internet && Helpers.Settings.LastUpdateDate.Date != today)
            {
                try {
                    isSupportedVersion = await Validate.AppVersion();
                    Helpers.Settings.IsTheAppUpdated = isSupportedVersion;
                }
                catch
                {
                    //ThisIsAgoodVersion();
                }
            }

            if (isSupportedVersion)
            {
                ThisIsAgoodVersion();
            }
            else
            {
                ShowHideUI(false, true);
            }

            /*else
            {
                NavigateToHomePage();
            }*/
        }

        
        private async void ThisIsAgoodVersion()
        {
            if (Helpers.Settings.GeneralSettings_SkipSS)
            {
                ShowHideUI(false, false);
                OnGifFinished();
            }
            else
            {
                ShowHideUI(true, false);
                await LoadGifFrames();
            }
        }
        private void ShowHideUI(bool Video,bool IsUpdate)
        {
            activityindicator.IsVisible = false;
            Loading.IsVisible = false;
            activityindicator.IsRunning = false;
            canvasView.IsVisible = Video;
            NeedUpdate.IsVisible = IsUpdate;
        }

        private async Task LoadGifFrames()
        {
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync(gifimage);
                var codec = SKCodec.Create(stream);
                gifFrames = new SKBitmap[codec.FrameCount];

                for (int i = 0; i < codec.FrameCount; i++)
                {
                    gifFrames[i] = new SKBitmap(codec.Info.Width, codec.Info.Height);
                    codec.GetPixels(gifFrames[i].Info, gifFrames[i].GetPixels(), new SKCodecOptions(i));
                }

                StartGifAnimation();
            }
            catch
            {
                OnGifFinished(); // Handle any errors in loading the GIF
            }
        }

        private void StartGifAnimation()
        {
            currentFrame = 0;
            //isAnimating = true;

            animationTimer = Dispatcher.CreateTimer();
            animationTimer.Interval = TimeSpan.FromMilliseconds(50);
            animationTimer.Tick += OnAnimationTick;
            animationTimer.Start();
        }
        private void OnAnimationTick(object sender, EventArgs e)
        {
            currentFrame++;
            if (currentFrame >= gifFrames.Length)
            {
                animationTimer.Stop();
                OnGifFinished();
                return;
            }

            canvasView.InvalidateSurface(); // Trigger a redraw
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            e.Surface.Canvas.Clear(SKColors.White);

            if (gifFrames == null || currentFrame >= gifFrames.Length) return;

            var bitmap = gifFrames[currentFrame];
            if (bitmap != null)
            {
                e.Surface.Canvas.DrawBitmap(bitmap, new SKRect(0, 0, e.Info.Width, e.Info.Height));
            }
        }

        private void OnGifFinished()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var user = Helpers.Settings.KeepMeLoggedIn_GetUser();
                if (current == NetworkAccess.Internet && Helpers.Settings.GeneralSettings_KeepMeLoggedIn && user != null)
                {
                    AL_HomePage.CU = user;
                    _ = AppShell.GoToPage(nameof(AL_HomePage));
                }
                else
                {
                    NavigateToHomePage();
                }
            });
        }

        private void NavigateToHomePage()
        {
            if (Helpers.Settings.GeneralSettings_SkipIntro)
            {
                _ = AppShell.GoToPage(nameof(HomePage));
            }
            else
            {
                _ = AppShell.GoToPage(nameof(Intro_1));
            }
        }

        private async void OnUpdateNow(object sender, EventArgs e)
        {
            await TapEventHandler.HandleTapEvent(false, async () =>
            {
                await GlobalFunc.RedirectToAppStore();
            }, Navigation);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //isAnimating = false;

            // Dispose of bitmaps to release resources
            if (gifFrames != null)
            {
                foreach (var frame in gifFrames)
                {
                    frame?.Dispose();
                }
                gifFrames = null;
            }
        }
    }
}
