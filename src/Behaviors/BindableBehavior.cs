using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;

namespace BackMeUp.Behaviors;

public class BindableBehavior : Behavior
{
    public static readonly DependencyProperty BehaviorProperty =
        DependencyProperty.Register(nameof(Behavior), typeof(Behavior), typeof(BindableBehavior), new(null, OnBehaviorChanged));

    public Behavior Behavior
    {
        get { return (Behavior)GetValue(BehaviorProperty); }
        set { SetValue(BehaviorProperty, value); }
    }

    private static void OnBehaviorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var bindableBehavior = (BindableBehavior)d;
        bindableBehavior.OnBehaviorChanged((Behavior)e.OldValue, (Behavior)e.NewValue);
    }

    private void OnBehaviorChanged(Behavior oldValue, Behavior newValue)
    {
        if (oldValue != null)
        {
            oldValue.Detach();
        }

        if (newValue != null && AssociatedObject != null)
        {
            newValue.Attach(AssociatedObject);
        }
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        if (Behavior != null)
        {
            Behavior.Attach(AssociatedObject);
        }
    }

    protected override void OnDetaching()
    {
        if (Behavior != null)
        {
            Behavior.Detach();
        }

        base.OnDetaching();
    }
}
