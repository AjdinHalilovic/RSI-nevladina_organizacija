using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace nevladinaOrg.Web.Helpers
{
    public class Notification
    {
        public NotificationTypes Type { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Duration { get; set; } // toastr notification duration needs to be string 

        public Notification(NotificationTypes Type, string Title, string Message, int Duration = 7000)
        {
            this.Type = Type;
            this.Title = Title;
            this.Message = Message;
            this.Duration = Duration.ToString();
        }

        public JsonResult ConvertToJson()
        {
            var notification = JsonConvert.SerializeObject(this);
            return new JsonResult(notification);
        }
    }

    public enum NotificationTypes
    {
        Success = 1,
        Warning = 2,
        Error = 3,
        Info = 4
    }
}
