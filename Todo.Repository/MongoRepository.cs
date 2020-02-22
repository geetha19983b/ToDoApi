using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
namespace Todo.Repository
{
    public class MongoRepository : IRepository
    {
        private readonly IMongoCollection<Author> _authors;

        public MongoRepository(IMongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _authors= database.GetCollection<Author>(settings.CollectionName);
        }
        public void AddAuthor(Author author)
        {
            _authors.InsertOne(author);
        }

        public void DeleteAuthor(Author author)
        {
            _authors.DeleteOne(auth => auth.Id == author.Id);
        }

        public Author GetAuthor(Guid authorId)
        {
           return  _authors.Find<Author>(author=> author.Id==authorId).FirstOrDefault();
        }

        public IEnumerable<Author> GetAuthors()
        {
            return _authors.Find<Author>(author => true).ToList();
        }

        
        

        public void UpdateAuthor(Guid authorId, Author author)
        {
            _authors.ReplaceOne(auth => auth.Id == authorId,author);
        }
    }
}
