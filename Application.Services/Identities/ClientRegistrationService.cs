using Application.Services.Models;
using Domain.Core;
using Domain.Core.Entities.Identities;

namespace Application.Services.Identities
{

    public partial class ClientRegistrationService : IClientRegistrationService
    {

        private readonly IClientService _clientService;
        private readonly IEncryptionService _encryptionService;
        private readonly IUnitOfWork _unitOfWork;



        public ClientRegistrationService(
            IClientService clientService,
            IEncryptionService encryptionService, IUnitOfWork unitOfWork)
        {
            _clientService = clientService;
            _encryptionService = encryptionService;
            _unitOfWork = unitOfWork;
        }



        public async Task<ClientLoginResults> LoginClient(string clientname, string password)
        {
            var user = await _clientService.GetClientByClientname(clientname);

            if (user == null)
                return ClientLoginResults.CustomerNotExist;
            if (user.Deleted)
                return ClientLoginResults.Deleted;
            if (!user.Active)
                return ClientLoginResults.NotActive;
            if (user.Password != _encryptionService.EncryptText(password))
                return ClientLoginResults.WrongPassword;

            return ClientLoginResults.Successful;
        }

        public async Task<string> ValidateClient(Client request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = "";

            if (string.IsNullOrEmpty(request.Clientname))
            {
                result = "نام کاربری معتبر نمی باشد";
                return result;
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                result = "رمز عبور معتبر نمی باشد";
                return result;
            }

            var user = await _clientService.GetClientByClientname(request.Clientname);
            if (user != null)
            {
                result = "این نام کاربری قبلا ثبت شده است";
                return result;
            }

            return result;
        }

        public async Task<ChangePasswordResult> ChangePassword(string clientname, string oldPassword, string newPassword)
        {

            var result = new ChangePasswordResult();
            if (string.IsNullOrWhiteSpace(clientname))
            {
                result.AddError("نام کاربری معتبر نمی باشد");
            }

            if (string.IsNullOrWhiteSpace(oldPassword))
            {
                result.AddError("پسورد قدیمی معتبر نمی باشد");
                return result;
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                result.AddError("پسورد جدید معتبر نمی باشد");
                return result;
            }

            var client = await _clientService.GetClientByClientname(clientname);
            if (client == null)
            {
                result.AddError("نام کاربری در سیستم موجود نیست");
                return result;
            }


            client.Password = _encryptionService.EncryptText(newPassword);
            _unitOfWork.Clients.Update(client);
            await _unitOfWork.CommitAsync();

            return result;
        }

    }
}