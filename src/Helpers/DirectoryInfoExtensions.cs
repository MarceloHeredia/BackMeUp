using System.IO;

namespace BackMeUp.Helpers
{
    internal static class DirectoryInfoExtensions
    {
        internal static void Empty(this DirectoryInfo directory)
        {
            foreach (var file in directory.GetFiles())
            {
                file.Delete();
            }

            foreach (var subDirectory in directory.GetDirectories())
            {
                subDirectory.Empty();
                subDirectory.Delete(true);
            }
        }
    }
}
