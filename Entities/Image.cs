namespace SimpleWebAppReact.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Libmongocrypt;

/// <summary>
/// 
/// </summary>
public class Image
{
    [BsonId]
    [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonElement("image"), BsonRepresentation(BsonType.Binary)]
    public Binary? ImageData { get; set; }
    
    [BsonElement("description"), BsonRepresentation(BsonType.String)]
    public string? Description { get; set; }

    [BsonElement("dateTaken"), BsonRepresentation(BsonType.DateTime)]
    public DateTime? DateTaken { get; set; }
}