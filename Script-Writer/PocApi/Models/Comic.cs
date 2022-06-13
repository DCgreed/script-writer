using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PocApi.Models;

/// <summary>
/// Comic containing the issues.
/// </summary>
public class Comic
{
    /// <summary>
    /// Gets the mongo id of the panel.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the comic.
    /// </summary>
    [BsonElement("Title")]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets who created the comic.
    /// </summary>
    public string createdBy { get; set; } = null!;

    /// <summary>
    /// Gets or sets the issues in the comic.
    /// </summary>
    public ICollection<Issue>? Issues { get; set; } = null!;
}