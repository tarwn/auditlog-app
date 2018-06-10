namespace AuditLogApp.Membership
{
    public class MembershipOptions
    {
        public string DefaultPathAfterLogin { get; set; }
        public string DefaultPathAfterLogout { get; set; }

        public string InteractiveAuthenticationScheme { get; set; }
        public string APIAuthenticationScheme { get; internal set; }
    }
}