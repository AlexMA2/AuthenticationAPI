using System.ComponentModel.DataAnnotations;

namespace AuthenticationProject.Models
{
    public class UserMetadata
    {
        [Key]
        public int Id { get; set; }
        public DateTime SignUpDate { get; set; }
        public DateTime LastTimeSignIn { get; set; }
        public bool IsLogged { get; set; }
        public int HoursSpentInTheApp { get; set; } = 0;
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
