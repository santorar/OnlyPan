using System;
using System.Collections.Generic;

namespace OnlyPan.Models;

public partial class Recetum
{
    public int IdReceta { get; set; }

    public int? Categoria { get; set; }

    public int? Etiqueta { get; set; }

    public int? Lista { get; set; }

    public string? TituloPlato { get; set; }

    public string? Instrucciones { get; set; }

    public byte[]? Foto { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public bool? Estado { get; set; }

    public virtual Categorium? CategoriaNavigation { get; set; }

    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    public virtual Etiquetum? EtiquetaNavigation { get; set; }

    public virtual RecetaIngrediente? ListaNavigation { get; set; }

    public virtual ICollection<RecetaChef> RecetaChefs { get; set; } = new List<RecetaChef>();

    public virtual ICollection<ReplicaUsuario> ReplicaUsuarios { get; set; } = new List<ReplicaUsuario>();

    public virtual ICollection<Valoracion> Valoracions { get; set; } = new List<Valoracion>();
}
