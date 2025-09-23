public class BaseAuditableEntity<TKey> where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public required string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}
