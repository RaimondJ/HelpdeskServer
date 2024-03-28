using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace HelpdeskServer.DTO.Response
{
    public class PostInfoDto
    {
        public int id { get; set; }
        public string beginDate { get; set; }
        public string endDate { get; set; }
        public string timeLeft { get; set; }
        public string subject { get; set; }
        public string description { get; set; }
        public bool isExpiring { get; set; }
    }
}
