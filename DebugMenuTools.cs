namespace VARP.DebugMenus
{
    public class DebugMenuTools
    {
        public static string GetFileName(string path)
        {
            var idx = path.LastIndexOf("/");
            if (idx < 0)
                return path;
            return path.Substring(idx + 1);
        }
        
        public static string GetDirectoryName(string path)
        {
            var idx = path.LastIndexOf("/");
            if (idx < 0)
                return string.Empty;
            return path.Substring(0, idx);
        }
    }
}