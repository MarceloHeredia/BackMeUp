using Microsoft.UI.Xaml.Markup;
using Microsoft.Windows.ApplicationModel.Resources;

namespace SaveMe.Utils.ExtensionMethods
{
    [MarkupExtensionReturnType(ReturnType = typeof(string))]
    internal class StringResourceExtension : MarkupExtension
    {
        private static readonly ResourceManager _resourceManager = new();
        private static readonly ResourceContext _resourceContext = _resourceManager.CreateResourceContext();

        public StringResourceExtension() { }

        public string SubTree { get; set; } = "Resources";
        public string Key { get; set; } = "";

        protected override object ProvideValue()
        {
            var resourceMap = _resourceManager.MainResourceMap.GetSubtree(SubTree);
            return resourceMap.GetValue(Key, _resourceContext).ValueAsString;
        }
    }
}
