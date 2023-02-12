using Project.Web.ViewModels;

namespace Project.Web.Responses
{
    public class AuthenticateResponse
    {
        public string AccessToken { get; set; }
        public UserViewModel User { get; set; }
    }
}
