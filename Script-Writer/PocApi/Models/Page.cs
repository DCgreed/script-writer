using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PocApi.Models
{
    /// <summary>
    /// Page containing the panels.
    /// </summary>
    public class Page
    {
        // <summary>
        /// Gets the mongo id of the page.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the issue this page belongs to.
        /// </summary>
        public string? IssueId { get; set; }

        /// <summary>
        /// Gets or set the number of the page.
        /// </summary>
        public int? PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the title of the issue.
        /// </summary>
        public string? PageTitle { get; set; }

        /// <summary>
        /// Gets or sets the description of the page.
        /// </summary>
        public string PageDescription { get; set; } = null!;

        /// <summary>
        /// Gets or set the panels in the page.
        /// </summary>
        public ICollection<Panel>? Panels { get; set; } = null!;
    }
}
