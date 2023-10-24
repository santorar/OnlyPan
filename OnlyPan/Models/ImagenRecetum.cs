using System;
using System.Collections.Generic;

namespace OnlyPan.Models;

public partial class ImagenRecetum
{
    public int IdImagen { get; set; }

    public int IdReceta { get; set; }

    public byte[]? Imagen { get; set; }
}
