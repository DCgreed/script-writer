using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PocApi.Models
{
    public class Issue
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? ComicId { get; set; }

        public int IssueNumber { get; set; }

        public string Title { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public ICollection<Page> Pages { get; set; } = null!;
    }
}
