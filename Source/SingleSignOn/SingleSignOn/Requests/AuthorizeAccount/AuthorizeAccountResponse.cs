namespace SingleSignOn.Requests.AuthorizeAccount
{
    public class AuthorizeAccountResponse
    {
        public string Token { get; set; }

        public AuthorizeAccountResponse(string token)
        {
            Token = token;
        }
    }
}
