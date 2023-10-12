using System;
using System.Collections.Generic;

namespace OnlyPan.Models;

public partial class Recetum
{
    public int IdReceta { get; set; }

    public int? IdCategoria { get; set; }

    public int? IdEtiqueta { get; set; }

    public string? NombreReceta { get; set; }

    public string? Instrucciones { get; set; }

    public byte[]? Foto { get; set; }

    public string? DescripcionReceta { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public int? IdEstado { get; set; }

    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    public virtual Categorium? IdCategoriaNavigation { get; set; }

    public virtual Estado? IdEstadoNavigation { get; set; }

    public virtual Etiquetum? IdEtiquetaNavigation { get; set; }

    public virtual ICollection<RecetaChef> RecetaChefs { get; set; } = new List<RecetaChef>();

    public virtual ICollection<RecetaIngrediente> RecetaIngredientes { get; set; } = new List<RecetaIngrediente>();

    public virtual ReplicaUsuario? ReplicaUsuario { get; set; }

    public virtual ICollection<Valoracion> Valoracions { get; set; } = new List<Valoracion>();
}
