using System.Collections;
using Microsoft.EntityFrameworkCore;
using OnlyPan.Models;
using OnlyPan.Models.Dtos.AdminDtos;

namespace OnlyPan.Repositories;

public class AdminRepositories
{
    private OnlyPanContext _context;

    public AdminRepositories(OnlyPanContext context)
    {
        _context = context;
    }

    public async Task<List<ReportedCommentDto>> RequestReportedComments()
    {
        try
        {
            var reportedComments = await _context.Comentarios.Where(x => x.Estado == 7)
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
            var comment = await _context.Comentarios.FirstOrDefaultAsync(x => x.IdComentario == idComment);
            if (comment != null) comment.Estado = 6;
            await _context.SaveChangesAsync();
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
            var comment = await _context.Comentarios.FirstOrDefaultAsync(x => x.IdComentario == idComment);
            if (comment != null) comment.Estado = 5;
            await _context.SaveChangesAsync();
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }
}