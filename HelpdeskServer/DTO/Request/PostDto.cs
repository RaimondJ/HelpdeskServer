namespace HelpdeskServer.DTO.Request
{
    public class PostDto
    {
        public required string subject { get; set; }
        public required string description { get; set; }
        public required DateTime endDate { get; set; }
    }
}
