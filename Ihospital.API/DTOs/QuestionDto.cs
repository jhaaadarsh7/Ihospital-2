namespace Ihospital.API.DTOs
{
    public class QuestionDto
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string? QuestionCode { get; set; }
        public string? QuestionCategory { get; set; }
        public string? ResponseType { get; set; }
        public bool IsActive { get; set; }
        public bool IsRequired { get; set; }
        public int? MaxSelections { get; set; }
        public int DisplayOrder { get; set; }
        public List<OptionListDto>? Options { get; set; }
    }

    public class CreateQuestionDto
    {
        public string QuestionText { get; set; } = string.Empty;
        public string? QuestionCode { get; set; }
        public string? QuestionCategory { get; set; }
        public string? ResponseType { get; set; }
        public bool IsRequired { get; set; }
        public int? MaxSelections { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class UpdateQuestionDto
    {
        public string? QuestionText { get; set; }
        public string? QuestionCode { get; set; }
        public string? QuestionCategory { get; set; }
        public string? ResponseType { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsRequired { get; set; }
        public int? MaxSelections { get; set; }
        public int? DisplayOrder { get; set; }
    }

    public class OptionListDto
    {
        public int OptionId { get; set; }
        public int QuestionId { get; set; }
        public string OptionText { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int OptionOrder { get; set; }
    }

    public class CreateOptionDto
    {
        public int QuestionId { get; set; }
        public string OptionText { get; set; } = string.Empty;
        public int OptionOrder { get; set; }
    }

    public class UpdateOptionDto
    {
        public string? OptionText { get; set; }
        public bool? IsActive { get; set; }
        public int? OptionOrder { get; set; }
    }
}
