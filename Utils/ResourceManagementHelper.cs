using Microsoft.Windows.ApplicationModel.Resources;

namespace BackMeUp.Utils
{
    internal static class ResourceManagementHelper
    {
        private static readonly ResourceManager ResourceManager = new();
        private static readonly ResourceContext ResourceContext = ResourceManager.CreateResourceContext();
        public static string GetResource(string key, string subTree = "Resources")
        {
            var resourceMap = ResourceManager.MainResourceMap.GetSubtree(subTree);
            return resourceMap.GetValue(key, ResourceContext).ValueAsString;
        }
    }
}
