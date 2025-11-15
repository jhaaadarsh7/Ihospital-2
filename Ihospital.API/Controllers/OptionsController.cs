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
    [Authorize]
    public class OptionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("question/{questionId}")]
        public async Task<ActionResult<IEnumerable<OptionListDto>>> GetOptionsByQuestion(int questionId)
        {
            var options = await _context.OptionLists
                .Where(o => o.QuestionId == questionId)
                .OrderBy(o => o.OptionOrder)
                .Select(o => new OptionListDto
                {
                    OptionId = o.OptionId,
                    QuestionId = o.QuestionId,
                    OptionText = o.OptionText,
                    IsActive = o.IsActive,
                    OptionOrder = o.OptionOrder
                })
                .ToListAsync();

            return Ok(options);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OptionListDto>> GetOption(int id)
        {
            var option = await _context.OptionLists
                .Where(o => o.OptionId == id)
                .Select(o => new OptionListDto
                {
                    OptionId = o.OptionId,
                    QuestionId = o.QuestionId,
                    OptionText = o.OptionText,
                    IsActive = o.IsActive,
                    OptionOrder = o.OptionOrder
                })
                .FirstOrDefaultAsync();

            if (option == null)
                return NotFound();

            return Ok(option);
        }

        [HttpPost]
        public async Task<ActionResult<OptionListDto>> CreateOption([FromBody] CreateOptionDto dto)
        {
            // Check if question exists
            if (!await _context.Questions.AnyAsync(q => q.QuestionId == dto.QuestionId))
                return BadRequest(new { message = "Question not found" });

            var option = new OptionList
            {
                QuestionId = dto.QuestionId,
                OptionText = dto.OptionText,
                IsActive = true,
                OptionOrder = dto.OptionOrder
            };

            _context.OptionLists.Add(option);
            await _context.SaveChangesAsync();

            var result = new OptionListDto
            {
                OptionId = option.OptionId,
                QuestionId = option.QuestionId,
                OptionText = option.OptionText,
                IsActive = option.IsActive,
                OptionOrder = option.OptionOrder
            };

            return CreatedAtAction(nameof(GetOption), new { id = option.OptionId }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOption(int id, [FromBody] UpdateOptionDto dto)
        {
            var option = await _context.OptionLists.FindAsync(id);

            if (option == null)
                return NotFound();

            if (dto.OptionText != null)
                option.OptionText = dto.OptionText;
            if (dto.IsActive.HasValue)
                option.IsActive = dto.IsActive.Value;
            if (dto.OptionOrder.HasValue)
                option.OptionOrder = dto.OptionOrder.Value;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOption(int id)
        {
            var option = await _context.OptionLists.FindAsync(id);

            if (option == null)
                return NotFound();

            // Soft delete by setting IsActive to false
            option.IsActive = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
