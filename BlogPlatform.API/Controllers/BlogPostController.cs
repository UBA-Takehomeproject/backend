using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlogPlatform.Domain.Entities;
using BlogPlatform.Domain.Interfaces;
using BlogPlatform.Application.Extentions;
using BlogPlatform.Application.DTOs;

namespace BlogPlatform.API.Controllers
{
    [Route("api/blogpost")]
    // [Produces("application/json")]
    // [Consumes("application/json")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BlogPostController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/blogpost
        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _unitOfWork.BlogPosts.GetAllAsync();
            return Ok(posts.ToDtoList());
        }

        // GET: api/blogpost/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(Guid id)
        {
            var post = await _unitOfWork.BlogPosts.GetByIdAsync(id.ToString());
            if (post == null)
                return NotFound();

            return Ok(post.ToDto());
        }

        // GET: api/blogpost/blog/{blogId}
        [HttpGet("blog/{blogId}")]
        public async Task<IActionResult> GetPostsByBlog(Guid blogId)
        {
            var post = await _unitOfWork.BlogPosts.GetByIdAsync(blogId.ToString());


            return Ok(post.ToDto());
        }

        // POST: api/blogpost
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> CreatePost([FromBody] BlogPostDto post)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _unitOfWork.BlogPosts.AddAsync(post.ToEntity());
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPostById), new { id = post.objectId }, post);
        }

        // PUT: api/blogpost/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> UpdatePost(Guid id, [FromBody] BlogPostDto post)
        {
            if (id != post.objectId)
                return BadRequest("Post ID mismatch.");

            var existing = await _unitOfWork.BlogPosts.GetByIdAsync(id.ToString());
            if (existing == null)
                return NotFound();

            existing.Title = post.title;
            existing.Content = post.content;
            existing.CreatedAt = post.createdAt;
            existing.ObjectId = post.objectId;

            _unitOfWork.BlogPosts.Update(existing);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/blogpost/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> DeletePost(Guid  id)
        {
            var post = await _unitOfWork.BlogPosts.GetByIdAsync(id.ToString());
            if (post == null)
                return NotFound();

            _unitOfWork.BlogPosts.Remove(post);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
