namespace ShortUrlService.Domain.Charts;

public class ShortUrlChartsOutput(int length)
{
    public int[] Access { get; set; } = new int[length];

    public Dictionary<int, int>[] BrowserAccess { get; set; } = new Dictionary<int, int>[length];

    public Dictionary<int, int>[] OsAccess { get; set; } = new Dictionary<int, int>[length];

    public int[] Generate { get; set; } = new int[length];

    public string[] Labels { get; set; } = new string[length];

    public string Title { get; set; } = "";
}

public enum ChartTypeEnum
{
    Day = 1,
    Week = 2,
    Month = 3
}