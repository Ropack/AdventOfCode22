namespace Common;

public static class ArrayExtensions
{
    public static bool IsInBounds<T>(this T[] array, int index)
    {
        return !(index < array.GetLowerBound(0) || index > array.GetUpperBound(0));
    }

    public static bool IsInBounds<T>(this T[,] array, int x, int y)
    {
        return !(x < array.GetLowerBound(0) || x > array.GetUpperBound(0))
        && !(y < array.GetLowerBound(1) || y > array.GetUpperBound(1));
    }

    public static bool IsInBounds<T>(this T[,,] array, int x, int y, int z)
    {
        return !(x < array.GetLowerBound(0) || x > array.GetUpperBound(0))
               && !(y < array.GetLowerBound(1) || y > array.GetUpperBound(1))
               && !(z < array.GetLowerBound(2) || z > array.GetUpperBound(2));
    }
}