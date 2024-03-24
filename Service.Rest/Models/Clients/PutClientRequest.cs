
namespace Service.Rest.Models.Clients
{
    public class PutClientRequest
    {
        public string Clientname { get; set; }

        public bool Active { get; set; }

        public long ClientRoleId { get; set; }
    }
}
