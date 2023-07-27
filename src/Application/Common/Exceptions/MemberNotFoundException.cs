namespace Application.Common.Exceptions
{
    public class MemberNotFoundException : Exception
    {
        public MemberNotFoundException(string type, string memberName)
            : base($"Type \"{type}\" doesn't have member with name ({memberName})")
        { 
        
        }
    }
}
