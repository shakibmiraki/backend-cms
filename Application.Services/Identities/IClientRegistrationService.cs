using Application.Services.Models;
using Domain.Core.Entities.Identities;

namespace Application.Services.Identities
{
    public partial interface IClientRegistrationService
    {

        Task<ClientLoginResults> LoginClient(string clientname, string password);

        Task<string> ValidateClient(Client request);

        Task<ChangePasswordResult> ChangePassword(string clientname, string oldPassword, string newPassword);
    }

}