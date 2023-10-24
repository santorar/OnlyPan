using OnlyPan.Models;
using OnlyPan.Models.Dtos;
using OnlyPan.Models.Dtos.RoleDtos;
using OnlyPan.Models.ViewModels.RolViewModels;
using OnlyPan.Repositories;

namespace OnlyPan.Services;

public class RoleServices
{
    private readonly RoleRepository _roleRepository;
    private readonly UserRepository _userRepository;

    public RoleServices(OnlyPanDbContext dbContext)
    {
        _roleRepository = new RoleRepository(dbContext);
        _userRepository = new UserRepository(dbContext);
    }

    public async Task<List<RoleDto>> GetRoles()
    {
        try
        {
            return await _roleRepository.RequestRoles();
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<bool> MakePetition(RoleMakePetitionViewModel model, int idUser)
    {
        bool result = await _roleRepository.CreatePetition(model, idUser);
        if (result)
        {
            List<string> emails = await _roleRepository.RequestAdminsEmails();
            EmailService em = new EmailService();
            string user = await _userRepository.RequestUserName(idUser);
            string role = await _roleRepository.RequestRoleName(model.Role);
            foreach (var email in emails)
            {
                await em.SendNotificacionRequestRole(email, user, role);
            }
        }

        return result;
    }

    public async Task<bool> CheckPetitions(int idUser, int idRole)
    {
        return await _roleRepository.RequestUserPetitions(idUser, idRole);
    }

    public async Task<List<RolePetitionViewModel>> GetPetitions()
    {
        try
        {
            List<RolePetitionDto> rolePetitionDtos = await _roleRepository.RequestPetitions();
            List<RolePetitionViewModel> resultSet = new List<RolePetitionViewModel>();
            foreach (var rolePetitionDto in rolePetitionDtos)
            {
                var rolePetitionViewModel = new RolePetitionViewModel()
                {
                    IdUser = rolePetitionDto.IdUser,
                    UserName = rolePetitionDto.UserName,
                    CurrentRoleName = rolePetitionDto.CurrentRoleName,
                    IdRequesedRole = rolePetitionDto.IdRequesedRole,
                    RequesedRoleName = rolePetitionDto.RequesedRoleName,
                    Time = rolePetitionDto.Time
                };
                resultSet.Add(rolePetitionViewModel);
            }

            return resultSet;
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<bool> AcceptPetition(int idUser, int idRole, int idUserAdmin)
    {
        return await _roleRepository.AcceptPetition(idUser, idRole, idUserAdmin);
    }

    public async Task<bool> RejectPetition(int idUser, int idRole, int idUserAdmin)
    {
        return await _roleRepository.RejectPetition(idUser, idRole, idUserAdmin);
    }
}