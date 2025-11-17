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
    public class SurveyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SurveyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all survey responses (for staff/admin)
        [Authorize]
        [HttpGet("responses")]
        public async Task<ActionResult<IEnumerable<SurveyResponseDto>>> GetAllResponses()
        {
            var responses = await _context.SurveyResponses
                .Include(sr => sr.Respondent)
                .Include(sr => sr.ResponseDetails)
                    .ThenInclude(rd => rd.Question)
                .Include(sr => sr.ResponseDetails)
                    .ThenInclude(rd => rd.Option)
                .OrderByDescending(sr => sr.ResponseDateTime)
                .Select(sr => new SurveyResponseDto
                {
                    SurveyResponseId = sr.SurveyResponseId,
                    RespondentId = sr.RespondentId,
                    ResponseDateTime = sr.ResponseDateTime,
                    MacAddress = sr.MacAddress,
                    Respondent = new RespondentDto
                    {
                        RespondentId = sr.Respondent.RespondentId,
                        IsAnonymous = sr.Respondent.IsAnonymous,
                        FirstName = sr.Respondent.FirstName,
                        LastName = sr.Respondent.LastName,
                        Email = sr.Respondent.Email,
                        ContactPhone = sr.Respondent.ContactPhone
                    },
                    ResponseDetails = sr.ResponseDetails.Select(rd => new ResponseDetailDto
                    {
                        ResponseDetailId = rd.ResponseDetailId,
                        SurveyResponseId = rd.SurveyResponseId,
                        QuestionId = rd.QuestionId,
                        OptionId = rd.OptionId,
                        AnswerText = rd.AnswerText,
                        CreatedDateTime = rd.CreatedDateTime,
                        QuestionText = rd.Question.QuestionText,
                        OptionText = rd.Option != null ? rd.Option.OptionText : null
                    }).ToList()
                })
                .ToListAsync();

            return Ok(responses);
        }

        // Get single survey response by ID
        [Authorize]
        [HttpGet("responses/{id}")]
        public async Task<ActionResult<SurveyResponseDto>> GetResponse(int id)
        {
            var response = await _context.SurveyResponses
                .Include(sr => sr.Respondent)
                .Include(sr => sr.ResponseDetails)
                    .ThenInclude(rd => rd.Question)
                .Include(sr => sr.ResponseDetails)
                    .ThenInclude(rd => rd.Option)
                .Where(sr => sr.SurveyResponseId == id)
                .Select(sr => new SurveyResponseDto
                {
                    SurveyResponseId = sr.SurveyResponseId,
                    RespondentId = sr.RespondentId,
                    ResponseDateTime = sr.ResponseDateTime,
                    MacAddress = sr.MacAddress,
                    Respondent = new RespondentDto
                    {
                        RespondentId = sr.Respondent.RespondentId,
                        IsAnonymous = sr.Respondent.IsAnonymous,
                        Title = sr.Respondent.Title,
                        FirstName = sr.Respondent.FirstName,
                        LastName = sr.Respondent.LastName,
                        DateOfBirth = sr.Respondent.DateOfBirth,
                        Gender = sr.Respondent.Gender,
                        Email = sr.Respondent.Email,
                        ContactPhone = sr.Respondent.ContactPhone,
                        State = sr.Respondent.State,
                        HomeSuburb = sr.Respondent.HomeSuburb,
                        HomePostcode = sr.Respondent.HomePostcode
                    },
                    ResponseDetails = sr.ResponseDetails.Select(rd => new ResponseDetailDto
                    {
                        ResponseDetailId = rd.ResponseDetailId,
                        SurveyResponseId = rd.SurveyResponseId,
                        QuestionId = rd.QuestionId,
                        OptionId = rd.OptionId,
                        AnswerText = rd.AnswerText,
                        CreatedDateTime = rd.CreatedDateTime,
                        QuestionText = rd.Question.QuestionText,
                        OptionText = rd.Option != null ? rd.Option.OptionText : null
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        // Get responses by respondent
        [Authorize]
        [HttpGet("responses/respondent/{respondentId}")]
        public async Task<ActionResult<IEnumerable<SurveyResponseDto>>> GetResponsesByRespondent(int respondentId)
        {
            var responses = await _context.SurveyResponses
                .Include(sr => sr.Respondent)
                .Include(sr => sr.ResponseDetails)
                    .ThenInclude(rd => rd.Question)
                .Include(sr => sr.ResponseDetails)
                    .ThenInclude(rd => rd.Option)
                .Where(sr => sr.RespondentId == respondentId)
                .OrderByDescending(sr => sr.ResponseDateTime)
                .Select(sr => new SurveyResponseDto
                {
                    SurveyResponseId = sr.SurveyResponseId,
                    RespondentId = sr.RespondentId,
                    ResponseDateTime = sr.ResponseDateTime,
                    MacAddress = sr.MacAddress,
                    ResponseDetails = sr.ResponseDetails.Select(rd => new ResponseDetailDto
                    {
                        ResponseDetailId = rd.ResponseDetailId,
                        SurveyResponseId = rd.SurveyResponseId,
                        QuestionId = rd.QuestionId,
                        OptionId = rd.OptionId,
                        AnswerText = rd.AnswerText,
                        CreatedDateTime = rd.CreatedDateTime,
                        QuestionText = rd.Question.QuestionText,
                        OptionText = rd.Option != null ? rd.Option.OptionText : null
                    }).ToList()
                })
                .ToListAsync();

            return Ok(responses);
        }

        // Submit a complete survey (public endpoint - no auth required)
        [HttpPost("submit")]
        public async Task<ActionResult<SurveyResponseDto>> SubmitSurvey([FromBody] SubmitSurveyDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                int respondentId;

                // Create or use existing respondent
                if (dto.RespondentId.HasValue)
                {
                    respondentId = dto.RespondentId.Value;
                    
                    // Verify respondent exists
                    if (!await _context.Respondents.AnyAsync(r => r.RespondentId == respondentId))
                        return BadRequest(new { message = "Respondent not found" });
                }
                else if (dto.Respondent != null)
                {
                    // Create new respondent
                    var respondent = new Respondent
                    {
                        IsAnonymous = dto.Respondent.IsAnonymous,
                        Title = dto.Respondent.Title,
                        FirstName = dto.Respondent.FirstName,
                        LastName = dto.Respondent.LastName,
                        DateOfBirth = dto.Respondent.DateOfBirth,
                        Gender = dto.Respondent.Gender,
                        ContactPhone = dto.Respondent.ContactPhone,
                        Email = dto.Respondent.Email,
                        State = dto.Respondent.State,
                        HomeSuburb = dto.Respondent.HomeSuburb,
                        HomePostcode = dto.Respondent.HomePostcode,
                        MacAddress = dto.Respondent.MacAddress,
                        CreatedDateTime = DateTime.Now,
                        HasPrivateInsurance = dto.Respondent.HasPrivateInsurance,
                        InsuranceProviders = dto.Respondent.InsuranceProviders,
                        DischargePlans = dto.Respondent.DischargePlans,
                        StayPeriod = dto.Respondent.StayPeriod,
                        WifiPlan = dto.Respondent.WifiPlan,
                        WifiSatisfaction = dto.Respondent.WifiSatisfaction
                    };

                    _context.Respondents.Add(respondent);
                    await _context.SaveChangesAsync();
                    respondentId = respondent.RespondentId;
                }
                else
                {
                    return BadRequest(new { message = "Either RespondentId or Respondent information must be provided" });
                }

                // Create survey response
                var surveyResponse = new SurveyResponse
                {
                    RespondentId = respondentId,
                    ResponseDateTime = DateTime.Now,
                    MacAddress = dto.MacAddress
                };

                _context.SurveyResponses.Add(surveyResponse);
                await _context.SaveChangesAsync();

                // Create response details
                foreach (var response in dto.Responses)
                {
                    // If OptionId not provided but AnswerText is present, try to map it to an Option_ID
                    int? resolvedOptionId = response.OptionId;
                    if (!resolvedOptionId.HasValue && !string.IsNullOrEmpty(response.AnswerText))
                    {
                        // Try to find an option matching the answer text for the question
                        var opt = await _context.OptionLists
                            .Where(o => o.QuestionId == response.QuestionId && o.OptionText == response.AnswerText)
                            .FirstOrDefaultAsync();

                        if (opt != null)
                        {
                            resolvedOptionId = opt.OptionId;
                        }
                    }

                    var responseDetail = new ResponseDetail
                    {
                        SurveyResponseId = surveyResponse.SurveyResponseId,
                        QuestionId = response.QuestionId,
                        OptionId = resolvedOptionId,
                        AnswerText = response.AnswerText,
                        CreatedDateTime = DateTime.Now
                    };

                    _context.ResponseDetails.Add(responseDetail);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Return success message with survey response ID
                return Ok(new { 
                    message = "Survey submitted successfully",
                    surveyResponseId = surveyResponse.SurveyResponseId,
                    respondentId = respondentId
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { message = "Error submitting survey", error = ex.Message });
            }
        }

        // Delete a survey response
        [Authorize]
        [HttpDelete("responses/{id}")]
        public async Task<IActionResult> DeleteResponse(int id)
        {
            var response = await _context.SurveyResponses.FindAsync(id);

            if (response == null)
                return NotFound();

            _context.SurveyResponses.Remove(response);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Get survey statistics
        [Authorize]
        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetStatistics()
        {
            var totalResponses = await _context.SurveyResponses.CountAsync();
            var totalRespondents = await _context.Respondents.CountAsync();
            var anonymousCount = await _context.Respondents.CountAsync(r => r.IsAnonymous);
            var totalQuestions = await _context.Questions.CountAsync(q => q.IsActive);

            var recentResponses = await _context.SurveyResponses
                .OrderByDescending(sr => sr.ResponseDateTime)
                .Take(5)
                .Select(sr => new
                {
                    sr.SurveyResponseId,
                    sr.ResponseDateTime,
                    RespondentName = sr.Respondent.IsAnonymous 
                        ? "Anonymous" 
                        : $"{sr.Respondent.FirstName} {sr.Respondent.LastName}",
                    ResponseCount = sr.ResponseDetails.Count
                })
                .ToListAsync();

            return Ok(new
            {
                totalResponses,
                totalRespondents,
                anonymousCount,
                totalQuestions,
                recentResponses
            });
        }
    }
}
