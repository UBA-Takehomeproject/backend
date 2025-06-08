// using Microsoft.AspNetCore.Mvc;
// using BlogPlatform.Application.DTOs;
// using BlogPlatform.Domain.Interfaces;
// using BlogPlatform.Domain.Entities;
// using BlogPlatform.Application.Extentions;

// namespace BlogPlatform.API.Controllers
// {
//     [ApiController]
//     [Route("api/authors")]

//     public class AuthorsController : ControllerBase
//     {
//         private readonly IRepository<Author> _authorService;

//         public AuthorsController(IRepository<Author> authorService)
//         {
//             _authorService = authorService;
//         }

//         [HttpGet]
//         public async Task<IActionResult> GetAll()
//         {
//             var authors = await _authorService.GetAllAsync();
//             return Ok(authors.ToDtoList());
//         }

//         [HttpGet("{id}")]
//         public async Task<IActionResult> GetById(Guid id)
//         {
//             var author = await _authorService.GetByIdAsync(id.ToString());
//             if (author == null)
//                 return NotFound();

//             return Ok(author.ToDto());
//         }

//         [HttpPost]
//         public async Task<IActionResult> Create([FromBody] AuthorDto request)
//         {
//             var entity = request.ToEntity();
//             await _authorService.AddAsync(entity);
//             return CreatedAtAction(nameof(GetById), new { id = entity.ObjectId }, entity.ToDto());
//         }

//         [HttpPut("{id}")]
//         public async Task<IActionResult> Update(Guid id, [FromBody] AuthorDto request)
//         {
//             var author = await _authorService.GetByIdAsync(id.ToString());
//             if (author == null)
//                 return NotFound();

//             var entityToUpdate = request.ToEntity();
//             entityToUpdate.ObjectId = author.ObjectId; // Ensure the ID is preserved

//             _authorService.Update(entityToUpdate);


//             return NoContent();
//         }

//         [HttpDelete("{id}")]
//         public async Task<IActionResult> Delete(Guid id)
//         {
//             var author = await _authorService.GetByIdAsync(id.ToString());
//             if (author == null)
//                 return NotFound();
//             _authorService.Remove(author);

//             return NoContent();
//         }
//     }
// }


using Microsoft.AspNetCore.Mvc;
using BlogPlatform.Application.DTOs;
using BlogPlatform.Domain.Entities;
using BlogPlatform.Application.Extentions;
using BlogPlatform.Infrastructure.UnitOfWork;
using BlogPlatform.Domain.Interfaces;

namespace BlogPlatform.API.Controllers
{
    [ApiController]
    [Route("api/authors")]
  
    public class AuthorsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthorsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var authors = await _unitOfWork.Authors.GetAllAsync();
            return Ok(authors.ToDtoList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id.ToString());
            if (author == null)
                return NotFound();

            return Ok(author.ToDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AuthorDto request)
        {
            var entity = request.ToEntity();
            await _unitOfWork.Authors.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = entity.ObjectId }, entity.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AuthorDto request)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id.ToString());
            if (author == null)
                return NotFound();

            var updated = request.ToEntity();
            updated.ObjectId = author.ObjectId;

            _unitOfWork.Authors.Update(updated);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id.ToString());
            if (author == null)
                return NotFound();

            _unitOfWork.Authors.Remove(author);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
