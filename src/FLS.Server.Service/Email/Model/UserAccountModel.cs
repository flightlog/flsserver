namespace FLS.Server.Service.Email.Model
{
    public class UserAccountModel
    {
        public string UnexpectedReturnAddress { get; set; }
        public string RecipientName { get; set; }
        public string FLSUrl { get; set; }

        public string Username { get; set; }
        public string NewPassword { get; set; }
    }
}
