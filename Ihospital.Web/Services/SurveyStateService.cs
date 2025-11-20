namespace Ihospital.Web.Services
{
    public interface ISurveyStateService
    {
        string? Title { get; set; }
        string? Email { get; set; }
        string? Gender { get; set; }
        string? AgeRange { get; set; }
        string? State { get; set; }
        string? HomeSuburb { get; set; }
        string? HomePostcode { get; set; }
        List<string>? ServiceTypes { get; set; }
        string? RoomType { get; set; }
        List<string>? RoomFacilities { get; set; }
        bool? HasPrivateInsurance { get; set; }
        List<string>? InsuranceProviders { get; set; }
        List<string>? DischargePlans { get; set; }
        string? StayPeriod { get; set; }
        List<string>? WifiServicesUsed { get; set; }
        string? WifiSatisfaction { get; set; }
        void Clear();
    }

    public class SurveyStateService : ISurveyStateService
    {
        public string? Title { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public string? AgeRange { get; set; }
        public string? State { get; set; }
        public string? HomeSuburb { get; set; }
        public string? HomePostcode { get; set; }
        public List<string>? ServiceTypes { get; set; }
        public string? RoomType { get; set; }
        public List<string>? RoomFacilities { get; set; }
        public bool? HasPrivateInsurance { get; set; }
        public List<string>? InsuranceProviders { get; set; }
        public List<string>? DischargePlans { get; set; }
        public string? StayPeriod { get; set; }
        public List<string>? WifiServicesUsed { get; set; }
        public string? WifiSatisfaction { get; set; }

        public void Clear()
        {
            Title = null;
            Email = null;
            Gender = null;
            AgeRange = null;
            State = null;
            HomeSuburb = null;
            HomePostcode = null;
            ServiceTypes = null;
            RoomType = null;
            RoomFacilities = null;
            HasPrivateInsurance = null;
            InsuranceProviders = null;
            DischargePlans = null;
            StayPeriod = null;
            WifiServicesUsed = null;
            WifiSatisfaction = null;
        }
    }
}
