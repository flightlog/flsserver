namespace FLS.Server.Service.Email.Model
{
    public class LostPasswordResetModel
    {
        public string UnexpectedReturnAddress { get; set; }
        public string RecipientName { get; set; }
        public string FLSUrl { get; set; }
        public string LostPasswordResetUrl { get; set; }
    }
}
