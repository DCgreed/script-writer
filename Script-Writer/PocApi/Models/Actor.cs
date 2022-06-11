using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PocApi.Models
{
    /// <summary>
    /// Actor containing the its information.
    /// </summary>
    public class Actor
    {
        /// <summary>
        /// Gets the mongo id of the page.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the comic this actor belongs to.
        /// </summary>
        public string? ComicId { get; set; }

        /// <summary>
        /// Gets or sets the display name of the actor.
        /// </summary>
        public string DisplayName { get; set; } = null!;
    }
}
