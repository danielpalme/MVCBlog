using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;

namespace MVCBlog.Web.Infrastructure;

public static class UrlHelper
{
    public static string SetParameters(this string? url, params KeyValuePair<string, string>[] values)
    {
        var query = QueryHelpers.ParseQuery(url);

        var items = query
            .SelectMany(x => x.Value, (col, value) => new KeyValuePair<string, string>(col.Key, value))
            .ToList();

        foreach (var value in values)
        {
            items.RemoveAll(i => i.Key == value.Key);
            if (!string.IsNullOrEmpty(value.Value))
            {
                items.Add(value);
            }
        }

        return new QueryBuilder(items).ToQueryString().Value ?? string.Empty;
    }
}