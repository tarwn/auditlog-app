namespace AuditLogApp.Models.Health
{
    public class StatusModelDetail
    {
        public StatusModelDetail(StatusEnum status, string details)
        {
            Status = status;
            Details = details;
        }

        public StatusEnum Status { get; set; }

        public string Details { get; set; }
    }
}