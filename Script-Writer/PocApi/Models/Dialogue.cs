using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PocApi.Models
{
    /// <summary>
    /// Dialgoue containing the actor and the line.
    /// </summary>
    public class Dialogue
    {
        /// <summary>
        /// Gets the mongo id of the page.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the panel this dialogue belongs to.
        /// </summary>
        public string? PanelId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the actor for this dialogue.
        /// </summary>
        public string? ActorId { get; set; }

        /// <summary>
        /// Gets or set the name of the actor.
        /// </summary>
        public string? ActorName { get; set; }

        /// <summary>
        /// Gets or set the order of the dialogue within the panel.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the line for the dialogue.
        /// </summary>
        public string Line { get; set; } = null!;
    }
}
