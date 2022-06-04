using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PocApi.Models
{
    public class Page
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? IssueId { get; set; }
        
        public int? PageNumber { get; set; }

        public string? PageTitle { get; set; }

        public string PageDescription { get; set; } = null!;

        public ICollection<Panel> Panels { get; set; } = null!;
    }
}
