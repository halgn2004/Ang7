namespace Ang7.CustomControls;

public partial class WatermarkControl : ContentView
{
    public static readonly BindableProperty BadgeTextProperty =
        BindableProperty.Create(nameof(BadgeText), typeof(string), typeof(WatermarkControl), string.Empty, propertyChanged: OnBadgeTextChanged);

    public string BadgeText
    {
        get => (string)GetValue(BadgeTextProperty);
        set => SetValue(BadgeTextProperty, value);
    }

    public static readonly BindableProperty IsAnimationEnabledProperty =
    BindableProperty.Create(nameof(IsAnimationEnabled), typeof(bool), typeof(WatermarkControl), false, propertyChanged: OnIsAnimationEnabledChanged);

    public bool IsAnimationEnabled
    {
        get => (bool)GetValue(IsAnimationEnabledProperty);
        set => SetValue(IsAnimationEnabledProperty, value);
    }
    private static void OnIsAnimationEnabledChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is WatermarkControl control && newValue is bool isEnabled)
        {
            if (isEnabled)
            {
                control.StartRandomAnimation();
            }
            else
            {
                control.StopAnimations();
            }
        }
    }
    private static void OnBadgeTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is WatermarkControl control && newValue is string newText)
        {
            control.WaterMarkBadge.Text = newText;
        }
    }
    private uint _animationDuration = 4000; // Duration of the animation in milliseconds

    public WatermarkControl()
    {
        InitializeComponent();
        //if(IsAnimationEnabled)
        //   StartRandomAnimation();
    }
    public void StartRandomAnimation()
    {
        var random = new Random();
        int animationIndex = random.Next(1, 3); // Adjust based on the number of animations

        switch (animationIndex)
        {
            case 1:
                StartRotationAnimation();
                break;
            case 2:
                StartScalingAnimation();
                break;
        }
    }

    private void StartRotationAnimation()
    {
        var animation = new Animation
            {
                { 0, 1, new Animation(v => AnimatedGrid.Rotation = v, 0, 360, Easing.Linear) }
            };
        animation.Commit(this, "RotationAnimation", length: _animationDuration, easing: Easing.Linear, repeat: () => true);
    }

    private void StartScalingAnimation()
    {
        var animation = new Animation
            {
                { 0, 0.5, new Animation(v => AnimatedGrid.Scale = v, 1, 1.5, Easing.SpringOut) },
                { 0.5, 1, new Animation(v => AnimatedGrid.Scale = v, 1.5, 1, Easing.SpringOut) }
            };
        animation.Commit(this, "ScalingAnimation", length: _animationDuration, easing: Easing.Linear, repeat: () => true);
    }

    public void StopAnimations()
    {
        this.AbortAnimation("RotationAnimation");
        this.AbortAnimation("ScalingAnimation");
    }
}