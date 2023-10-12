using System;
using System.Collections.Generic;

namespace OnlyPan.Models.Dtos;

public class CategoriaDto
{
    public int IdCategoria { get; set; }

    public string? NombreCategoria { get; set; }

    public string? DescripcionCategoria { get; set; }

    public virtual ICollection<Recetum> Receta { get; set; } = new List<Recetum>();
}
