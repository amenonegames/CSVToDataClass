using System.Linq;

namespace Packages.DataClassGenerator.Settings
{
    public static class ValidTypeJudge
    {
        public static bool IsValidTypeStr(string field)
        {
            string[] nativeTypes = new [] { "int", "uint" ,"float", "double", "bool", "string" ,"Vector2","Vector3" }; // 例
            return nativeTypes.Contains(field);
        }
    }
}