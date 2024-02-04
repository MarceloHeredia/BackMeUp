using Microsoft.UI.Xaml.Markup;
using Microsoft.Windows.ApplicationModel.Resources;

namespace BackMeUp.Utils.ExtensionMethods
{
    [MarkupExtensionReturnType(ReturnType = typeof(string))]
    internal class StringResourceExtension : MarkupExtension
    {
        private static readonly Microsoft.Windows.ApplicationModel.Resources.ResourceManager ResourceManager = new();
        private static readonly ResourceContext ResourceContext = ResourceManager.CreateResourceContext();

        public StringResourceExtension() { }

        public string SubTree { get; set; } = "Resources";
        public string Key { get; set; } = "";

        protected override object ProvideValue()
        {
            var resourceMap = ResourceManager.MainResourceMap.GetSubtree(SubTree);
            return resourceMap.GetValue(Key, ResourceContext).ValueAsString;
        }
    }

}
