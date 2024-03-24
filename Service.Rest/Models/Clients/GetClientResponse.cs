using Application.Services.Models;
using Domain.Core.Entities.Identities;
using Infrastructure.Data.EntityFramework.PageList;

namespace Service.Rest.Models.Clients
{
    public class GetClientResponse : BaseResponseModel
    {
        public IPagedList<Client> Clients { get; set; }

        public int TotalCount { get; set; }

        public int Page { get; set; }
    }
}
