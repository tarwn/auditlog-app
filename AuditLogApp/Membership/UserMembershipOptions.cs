namespace AuditLogApp.Membership
{
    public class UserMembershipOptions
    {
        public string DefaultPathAfterLogin { get; set; }
        public string DefaultPathAfterLogout { get; set; }

        public string InteractiveAuthenticationType { get; set; }
    }
}