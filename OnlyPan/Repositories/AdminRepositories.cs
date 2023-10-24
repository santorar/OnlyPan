using System.Collections;
using Microsoft.EntityFrameworkCore;
using OnlyPan.Models;
using OnlyPan.Models.Dtos.AdminDtos;

namespace OnlyPan.Repositories;

public class AdminRepositories
{
    private OnlyPanDbContext _dbContext;

    public AdminRepositories(OnlyPanDbContext dbContext)
    {
        _dbContext = dbContext;
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
}