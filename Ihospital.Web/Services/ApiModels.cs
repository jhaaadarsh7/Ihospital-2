using System.Collections.Generic;

namespace Ihospital.Web.Services
{
    public class OptionDto
    {
        public int OptionId { get; set; }
        public int QuestionId { get; set; }
        public string? OptionText { get; set; }
        public bool IsActive { get; set; }
        public int OptionOrder { get; set; }
    }

    public class QuestionDto
    {
        public int QuestionId { get; set; }
        public string? QuestionText { get; set; }
        public string? ResponseType { get; set; }
        public List<OptionDto>? Options { get; set; }
    }
}
