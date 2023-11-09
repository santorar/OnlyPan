using System;
using System.Collections.Generic;

namespace OnlyPan.Models;

public partial class Donacion
{
    public int IdDonacion { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdChef { get; set; }

    public decimal? Monto { get; set; }

    public DateTime? Fecha { get; set; }

    public int? Estado { get; set; }

    public byte[]? Imagen { get; set; }

    public virtual Usuario? IdChefNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
