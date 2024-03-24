
namespace Application.Services.Models
{
    public class ChangePasswordResult
    {
        public ChangePasswordResult()
        {
            Errors = new List<string>();
        }

        public bool Success => !Errors.Any();

        public IList<string> Errors { get; set; }


        public void AddError(string error)
        {
            Errors.Add(error);
        }


    }
}