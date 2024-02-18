namespace BackMeUp.Activation;

// https://github.com/microsoft/TemplateStudio/blob/main/docs/WinUI/activation.md
public abstract class ActivationHandler<T> : IActivationHandler where T : class
{
    // override this method to add the logic for whether to handle the activation.
    protected virtual bool CanHandleInternal(T args) => true;
    
    // override this method to add the logic for handling the activation handler.
    protected abstract Task HandleInternalAsync(T args);
    
    public bool CanHandle(object args) => args is T argsT && CanHandleInternal(argsT);

    public async Task HandleAsync(object args) => await HandleInternalAsync((args as T)!);
}
