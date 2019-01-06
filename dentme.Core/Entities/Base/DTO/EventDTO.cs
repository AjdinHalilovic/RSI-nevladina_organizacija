namespace Core.Entities.Base.DTO
{
    public class EventDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string Place { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public int NumberOfPayments { get; set; }
        public int NumberOfRegistrations { get; set; }
    }
}
