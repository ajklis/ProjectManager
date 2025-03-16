namespace ProjectManager.Domain.Models
{
    public sealed class AccessToken
    {
        public Guid TokenId { get; set; }
        public int UserId { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
