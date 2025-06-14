using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlogPlatform.Domain.Entities;
using BlogPlatform.Domain.Interfaces;
using BlogPlatform.Infrastructure.UnitOfWork;
using BlogPlatform.Application.Extensions;
using BlogPlatform.Application.DTOs;
using BlogPlatform.Application.Extentions;

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

        // GET: api/blog/author/{authorId}
        [HttpGet("by/{authorId}")]
        public async Task<IActionResult> GetBlogsByAuthorId(Guid authorId)
        {
            var blogs = await _unitOfWork.Blogs.GetAllAsync(
            b => b.AuthorsInfo.ObjectId == authorId,
            b => b.AuthorsInfo,
            b => b.BlogPosts
            );

            if (blogs == null || !blogs.Any())
                return NotFound();

            return Ok(blogs.ToDtoList());
        }
        // GET: api/blog
        [HttpGet]
        public async Task<IActionResult> GetAllBlogs()
        {
            var blogs = await _unitOfWork.Blogs.GetAllAsync(null, b => b.AuthorsInfo, b => b.AuthorsInfo, b => b.BlogPosts);
            return Ok(blogs.ToDtoList());
        }

        // GET: api/blog/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlogById(Guid id)
        {
            var blog = await _unitOfWork.Blogs.GetByIdAsync(id, b => b.AuthorsInfo);
            var blogPosts = await _unitOfWork.BlogPosts.GetAllAsync(b => b.BlogObjectId == id, b => b.Blog);

            if (blog == null)
                return NotFound();

            var blogDto = blog.ToDto();
            if (blogPosts == null)
            {
                blogDto.blogPosts = new List<BlogPostDto>();
                return Ok(blogDto);
            }
            blogDto.blogPosts = blogPosts.ToDtoList();
            return Ok(blogDto);
        }

        // POST: api/blog
        [HttpPost]
        [Authorize(Roles = "USER,ADMIN")]
        public async Task<IActionResult> CreateBlog([FromBody] BlogDto blog)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (blog.objectId == Guid.Empty)
            {
                var uid = Guid.NewGuid();
                blog.objectId = uid;
                blog.href = $"/blog?blogid={uid}";
            }
            blog.createdAt = DateTime.UtcNow;

            await _unitOfWork.Blogs.AddAsync(blog.ToEntity());
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBlogById), new { id = blog.objectId }, blog);
        }

        // PUT: api/blog/5
        [HttpPut("{id}")]
        [Authorize(Roles = "USER,ADMIN")]
        public async Task<IActionResult> UpdateBlog(Guid id, [FromBody] BlogDto blog)
        {
            if (id != blog.objectId)
                return BadRequest("Blog ID mismatch.");

            var existing = await _unitOfWork.Blogs.GetByIdAsync(id, b => b.AuthorsInfo, b => b.BlogPosts);
            if (existing == null)
                return NotFound();

            // Update fields
            existing.Title = blog.title;
            existing.Href = blog.href;
            existing.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Blogs.Update(existing);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/blog/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteBlog(Guid id)
        {
            var blog = await _unitOfWork.Blogs.GetByIdAsync(id);
            if (blog == null)
                return NotFound();

            _unitOfWork.Blogs.Remove(blog);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
