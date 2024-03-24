namespace Application.Services.Models
{
    public class LoginResponse : BaseResponseModel
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
