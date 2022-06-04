using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PocApi.Models
{
    public class Panel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? PageId { get; set; }

        public string PanelDesscription { get; set; } = null!;

        public int PanelOrder { get; set; }

        public ICollection<Text> Texts { get; set; } = null!;
    }
}
