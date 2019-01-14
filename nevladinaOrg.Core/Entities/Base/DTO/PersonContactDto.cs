namespace Core.Entities.Base.DTO
{
    public class PersonContactDTO
    {
        public int Id { get; set; }
        public string Contact { get; set; }
        public int ContactTypeId { get; set; }
        public bool Primary { get; set; }
    }
}
