using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PocApi.Models
{
    public class Text
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? PanelId { get; set; }

        public string ActorName { get; set; } = null!;

        public int? ActorId { get; set; }

        public int Order { get; set; }

        public string Line { get; set; } = null!;
    }
}
