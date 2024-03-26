namespace HelpdeskServer.DTO.Response
{
    public class PostInfoDto
    {
        public DateTime beginDate { get; set; }
        public DateTime endDate { get; set; }
        public DateTime timeLeft { get; set; }
        public string subject { get; set; }
        public string description { get; set; }
        public bool isExpiring { get; set; }
    }
}
