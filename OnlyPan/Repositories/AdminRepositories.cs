using System.Collections;
using Microsoft.EntityFrameworkCore;
using OnlyPan.Models;
using OnlyPan.Models.Dtos;
using OnlyPan.Models.Dtos.AdminDtos;

namespace OnlyPan.Repositories;

public class AdminRepositories
{
    private OnlyPanDbContext _dbContext;

    public AdminRepositories(OnlyPanDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<StateDto>> RequestStates(int indicator)
    {
        try
        {
            List<Estado> states;
            if (indicator == 0)
                states = await _dbContext.Estados.Where(e => e.IdEstado <= 3).ToListAsync();
            else
                states = await _dbContext.Estados.Where(e => e.IdEstado > 3 && e.IdEstado <= 6).ToListAsync();
            List<StateDto> result = new List<StateDto>();
            foreach (var state in states)
            {
                StateDto stateDto = new StateDto()
                {
                    IdState = state.IdEstado,
                    State = state.NombreEstado
                };
                result.Add(stateDto);
            }

            return result;
        }
        catch (Exception)
        {
            return null!;
        }
    }

    public async Task<List<ReportedCommentDto>> RequestReportedComments()
    {
        try
        {
            var reportedComments = await _dbContext.Comentarios.Where(x => x.Estado == 7)
                .Include(x => x.IdUsuarioNavigation).ToListAsync();
            List<ReportedCommentDto> result = new List<ReportedCommentDto>();
            foreach (var reportedComment in reportedComments)
            {
                ReportedCommentDto reportedCommentDto = new ReportedCommentDto()
                {
                    Comment = reportedComment.Comentario1,
                    Date = reportedComment.Fecha,
                    IdComment = reportedComment.IdComentario,
                    IdRecipe = reportedComment.IdReceta,
                    IdUser = reportedComment.IdUsuario,
                    UserName = reportedComment.IdUsuarioNavigation.Nombre,
                };
                result.Add(reportedCommentDto);
            }

            return result;
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<bool> BlockComment(int idComment)
    {
        try
        {
            var comment = await _dbContext.Comentarios.FirstOrDefaultAsync(x => x.IdComentario == idComment);
            if (comment != null) comment.Estado = 6;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<bool> AcceptComment(int idComment)
    {
        try
        {
            var comment = await _dbContext.Comentarios.FirstOrDefaultAsync(x => x.IdComentario == idComment);
            if (comment != null) comment.Estado = 5;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<List<DonationsDto>> RequestDonations()
    {
        try
        {
            var donations = await _dbContext.Donacions.Where(d => d.Estado == 4).Include(x => x.IdChefNavigation)
                .Include(x => x.IdUsuarioNavigation).ToListAsync();
            List<DonationsDto> result = new List<DonationsDto>();
            foreach (var donation in donations)
            {
                DonationsDto donationsDto = new DonationsDto()
                {
                    Amount = (float?)donation.Monto,
                    ChefName = donation.IdChefNavigation.Nombre,
                    Date = donation.Fecha,
                    DonationId = donation.IdDonacion,
                    State = donation.Estado,
                    UserName = donation.IdUsuarioNavigation.Nombre,
                    Photo = donation.Imagen
                };
                result.Add(donationsDto);
            }

            return result;
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<bool> AcceptDonation(int donationId)
    {
        try
        {
            var donation = await _dbContext.Donacions.FirstOrDefaultAsync(x => x.IdDonacion == donationId);
            if (donation != null) donation.Estado = 5;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<bool> BlockDonation(int donationId)
    {
        try
        {
            var donation = await _dbContext.Donacions.FirstOrDefaultAsync(x => x.IdDonacion == donationId);
            if (donation != null) donation.Estado = 6;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<List<RecipeModerateDto>> RequestRecipes(int idState)
    {
        try
        {
            var recipes = await _dbContext.Receta.Include(r => r.IdEstadoNavigation).Where(r => r.IdEstado == idState).ToListAsync();
            List<RecipeModerateDto> result = new List<RecipeModerateDto>();
            foreach (var recipe in recipes)
            {
                var recipeChef = await _dbContext.RecetaChefs
                    .Where(r => r.IdReceta == recipe.IdReceta)
                    .Include(r => r.IdChefNavigation)
                    .FirstOrDefaultAsync();
                RecipeModerateDto recipeModerateDto = new RecipeModerateDto()
                {
                    IdRecipe = recipe.IdReceta,
                    Name = recipe.NombreReceta,
                    ChefName = recipeChef!.IdChefNavigation.Nombre,
                    State = recipe.IdEstadoNavigation!.NombreEstado,
                    Date = recipe.FechaCreacion
                };
                result.Add(recipeModerateDto);
            }

            return result;
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<bool> AcceptRecipe(int idRecipe)
    {
        try
        {
            var recipe = await _dbContext.Receta.FirstOrDefaultAsync(x => x.IdReceta == idRecipe);
            if (recipe != null) recipe.IdEstado = 5;
            else throw new SystemException();
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<bool> BlockRecipe(int idRecipe)
    {
        try
        {
            var recipe = await _dbContext.Receta.FirstOrDefaultAsync(x => x.IdReceta == idRecipe);
            if (recipe != null) recipe.IdEstado = 6;
            else throw new SystemException();
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }
}