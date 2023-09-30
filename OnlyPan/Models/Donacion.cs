namespace OnlyPan.Models;

public class Donacion
{
  public int IdDonacion { get; set; }

  public int? Donador { get; set; }

  public decimal? Monto { get; set; }

  public DateTime? FechaDonacion { get; set; }

  public virtual Usuario? DonadorNavigation { get; set; }
}