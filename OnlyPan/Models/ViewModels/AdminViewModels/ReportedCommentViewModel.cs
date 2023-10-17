namespace OnlyPan.Models.ViewModels.AdminViewModels;

public class ReportedCommentViewModel
{
    public int IdComment { get; set; }
    public int IdUser { get; set; }
    public int IdRecipe { get; set; }
    public string? Comment { get; set; }
    public DateTime? Date { get; set; }
    public string? UserName { get; set; }
}