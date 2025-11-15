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
    public class RespondentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RespondentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RespondentDto>>> GetRespondents()
        {
            var respondents = await _context.Respondents
                .Select(r => new RespondentDto
                {
                    RespondentId = r.RespondentId,
                    IsAnonymous = r.IsAnonymous,
                    Title = r.Title,
                    FirstName = r.FirstName,
                    LastName = r.LastName,
                    DateOfBirth = r.DateOfBirth,
                    Gender = r.Gender,
                    ContactPhone = r.ContactPhone,
                    Email = r.Email,
                    State = r.State,
                    HomeSuburb = r.HomeSuburb,
                    HomePostcode = r.HomePostcode,
                    MacAddress = r.MacAddress,
                    CreatedDateTime = r.CreatedDateTime
                })
                .OrderByDescending(r => r.CreatedDateTime)
                .ToListAsync();

            return Ok(respondents);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<RespondentDto>> GetRespondent(int id)
        {
            var respondent = await _context.Respondents
                .Where(r => r.RespondentId == id)
                .Select(r => new RespondentDto
                {
                    RespondentId = r.RespondentId,
                    IsAnonymous = r.IsAnonymous,
                    Title = r.Title,
                    FirstName = r.FirstName,
                    LastName = r.LastName,
                    DateOfBirth = r.DateOfBirth,
                    Gender = r.Gender,
                    ContactPhone = r.ContactPhone,
                    Email = r.Email,
                    State = r.State,
                    HomeSuburb = r.HomeSuburb,
                    HomePostcode = r.HomePostcode,
                    MacAddress = r.MacAddress,
                    CreatedDateTime = r.CreatedDateTime
                })
                .FirstOrDefaultAsync();

            if (respondent == null)
                return NotFound();

            return Ok(respondent);
        }

        [HttpPost]
        public async Task<ActionResult<RespondentDto>> CreateRespondent([FromBody] CreateRespondentDto dto)
        {
            var respondent = new Respondent
            {
                IsAnonymous = dto.IsAnonymous,
                Title = dto.Title,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                ContactPhone = dto.ContactPhone,
                Email = dto.Email,
                State = dto.State,
                HomeSuburb = dto.HomeSuburb,
                HomePostcode = dto.HomePostcode,
                MacAddress = dto.MacAddress,
                CreatedDateTime = DateTime.Now
            };

            _context.Respondents.Add(respondent);
            await _context.SaveChangesAsync();

            var result = new RespondentDto
            {
                RespondentId = respondent.RespondentId,
                IsAnonymous = respondent.IsAnonymous,
                Title = respondent.Title,
                FirstName = respondent.FirstName,
                LastName = respondent.LastName,
                DateOfBirth = respondent.DateOfBirth,
                Gender = respondent.Gender,
                ContactPhone = respondent.ContactPhone,
                Email = respondent.Email,
                State = respondent.State,
                HomeSuburb = respondent.HomeSuburb,
                HomePostcode = respondent.HomePostcode,
                MacAddress = respondent.MacAddress,
                CreatedDateTime = respondent.CreatedDateTime
            };

            return CreatedAtAction(nameof(GetRespondent), new { id = respondent.RespondentId }, result);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRespondent(int id, [FromBody] UpdateRespondentDto dto)
        {
            var respondent = await _context.Respondents.FindAsync(id);

            if (respondent == null)
                return NotFound();

            if (dto.IsAnonymous.HasValue)
                respondent.IsAnonymous = dto.IsAnonymous.Value;
            if (dto.Title != null)
                respondent.Title = dto.Title;
            if (dto.FirstName != null)
                respondent.FirstName = dto.FirstName;
            if (dto.LastName != null)
                respondent.LastName = dto.LastName;
            if (dto.DateOfBirth.HasValue)
                respondent.DateOfBirth = dto.DateOfBirth;
            if (dto.Gender != null)
                respondent.Gender = dto.Gender;
            if (dto.ContactPhone != null)
                respondent.ContactPhone = dto.ContactPhone;
            if (dto.Email != null)
                respondent.Email = dto.Email
                ;
            if (dto.State != null)
                respondent.State = dto.State;
            if (dto.HomeSuburb != null)
                respondent.HomeSuburb = dto.HomeSuburb;
            if (dto.HomePostcode != null)
                respondent.HomePostcode = dto.HomePostcode;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRespondent(int id)
        {
            var respondent = await _context.Respondents.FindAsync(id);

            if (respondent == null)
                return NotFound();

            _context.Respondents.Remove(respondent);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
