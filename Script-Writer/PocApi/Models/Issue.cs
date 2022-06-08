using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PocApi.Models
{
    /// <summary>
    /// Issue containing its pages.
    /// </summary>
    public class Issue
    {
        /// <summary>
        /// Gets the mongo id of the issue.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        /// <summary>
        /// The identifier of the comic this issue belongs to.
        /// </summary>
        public string? ComicId { get; set; }

        /// <summary>
        /// The number of the issue.
        /// </summary>
        public int IssueNumber { get; set; }

        /// <summary>
        /// Gets or sets the title of the issue.
        /// </summary>
        public string Title { get; set; } = null!;
                
        /// <summary>
        /// Gets or sets the pages in the issue.
        /// </summary>
        public ICollection<Page>? Pages { get; set; } = null!;
    }
}
