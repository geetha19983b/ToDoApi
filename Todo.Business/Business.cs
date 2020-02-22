using System;
using System.Collections.Generic;
using Todo.Repository;

namespace Todo.Business
{
    public class Business :IBusiness
    {
        private readonly IRepository _repositoryObj;
        public Business(IRepository repositoryObj)
        {
            _repositoryObj = repositoryObj ??
                throw new ArgumentNullException(nameof(repositoryObj));
        }

       
        public void AddAuthor(Author author)
        {
            _repositoryObj.AddAuthor(author);
            

        }

        public void DeleteAuthor(Author author)
        {
            _repositoryObj.DeleteAuthor(author);
        }

        public Author GetAuthor(Guid authorId)
        {
           return  _repositoryObj.GetAuthor(authorId);
        }

        public IEnumerable<Author> GetAuthors()
        {
            return _repositoryObj.GetAuthors();
        }

        public void UpdateAuthor(Guid authorId, Author author)
        {
            _repositoryObj.UpdateAuthor(authorId,author);
        }
    }
}
