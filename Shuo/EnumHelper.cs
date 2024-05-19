namespace Shuo;

public static class EnumHelper
{
    public static T NextEnum<T>(this T src) where T : struct
    {
        if (!typeof(T).IsEnum) throw new ArgumentException($"Argument {typeof(T).FullName} is not an Enum");
        var arr = (T[])Enum.GetValues(src.GetType());
        int j = Array.IndexOf(arr, src) + 1;
        return (arr.Length == j) ? arr[0] : arr[j];
    }
}
