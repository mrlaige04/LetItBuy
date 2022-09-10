namespace Shop.Models.ClientsModels
{
    public class ClientsResultModel
    {
        public ResultCodes ResultCode { get; set; }
        public IEnumerable<string> Errors { get; set; }
        
        public ClientsResultModel()
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