using Banking.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Entities.Parties;

public class IdentificationDocument
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid InvolvedPartyId { get; set; }

    [Required]
    public IdentificationDocumentType Type { get; set; }

    [Required]
    public string DocumentNumber { get; set; }

    [Required]
    public DateTime IssueDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    [Required]
    public string IssuingCountry { get; set; } // ISO 3166-1 alpha-2 recommended

    public string? IssuingAuthority { get; set; }

    public string? ScanUrl { get; set; } // Optional link to document image or PDF

    [Required]
    public bool IsVerified { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? VerifiedAt { get; set; }

    public Guid? VerifiedByUserId { get; set; }

    public string? Notes { get; set; }
}
