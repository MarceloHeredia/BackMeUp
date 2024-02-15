using Microsoft.Windows.ApplicationModel.Resources;

namespace BackMeUp.Data.Management
{
    internal static class ResourcesManager
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
