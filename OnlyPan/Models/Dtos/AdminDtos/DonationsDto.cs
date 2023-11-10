namespace OnlyPan.Models.Dtos.AdminDtos;

public class DonationsDto
{
    public int DonationId { get; set; }

    public string? UserName { get; set; }

    public string? ChefName { get; set; }

    public float? Amount { get; set; }

    public DateTime? Date { get; set; }

    public byte[] Photo { get; set; }
    public int? State { get; set; }
}