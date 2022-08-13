using Traffic.Utilities;

namespace Traffic.Application.Models
{
    public class ResponseModel
    {
        public ResponseModel()
        {
            Status = Enums.Status.SUCCESS.ToString();
        }
        public string Status { get; set; }
        public string Description { get; set; }
    }
}
