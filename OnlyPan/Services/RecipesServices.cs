using OnlyPan.Models;
using OnlyPan.Models.Dtos;
using OnlyPan.Models.Dtos.RecipesDtos;
using OnlyPan.Models.ViewModels.RecipesViewModels;
using OnlyPan.Repositories;
using OnlyPan.Utilities.Classes;

namespace OnlyPan.Services;

public class RecipesServices
{
    private readonly RecipesRepository _recipesRepository;

    public RecipesServices(OnlyPanDbContext dbContext)
    {
        _recipesRepository = new RecipesRepository(dbContext);
    }

    public async Task<List<CategoryDto>> GetCategories()
    {
        return await _recipesRepository.RequestCategories();
    }

    public async Task<List<IngredientDto>> GetIngredients()
    {
        return await _recipesRepository.RequestIngredients();
    }

    public async Task<List<TagDto>> GetTags()
    {
        return await _recipesRepository.RequestTags();
    }

    public async Task<List<UnitDto>> GetUnits()
    {
        return await _recipesRepository.RequestUnits();
    }

    public async Task<bool> CreateRecipe(RecipeCreateViewModel model, int idUser)
    {
        try
        {
            var pu = new PhotoUtilities();
            if (model.IdsIngredients == null || model.IngredientsQuantity == null || model.Unit == null)
                throw new SystemException();
            List<string> ids = model.IdsIngredients!.Split(',').ToList();
            List<int> idsInt = new List<int>();
            if (idsInt == null) throw new ArgumentNullException(nameof(idsInt));
            foreach (var id in ids)
            {
                idsInt.Add(int.Parse(id));
            }

            List<string> quantities = model.IngredientsQuantity!.Split(',').ToList();
            List<int> quantitiesInt = new List<int>();
            if (quantitiesInt == null) throw new ArgumentNullException(nameof(quantitiesInt));
            foreach (var quantity in quantities)
            {
                quantitiesInt.Add(int.Parse(quantity));
            }
            List<byte[]> photos = new List<byte[]>();
            foreach(var photo in model.Photos!)
            {
                photos.Add(await pu.convertToBytes(photo));
            }
            List<string> unitsStringList = model.IngredientsUnit!.Split(',').ToList();
            List<int> units = new List<int>();
            foreach (var unitString in unitsStringList)
            {
                units.Add(int.Parse(unitString));
            }
            RecipeDto recipe = new RecipeDto()
            {
                ChefId = idUser,
                Name = model.Name,
                Description = model.Description,
                IdCategory = model.IdCategory,
                IdTag = model.IdTag,
                IdsIngredients = idsInt,
                IngredientsQuantity = quantitiesInt,
                IngredientsUnit = units,
                Instructions = model.Instructions,
                Photos = photos!,
                Date = DateTime.Now
            };
            return await _recipesRepository.CreateRecipe(recipe);
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<List<RecipeFeedViewModel>> GetRecipes()
    {
        try
        {
            List<RecipeFeedDto> recipesDtos = await _recipesRepository.RequestRecipesFeed();
            List<RecipeFeedViewModel> recipeFeedViewModels = new List<RecipeFeedViewModel>();
            foreach (var recipeDto in recipesDtos)
            {
                var recipeViewModel = new RecipeFeedViewModel()
                {
                    IdRecipe = recipeDto.IdRecipe,
                    Name = recipeDto.Name,
                    Description = recipeDto.Description,
                    Rating = recipeDto.Rating,
                    Photo = recipeDto.Photo
                };
                recipeFeedViewModels.Add(recipeViewModel);
            }

            return recipeFeedViewModels;
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<RecipeViewModel> GetRecipe(int idRecipe, int userId)
    {
        try
        {
            RecipeDto recipeDto = await _recipesRepository.RequestRecipe(idRecipe, userId);
            RecipeViewModel recipeViewModel = new RecipeViewModel()
            {
                IdRecipe = recipeDto.IdRecipe,
                Name = recipeDto.Name,
                Description = recipeDto.Description,
                Photos = recipeDto.Photos,
                Instructions = recipeDto.Instructions,
                Rating = recipeDto.Rating,
                PersonalRating = recipeDto.PersonalRating,
                Ingredients = recipeDto.Ingredients,
                Category = recipeDto.Category,
                Tag = recipeDto.Tag,
                Date = recipeDto.Date,
                ChefId = recipeDto.ChefId,
                Chef = recipeDto.Chef,
                Comments = recipeDto.CommentsDto
            };
            return recipeViewModel;
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public List<int> GetRatingList()
    {
        List<int> rating = new List<int>();
        for (int i = 1; i <= 5; i++)
        {
            rating.Add(i);
        }

        return rating;
    }

    public async Task<bool> RateRecipe(int recipeId, int rating, int userId)
    {
        return await _recipesRepository.SetPersonalRating(recipeId, userId, rating);
    }

    public async Task<bool> CreateComment(string comment, int recipeId, int idUser)
    {
        CommentDto commentDto = new CommentDto()
        {
            IdUser = idUser,
            IdRecipe = recipeId,
            Comment = comment
        };
        var result = await _recipesRepository.CreateComment(commentDto);
        return result;
    }

    public async Task<bool> ReportComment(int commentId)
    {
        return await _recipesRepository.ReportComment(commentId);
    }

    public async Task<bool> SearchReportedComment(int commentId)
    {
        return await _recipesRepository.SearchReportedComment(commentId);
    }

    public async Task<bool> MakeDonation(int recipeId, float amount, int userId)
    {
        return await _recipesRepository.MakeDonation(amount, userId, recipeId);
    }
}