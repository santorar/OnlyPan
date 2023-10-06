using Microsoft.EntityFrameworkCore;
using OnlyPan.Models;
using OnlyPan.Models.ViewModels;

namespace OnlyPan.Services;

public class RoleServices
{
  public async Task<bool> MakePetition(RolPetitionViewModel model, OnlyPanContext context, int idUser)
  {
    try
    {
      var peticion = new SolicitudRol()
      {
        UsuarioSolicitud = idUser,
        RolSolicitado = model.Role,
        EstadoSolicitud = 4,
        //TODO fix the error that is showing always the time in 12 AM
        FechaSolicitud = DateTime.UtcNow
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
}