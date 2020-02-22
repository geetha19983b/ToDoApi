using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Repository;


namespace Todo.Repository
{
    public class SqlRepository : IRepository,IDisposable
    {
        private readonly AuthorContext _context;

        public SqlRepository(AuthorContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public IEnumerable<Author> GetAuthors()
        {
            return _context.Authors.ToList<Author>();
        }
        public Author GetAuthor(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            return _context.Authors.FirstOrDefault(a => a.Id == authorId);
        }

        public void AddAuthor(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            // the repository fills the id (instead of using identity columns)
            author.Id = Guid.NewGuid();

            
            _context.Authors.Add(author);
            _context.SaveChanges();
        }
        public void UpdateAuthor(Guid authorId,Author author)
        {
            var authorfromRepo = _context.Authors.Where(x => x.Id == authorId).FirstOrDefault();

            if (authorfromRepo != null)
            {
                authorfromRepo.FirstName = author.FirstName;
                authorfromRepo.LastName = author.LastName;
                //_context.Entry(author).State = EntityState.Modified;

                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            else
            {
                throw new Exception("Author id does not exists in db");
            }
        }
        private bool AuthorExists(Guid id) =>
         _context.Authors.Any(e => e.Id == id);



        public void DeleteAuthor(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            _context.Authors.Remove(author);
            _context.SaveChanges();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }
    }
}
