using System;
using System.Collections.Generic;

namespace OnlyPan.Models.Dtos;

public class DonacionDto
{
    public int IdDonacion { get; set; }

    public int? IdUsuario { get; set; }

    public decimal? Monto { get; set; }

    public DateTime? Fecha { get; set; }

    public int? Estado { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
