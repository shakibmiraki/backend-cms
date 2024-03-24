namespace Service.Rest.Models.Clients
{
    public class PostClientRequest
    {
        public string Clientname { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public bool Active { get; set; }

        public long ClientRoleId { get; set; }


    }
}
