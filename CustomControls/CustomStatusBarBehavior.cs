/*using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls;

namespace Ang7.CustomControls
{
    public class CustomStatusBarBehavior : Behavior<Page>
    {
        // Default color and style for the status bar
        private static Color _statusBarColor = Colors.White;
        private static StatusBarStyle _statusBarStyle = StatusBarStyle.DarkContent;

        // Define the attached property for StatusBarColor
        public static readonly BindableProperty StatusBarColorProperty = BindableProperty.CreateAttached(
            propertyName: "StatusBarColor",
            returnType: typeof(Color),
            declaringType: typeof(CustomStatusBarBehavior),
            defaultValue: Colors.White,
            propertyChanged: OnStatusBarColorChanged);

        // Define the attached property for StatusBarStyle
        public static readonly BindableProperty StatusBarStyleProperty = BindableProperty.CreateAttached(
            propertyName: "StatusBarStyle",
            returnType: typeof(StatusBarStyle),
            declaringType: typeof(CustomStatusBarBehavior),
            defaultValue: StatusBarStyle.DarkContent,
            propertyChanged: OnStatusBarStyleChanged);

        // Property change handler for StatusBarColor
        private static void OnStatusBarColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is Page page && newValue is Color newColor)
            {
                _statusBarColor = newColor;
                UpdateStatusBarBehavior(page);
            }
        }

        // Property change handler for StatusBarStyle
        private static void OnStatusBarStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is Page page && newValue is StatusBarStyle newStyle)
            {
                _statusBarStyle = newStyle;
                UpdateStatusBarBehavior(page);
            }
        }

        // Getters and setters for the attached properties
        public static Color GetStatusBarColor(BindableObject view) => (Color)view.GetValue(StatusBarColorProperty);
        public static void SetStatusBarColor(BindableObject view, Color value) => view.SetValue(StatusBarColorProperty, value);

        public static StatusBarStyle GetStatusBarStyle(BindableObject view) => (StatusBarStyle)view.GetValue(StatusBarStyleProperty);
        public static void SetStatusBarStyle(BindableObject view, StatusBarStyle value) => view.SetValue(StatusBarStyleProperty, value);

        // Adds or updates the StatusBarBehavior on the page
        private static void UpdateStatusBarBehavior(Page page)
        {
            // Check if a StatusBarBehavior already exists, and if not, add one
            var existingBehavior = page.Behaviors.FirstOrDefault(b => b is StatusBarBehavior) as StatusBarBehavior;

            if (existingBehavior == null)
            {
                page.Behaviors.Add(new StatusBarBehavior
                {
                    StatusBarColor = _statusBarColor,
                    StatusBarStyle = _statusBarStyle
                });
            }
            else
            {
                existingBehavior.StatusBarColor = _statusBarColor;
                existingBehavior.StatusBarStyle = _statusBarStyle;
            }
        }
    }
}*/