using System;
using System.Collections.Generic;
using System.Text;

namespace Todo.Repository
{
    public class Repository : IRepository
    {
        public void AddAuthor(Author author)
        {
            throw new NotImplementedException();
        }

        public void DeleteAuthor(Author author)
        {
            throw new NotImplementedException();
        }

        public Author GetAuthor(Guid authorId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Author> GetAuthors()
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateAuthor(Guid authorId, Author author)
        {
            throw new NotImplementedException();
        }
    }
}
