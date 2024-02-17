using System.IO;

namespace BackMeUp.Utils.ExtensionMethods
{
    internal static class DirectoryInfoExtensions
    {
        internal static void Empty(this DirectoryInfo directory)
        {
            foreach (var file in directory.GetFiles()) file.Delete();
            foreach (var subDirectory in directory.GetDirectories())
            {
                Empty(subDirectory);
                subDirectory.Delete(true);
            }
        }
    }
}
