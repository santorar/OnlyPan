using OnlyPan.Models;
using OnlyPan.Models.ViewModels.AdminViewModels;
using OnlyPan.Repositories;

namespace OnlyPan.Services;

public class AdminServices
{
    private AdminRepositories _adminRepositories;

    public AdminServices(OnlyPanDbContext dbContext)
    {
        _adminRepositories = new AdminRepositories(dbContext);
    }

    public async Task<List<ReportedCommentViewModel>> GetReportedComments()
    {
        try
        {
            var reportedComments = await _adminRepositories.RequestReportedComments();
            List<ReportedCommentViewModel> result = new List<ReportedCommentViewModel>();
            foreach (var reportedComment in reportedComments)
            {
                ReportedCommentViewModel reportedCommentViewModelDto = new ReportedCommentViewModel()
                {
                    Comment = reportedComment.Comment,
                    Date = reportedComment.Date,
                    IdComment = reportedComment.IdComment,
                    IdRecipe = reportedComment.IdRecipe,
                    IdUser = reportedComment.IdUser,
                    UserName = reportedComment.UserName
                };
                result.Add(reportedCommentViewModelDto);
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
        return await _adminRepositories.BlockComment(idComment);
    }

    public async Task<bool> AcceptComment(int idComment)
    {
        return await _adminRepositories.AcceptComment(idComment);
    }

    public async Task<List<DonationsViewModel>> GetDonations()
    {
        try
        {
            var donations = await _adminRepositories.RequestDonations();
            List<DonationsViewModel> result = new List<DonationsViewModel>();
            foreach (var donation in donations)
            {
                DonationsViewModel donationsViewModelDto = new DonationsViewModel()
                {
                    DonationId = donation.DonationId,
                    Amount = donation.Amount,
                    ChefName = donation.ChefName,
                    Date = donation.Date,
                    State = donation.State,
                    UserName = donation.UserName
                };
                result.Add(donationsViewModelDto);
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
        return await _adminRepositories.AcceptDonation(donationId);
    }
    public async Task<bool> BlockDonation(int donationId)
    {
        return await _adminRepositories.BlockDonation(donationId);
    }
}