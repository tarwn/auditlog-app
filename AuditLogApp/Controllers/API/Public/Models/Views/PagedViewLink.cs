namespace AuditLogApp.Controllers.API.Public.Models.Views
{
    public class PagedViewLink
    {
        public PagedViewLink(string label, string type, string href)
        {
            Label = label;
            Type = type;
            Href = href;
        }

        public string Label { get; set; }
        public string Type { get; set; }
        public string Href { get; set; }
    }
}