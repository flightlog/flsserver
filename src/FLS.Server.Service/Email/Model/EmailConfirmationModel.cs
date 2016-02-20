namespace FLS.Server.Service.Email.Model
{
    public class EmailConfirmationModel
    {
        public string UnexpectedReturnAddress { get; set; }
        public string RecipientName { get; set; }
        public string FLSUrl { get; set; }

        public string Username { get; set; }

        public string EmailConfirmationUrl { get; set; }
    }
}
