using Application.Services.Models;
using Domain.Core.Entities.Identities;

namespace Service.Rest.Models.Clients
{
    public class GetClientByIdResponse : BaseResponseModel
    {
        public Client Client { get; set; }
    }
}
