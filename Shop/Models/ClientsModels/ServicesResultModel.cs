namespace Shop.Models.ClientsModels
{
    public class ServicesResultModel
    {
        public ResultCodes ResultCode { get; set; }
        public IEnumerable<string> Errors { get; set; }
        
        public ServicesResultModel()
        {
            Errors = new List<string>();
        }
    }
    public enum ResultCodes
    {
        Successed, 
        Failed
    }
}