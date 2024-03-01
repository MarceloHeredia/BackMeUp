namespace BackMeUp.Helpers;

/// <summary>
/// This was created to add support to ContentNavigationService, which does not use Frame.Navigate and cant trigger the NavigationEventArgs.
/// This custom event is enough for both implementations of NavigationService.
/// </summary>
/// <param name="sourcePageType"></param>
public class CustomNavigationEventArgs(Type sourcePageType) : EventArgs
{
    public Type SourcePageType { get; } = sourcePageType;
}

public delegate void CustomNavigatedEventHandler(object sender, CustomNavigationEventArgs e);
