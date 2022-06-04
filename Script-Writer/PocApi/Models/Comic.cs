using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PocApi.Models;

public class Comic
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Title")]
    public string Title { get; set; } = null!;

    public string createdBy { get; set; } = null!;

    public ICollection<Issue> Issues { get; set; } = null!;
}