using OnlyPan.Models;
using OnlyPan.Models.Dtos;
using OnlyPan.Models.Dtos.AdminDtos;
using OnlyPan.Models.ViewModels.AdminViewModels;
using OnlyPan.Models.ViewModels.RecipesViewModels;
using OnlyPan.Repositories;

namespace OnlyPan.Services;

public class AdminServices
{
    private AdminRepositories _adminRepositories;
    private readonly RecipesRepository _recipesRepository;

    public AdminServices(OnlyPanDbContext dbContext)
    {
        _adminRepositories = new AdminRepositories(dbContext);
        _recipesRepository = new RecipesRepository(dbContext);
    }

    public async Task<List<StateDto>> GetStates(int indicator)
    {
        return await _adminRepositories.RequestStates(indicator);
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

    public async Task<List<RecipeModerateViewModel>> GetRecipes(int idState)
    {
        try
        {
            List<RecipeModerateDto> recipes = await _adminRepositories.RequestRecipes(idState);
            List<RecipeModerateViewModel> recipesViewModels = new List<RecipeModerateViewModel>();
            foreach (var recipe in recipes)
            {
                recipesViewModels.Add(new RecipeModerateViewModel()
                {
                    IdRecipe = recipe.IdRecipe,
                    Name = recipe.Name,
                    ChefName = recipe.ChefName,
                    State = recipe.State,
                    Date = recipe.Date
                });
            }
            return recipesViewModels;
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<RecipeViewModel> GetRecipe(int idRecipe)
    {
        try
        {
            RecipeDto recipeDto = await _recipesRepository.RequestRecipeAdmin(idRecipe);
            RecipeViewModel recipeViewModel = new RecipeViewModel()
            {
                IdRecipe = recipeDto.IdRecipe,
                Name = recipeDto.Name,
                Description = recipeDto.Description,
                Photos = recipeDto.Photos,
                Instructions = recipeDto.Instructions,
                Rating = recipeDto.Rating,
                Ingredients = recipeDto.Ingredients,
                Category = recipeDto.Category,
                Tag = recipeDto.Tag,
                Date = recipeDto.Date,
                ChefId = recipeDto.ChefId,
                Chef = recipeDto.Chef
            };
            return recipeViewModel;
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<bool> AcceptRecipe(int idRecipe)
    {
        return await _adminRepositories.AcceptRecipe(idRecipe);
    }

    public async Task<bool> BlockRecipe(int idRecipe)
    {
        return await _adminRepositories.BlockRecipe(idRecipe);
    }
}