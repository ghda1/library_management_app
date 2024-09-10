public class Utils
{
    public static List<T> Pagination<T>(List<T> list, int pageNumber = 1, int limit = 3) where T : CommonFeatures
    {
        return list.Skip((pageNumber - 1) * limit).Take(limit).ToList();
    }
}