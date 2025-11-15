namespace Ihospital.API.DTOs
{
    public class SurveyResponseDto
    {
        public int SurveyResponseId { get; set; }
        public int RespondentId { get; set; }
        public DateTime ResponseDateTime { get; set; }
        public string? MacAddress { get; set; }
        public RespondentDto? Respondent { get; set; }
        public List<ResponseDetailDto>? ResponseDetails { get; set; }
    }

    public class CreateSurveyResponseDto
    {
        public int RespondentId { get; set; }
        public string? MacAddress { get; set; }
        public List<CreateResponseDetailDto> ResponseDetails { get; set; } = new();
    }

    public class ResponseDetailDto
    {
        public int ResponseDetailId { get; set; }
        public int SurveyResponseId { get; set; }
        public int QuestionId { get; set; }
        public int? OptionId { get; set; }
        public string? AnswerText { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? QuestionText { get; set; }
        public string? OptionText { get; set; }
    }

    public class CreateResponseDetailDto
    {
        public int QuestionId { get; set; }
        public int? OptionId { get; set; }
        public string? AnswerText { get; set; }
    }

    public class SubmitSurveyDto
    {
        public CreateRespondentDto? Respondent { get; set; }
        public int? RespondentId { get; set; }
        public string? MacAddress { get; set; }
        public List<CreateResponseDetailDto> Responses { get; set; } = new();
    }
}
