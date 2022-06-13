using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PocApi.Models
{
    /// <summary>
    /// Panel containing the text.
    /// </summary>
    public class Panel
    {
        /// <summary>
        /// Gets the mongo id of the panel.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the issue this page belongs to.
        /// </summary>
        public string? PageId { get; set; }

        /// <summary>
        /// Gets or sets the description of the page.
        /// </summary>
        public string PanelDescription { get; set; } = null!;

        /// <summary>
        /// Gets or sets the panel order.
        /// </summary>
        public int PanelOrder { get; set; }

        /// <summary>
        /// Gets or set the dialogues in the panel.
        /// </summary>
        public ICollection<Dialogue>? Dialogues { get; set; } = null!;
    }
}
