using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlogPlatform.Domain.Entities;
using BlogPlatform.Domain.Interfaces;
using BlogPlatform.Infrastructure.UnitOfWork;
using BlogPlatform.Application.Extensions;
using BlogPlatform.Application.DTOs;

namespace BlogPlatform.API.Controllers
{
    [Route("api/blog")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BlogController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/blog
        [HttpGet]
        public async Task<IActionResult> GetAllBlogs()
        {
            var blogs = await _unitOfWork.Blogs.GetAllAsync();
            return Ok(blogs.ToDtoList());
        }

        // GET: api/blog/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlogById(Guid id)
        {
            var blog = await _unitOfWork.Blogs.GetByIdAsync(id.ToString());
            if (blog == null)
                return NotFound();

            return Ok(blog.ToDto());
        }

        // POST: api/blog
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> CreateBlog([FromBody] BlogDto blog)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _unitOfWork.Blogs.AddAsync(blog.ToEntity());
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBlogById), new { id = blog.objectId }, blog);
        }

        // PUT: api/blog/5
        [HttpPut("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> UpdateBlog(Guid id, [FromBody] BlogDto blog)
        {
            if (id != blog.objectId)
                return BadRequest("Blog ID mismatch.");

            var existing = await _unitOfWork.Blogs.GetByIdAsync(id.ToString());
            if (existing == null)
                return NotFound();

            // Update fields
            existing.Title = blog.title;
            existing.Href = blog.href;

            _unitOfWork.Blogs.Update(existing);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/blog/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBlog(Guid id)
        {
            var blog = await _unitOfWork.Blogs.GetByIdAsync(id.ToString());
            if (blog == null)
                return NotFound();

            _unitOfWork.Blogs.Remove(blog);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
