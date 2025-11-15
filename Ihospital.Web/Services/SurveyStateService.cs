namespace Ihospital.Web.Services
{
    public interface ISurveyStateService
    {
        string? Gender { get; set; }
        string? AgeRange { get; set; }
        string? State { get; set; }
        string? HomeSuburb { get; set; }
        string? HomePostcode { get; set; }
        List<string>? ServiceTypes { get; set; }
        string? RoomType { get; set; }
        List<string>? RoomFacilities { get; set; }
        void Clear();
    }

    public class SurveyStateService : ISurveyStateService
    {
        public string? Gender { get; set; }
        public string? AgeRange { get; set; }
        public string? State { get; set; }
        public string? HomeSuburb { get; set; }
        public string? HomePostcode { get; set; }
        public List<string>? ServiceTypes { get; set; }
        public string? RoomType { get; set; }
        public List<string>? RoomFacilities { get; set; }

        public void Clear()
        {
            Gender = null;
            AgeRange = null;
            State = null;
            HomeSuburb = null;
            HomePostcode = null;
            ServiceTypes = null;
            RoomType = null;
            RoomFacilities = null;
        }
    }
}
