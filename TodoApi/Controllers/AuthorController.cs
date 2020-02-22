﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Todo.Business;
using Todo.Repository;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IBusiness _businessObj;
        public AuthorController(IBusiness businessobj)
        {
            _businessObj = businessobj ??
                throw new ArgumentNullException(nameof(businessobj));
        }

        [HttpGet()]
       
        public ActionResult<IEnumerable<Author>> GetAuthors()
        {
            var authorsFromRepo = _businessObj.GetAuthors();
            if (authorsFromRepo != null)
            {
                return Ok(authorsFromRepo);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{authorId}", Name = "GetAuthor")]
        public IActionResult GetAuthor(Guid authorId)
        {
            var authorFromRepo = _businessObj.GetAuthor(authorId);

            if (authorFromRepo == null)
            {
                return NotFound();
            }

            return Ok(authorFromRepo);
        }
        [HttpPut("{authorId}")]
        public IActionResult UpdateAuthor(Guid authorId,Author author)
        {
            if(authorId != author.Id)
            {
                return BadRequest();
            }
            var authorFromRepo = _businessObj.GetAuthor(authorId);

            if (authorFromRepo == null)
            {
                return NotFound();
            }

            _businessObj.UpdateAuthor(authorId, author);
            return NoContent();
        }
        [HttpDelete("{authorId}")]
        public ActionResult DeleteAuthor(Guid authorId)
        {
            var authorFromRepo = _businessObj.GetAuthor(authorId);

            if (authorFromRepo == null)
            {
                return NotFound();
            }

            _businessObj.DeleteAuthor(authorFromRepo);

            return NoContent();
        }
        [HttpPost]
        public ActionResult<Author> CreateAuthor(Author author)
        {

            _businessObj.AddAuthor(author);
            

            
            return CreatedAtRoute("GetAuthor",
                new { authorId = author.Id },
                author);
        }

    }
}