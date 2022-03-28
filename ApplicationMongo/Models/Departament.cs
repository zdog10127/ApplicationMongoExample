using MongoDB.Bson;

namespace ApplicationMongo.Models
{
    public class Departament
    {
        public ObjectId Id { get; set; }
        public int DepartamentId { get; set; }
        public string DepartamentName  { get; set; }
    }
}
