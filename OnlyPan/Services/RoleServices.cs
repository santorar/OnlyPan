using Microsoft.EntityFrameworkCore;
using OnlyPan.Models;
using OnlyPan.Models.ViewModels;

namespace OnlyPan.Services;

public class RoleServices
{
  public async Task<bool> makePetition(RolPetitionViewModel model, OnlyPanContext context, int idUser)
  {
    try
    {
      var peticion = new SolicitudRol()
      {
        UsuarioSolicitud = idUser,
        Comentario = model.Comentario,
        RolSolicitado = model.Rol,
        EstadoSolicitud = 4,
        FechaSolicitud = DateTime.UtcNow,
      };

      context.Add(peticion);
      var result = await context.SaveChangesAsync();
      return true;
    }
    catch (SystemException e)
    {
      return false;
    }
  }

  public bool CheckPetitions(OnlyPanContext context, int idUser)
  {
    var request = context.SolicitudRols.FromSqlRaw("EXECUTE sp_check_petitions {0}", idUser).ToListAsync();
    if (request.Result.Count > 0)
    {
      return true;
    }
    return false;
  }

  public bool AcceptRole()
  {
    return false;
  }
}