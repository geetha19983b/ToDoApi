using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Todo.Repository
{
    public class Author
    {
        // [Key]
        //public Guid Id { get; set; }

        //[Required]
        //[MaxLength(50)]
        //public string FirstName { get; set; }

        //[Required]
        //[MaxLength(50)]
        //public string LastName { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.String)]

        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

    }
}
