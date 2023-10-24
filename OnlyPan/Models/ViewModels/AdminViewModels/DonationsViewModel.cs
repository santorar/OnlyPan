namespace OnlyPan.Models.ViewModels.AdminViewModels;

public class DonationsViewModel
{
    public int DonationId { get; set; }

    public string? UserName { get; set; }

    public string? ChefName { get; set; }

    public float? Amount { get; set; }

    public DateTime? Date { get; set; }

    public int? State { get; set; }
}