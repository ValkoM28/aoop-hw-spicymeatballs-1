using System;
using System.Threading.Tasks;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;

namespace RestaurantSimulator.Services;

public class AnimationService
{
    public static async Task AnimateProgressBar(ProgressBar progressBar, double from, double to, TimeSpan duration)
    {
        var animation = new Animation
        {
            Duration = duration,
            Children =
            {
                new KeyFrame
                {
                    Cue = new Cue(0),
                    Setters = { new Setter(ProgressBar.ValueProperty, from) }
                },
                new KeyFrame
                {
                    Cue = new Cue(1),
                    Setters = { new Setter(ProgressBar.ValueProperty, to) }
                }
            }
        };

        await animation.RunAsync(progressBar);
    }

    public static async Task AnimateColorTransition(Control control, IBrush fromColor, IBrush toColor, TimeSpan duration)
    {
        var animation = new Animation
        {
            Duration = duration,
            Children =
            {
                new KeyFrame
                {
                    Cue = new Cue(0),
                    Setters = { new Setter(Control.OpacityProperty, 0.5) }
                },
                new KeyFrame
                {
                    Cue = new Cue(1),
                    Setters = { new Setter(Control.OpacityProperty, 1.0) }
                }
            }
        };

        await animation.RunAsync(control);
    }

    public static async Task AnimateScale(Control control, double fromScale, double toScale, TimeSpan duration)
    {
        var animation = new Animation
        {
            Duration = duration,
            Children =
            {
                new KeyFrame
                {
                    Cue = new Cue(0),
                    Setters = { new Setter(Control.RenderTransformProperty, new ScaleTransform(fromScale, fromScale)) }
                },
                new KeyFrame
                {
                    Cue = new Cue(1),
                    Setters = { new Setter(Control.RenderTransformProperty, new ScaleTransform(toScale, toScale)) }
                }
            }
        };

        await animation.RunAsync(control);
    }
} 