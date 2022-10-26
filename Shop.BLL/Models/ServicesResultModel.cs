namespace Shop.BLL.Models
{
    public class ServicesResultModel
    {
        public ResultCodes ResultCode { get; set; }
        public List<string> Errors { get; set; }

        public ServicesResultModel()
        {
            Errors = new List<string>();
        }
    }
    public enum ResultCodes
    {
        Success,
        Fail
    }
}
