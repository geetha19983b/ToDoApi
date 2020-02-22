using System;
using System.Collections.Generic;
using System.Text;
using Todo.Repository;

namespace Todo.Business
{
    public interface IBusiness
    {
        IEnumerable<Author> GetAuthors();
        void AddAuthor(Author author);

        Author GetAuthor(Guid authorId);

        void UpdateAuthor(Guid authorId, Author author);

        void DeleteAuthor(Author author);

    }
}
