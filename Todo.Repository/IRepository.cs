using System;
using System.Collections.Generic;
using System.Text;


namespace Todo.Repository
{
    public interface IRepository
    {
        IEnumerable<Author> GetAuthors();
        void AddAuthor(Author author);
        Author GetAuthor(Guid authorId);

        void UpdateAuthor(Guid authorId, Author author);

        void DeleteAuthor(Author author);
       
    }
}
