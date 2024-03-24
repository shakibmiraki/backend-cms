namespace Service.Rest.Models.Roles
{
    public class PutClientRoleRequest
    {
        public string Name { get; set; }

        public string SystemName { get; set; }

        public bool Active { get; set; }

        public IList<long> PermissionIds { get; set; }
    }
}
