using System.ComponentModel.DataAnnotations;

namespace MVCBlog.Core.Entities
{
    /// <summary>
    /// Represents a FeedStatistic.
    /// </summary>
    public class FeedStatistic : EntityBase
    {
        [StringLength(50)]
        public string Application { get; set; }

        [StringLength(40)]
        public string Identifier { get; set; }

        public int Users { get; set; }

        public int Visits { get; set; }
    }
}
