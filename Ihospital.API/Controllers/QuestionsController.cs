using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ihospital.API.Data;
using Ihospital.API.DTOs;
using Ihospital.API.Models;

namespace Ihospital.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public QuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetQuestions([FromQuery] bool activeOnly = false)
        {
            var query = _context.Questions
                .Include(q => q.Options)
                .AsQueryable();

            if (activeOnly)
                query = query.Where(q => q.IsActive);

            var questions = await query
                .OrderBy(q => q.DisplayOrder)
                .Select(q => new QuestionDto
                {
                    QuestionId = q.QuestionId,
                    QuestionText = q.QuestionText,
                    QuestionCode = q.QuestionCode,
                    QuestionCategory = q.QuestionCategory,
                    ResponseType = q.ResponseType,
                    IsActive = q.IsActive,
                    IsRequired = q.IsRequired,
                    MaxSelections = q.MaxSelections,
                    DisplayOrder = q.DisplayOrder,
                    Options = q.Options
                        .Where(o => o.IsActive)
                        .OrderBy(o => o.OptionOrder)
                        .Select(o => new OptionListDto
                        {
                            OptionId = o.OptionId,
                            QuestionId = o.QuestionId,
                            OptionText = o.OptionText,
                            IsActive = o.IsActive,
                            OptionOrder = o.OptionOrder
                        }).ToList()
                })
                .ToListAsync();

            return Ok(questions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDto>> GetQuestion(int id)
        {
            var question = await _context.Questions
                .Include(q => q.Options)
                .Where(q => q.QuestionId == id)
                .Select(q => new QuestionDto
                {
                    QuestionId = q.QuestionId,
                    QuestionText = q.QuestionText,
                    QuestionCode = q.QuestionCode,
                    QuestionCategory = q.QuestionCategory,
                    ResponseType = q.ResponseType,
                    IsActive = q.IsActive,
                    IsRequired = q.IsRequired,
                    MaxSelections = q.MaxSelections,
                    DisplayOrder = q.DisplayOrder,
                    Options = q.Options
                        .OrderBy(o => o.OptionOrder)
                        .Select(o => new OptionListDto
                        {
                            OptionId = o.OptionId,
                            QuestionId = o.QuestionId,
                            OptionText = o.OptionText,
                            IsActive = o.IsActive,
                            OptionOrder = o.OptionOrder
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            if (question == null)
                return NotFound();

            return Ok(question);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<QuestionDto>> CreateQuestion([FromBody] CreateQuestionDto dto)
        {
            // Check if question code is unique
            if (!string.IsNullOrEmpty(dto.QuestionCode) && 
                await _context.Questions.AnyAsync(q => q.QuestionCode == dto.QuestionCode))
            {
                return BadRequest(new { message = "Question code already exists" });
            }

            var question = new Question
            {
                QuestionText = dto.QuestionText,
                QuestionCode = dto.QuestionCode,
                QuestionCategory = dto.QuestionCategory,
                ResponseType = dto.ResponseType,
                IsActive = true,
                IsRequired = dto.IsRequired,
                MaxSelections = dto.MaxSelections,
                DisplayOrder = dto.DisplayOrder
            };

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetQuestion), new { id = question.QuestionId }, question);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(int id, [FromBody] UpdateQuestionDto dto)
        {
            var question = await _context.Questions.FindAsync(id);

            if (question == null)
                return NotFound();

            if (dto.QuestionText != null)
                question.QuestionText = dto.QuestionText;
            if (dto.QuestionCode != null)
                question.QuestionCode = dto.QuestionCode;
            if (dto.QuestionCategory != null)
                question.QuestionCategory = dto.QuestionCategory;
            if (dto.ResponseType != null)
                question.ResponseType = dto.ResponseType;
            if (dto.IsActive.HasValue)
                question.IsActive = dto.IsActive.Value;
            if (dto.IsRequired.HasValue)
                question.IsRequired = dto.IsRequired.Value;
            if (dto.MaxSelections.HasValue)
                question.MaxSelections = dto.MaxSelections;
            if (dto.DisplayOrder.HasValue)
                question.DisplayOrder = dto.DisplayOrder.Value;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);

            if (question == null)
                return NotFound();

            // Soft delete by setting IsActive to false
            question.IsActive = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<string>>> GetCategories()
        {
            var categories = await _context.Questions
                .Where(q => q.QuestionCategory != null)
                .Select(q => q.QuestionCategory!)
                .Distinct()
                .ToListAsync();

            return Ok(categories);
        }
    }
}
