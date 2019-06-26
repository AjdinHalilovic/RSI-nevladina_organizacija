
namespace Core.Entities.Base.DTO
{
    public class InstitutionDTO
    {
        public int Id { get; set; }
        public string Parent { get; set; }
        public string InstitutionType { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string WebsiteURL { get; set; }
        public string SocialURL { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public byte[] Logo { get; set; }
        public string AdditionalInformation { get; set; }
        public bool Active { get; set; }
    }
}
