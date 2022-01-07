using System.Runtime.Serialization;

namespace MVCBlog.Web.Infrastructure.Paging;

[DataContract]
public enum SortDirection
{
    [EnumMember]
    Ascending,

    [EnumMember]
    Descending
}