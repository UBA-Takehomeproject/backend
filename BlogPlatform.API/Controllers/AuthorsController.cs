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
            var author = await _unitOfWork.Authors.GetByIdAsync(id);
            if (author == null)
                return NotFound();

            return Ok(author.ToDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AuthorDto request)
        {

            // var user = await _unitOfWork.Users.GetByIdAsync(request.objectId.ToString());
            if (request.objectId == Guid.Empty)
                request.objectId = Guid.NewGuid();

            var entity = request.ToEntity();
            await _unitOfWork.Authors.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = entity.ObjectId }, entity.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AuthorDto request)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id);
            if (author == null)
                return NotFound();
            // var user = await _unitOfWork.Users.GetByIdAsync(request.objectId.ToString());
            // if (user == null)
            //     return NotFound("User not found");
            var updated = request.ToEntity();
            updated.ObjectId = author.ObjectId;

            _unitOfWork.Authors.Update(updated);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id);
            if (author == null)
                return NotFound();

            _unitOfWork.Authors.Remove(author);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
