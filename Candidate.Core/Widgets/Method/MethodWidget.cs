using System.Reflection;

namespace Candidate.Core.Widgets.Method;

public static class MethodWidget
{

    /// <summary>
    /// برگرداندان متد و کلاس
    /// </summary>
    /// <param name="methodBase"></param>
    /// <returns></returns>
    public static string GetSourceName(this MethodBase methodBase)
    {
        var className = methodBase.ReflectedType;
        var result = className == null ? null : className.FullName;
        result = $"{result}({methodBase.Name})";
        return result;
    }


    /// <summary>
    /// برگرداندن نام متد
    /// </summary>
    /// <param name="methodBase"></param>
    /// <returns></returns>
    public static string GetMethodName(this MethodBase methodBase)
    {
        string result = methodBase.Name;
        return result;
    }
}