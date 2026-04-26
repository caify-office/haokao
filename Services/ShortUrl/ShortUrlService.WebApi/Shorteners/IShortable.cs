namespace ShortUrlService.WebApi.Shorteners;

public interface IShortable
{
    string Short(object value);

    string Restore(string key);
}