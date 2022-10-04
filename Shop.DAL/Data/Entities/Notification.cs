using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.DAL.Data.Entities
{
    [Table("Notifications")]
    public class Notification
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public Guid UserID { get; set; }


        public string? Title { get; set; }
        public string? Message { get; set; }
    }
}
