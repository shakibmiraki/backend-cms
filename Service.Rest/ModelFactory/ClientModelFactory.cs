using Application.Services.Identities;
using Domain.Core;
using Domain.Core.Entities.Identities;
using Service.Rest.Models.Clients;

namespace IdentityMicroservice.ModelFactory
{
    public class ClientModelFactory : IClientModelFactory
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEncryptionService _encryptionService;

        public ClientModelFactory(IUnitOfWork unitOfWork, IEncryptionService encryptionService)
        {
            _unitOfWork = unitOfWork;
            _encryptionService = encryptionService;
        }

        public async Task<Client> PreparePostClientRequestToClient(PostClientRequest request)
        {

            var clientRole = await _unitOfWork.ClientRoles.GetAsync(request.ClientRoleId);

            var model = new Client
            {
                Active = request.Active,
                IsSystemUser = false,
                Password = _encryptionService.EncryptText(request.Password),
                Clientname = request.Clientname,
                ClientRole = clientRole
            };

            return model;
        }
    }
}