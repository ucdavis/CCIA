namespace CCIA.Models
{
    public partial class AlertViewModel
    {
        public AlertViewModel(string id, string title, string body, string primBtn, string secBtn)
        {
            Id = id;
            Title = title;
            Body = body;
            SecondaryBtnText = secBtn;
            PrimaryBtnText = primBtn;
        }

        public string Title { get; set; }
        public string Body { get; set; }
        public string SecondaryBtnText { get; set; }
        public string PrimaryBtnText { get; set; }
        public string Id { get; set; }
    }
}