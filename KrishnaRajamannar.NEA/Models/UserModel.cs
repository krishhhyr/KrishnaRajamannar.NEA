namespace KrishnaRajamannar.NEA.Models
{
    // This class defines the properties of a user only.
    public class UserModel
    {
        public int? UserID { get; set; }
        public string? Username { get; set; }
        public string? HashedPassword { get; set; }
        public int? TotalPoints { get; set; }
    }
}
