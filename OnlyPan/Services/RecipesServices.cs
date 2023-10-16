using OnlyPan.Models;
using OnlyPan.Models.Dtos;
using OnlyPan.Models.ViewModels.RecipesViewModels;
using OnlyPan.Repositories;
using OnlyPan.Utilities.Classes;

namespace OnlyPan.Services;

public class RecipesServices
{
    private readonly RecipesRepository _recipesRepository;

    public RecipesServices(OnlyPanContext context)
    {
        _recipesRepository = new RecipesRepository(context);
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

    public List<UnitDto> GetUnits()
    {
        return _recipesRepository.RequestUnits();
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

            List<string> units = model.IngredientsUnit!.Split(',').ToList();
            RecipeDto recipe = new RecipeDto()
            {
                IdUser = idUser,
                Name = model.Name,
                Description = model.Description,
                IdCategory = model.IdCategory,
                IdTag = model.IdTag,
                IdsIngredients = idsInt,
                IngredientsQuantity = quantitiesInt,
                IngredientsUnit = units,
                Instructions = model.Instructions,
                Photo = await pu.convertToBytes(model.Photo),
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

    public async Task<RecipeViewModel> GetRecipe(int idRecipe)
    {
        try
        {
            RecipeDto recipeDto = await _recipesRepository.RequestRecipe(idRecipe);
            RecipeViewModel recipeViewModel = new RecipeViewModel()
            {
                IdRecipe = recipeDto.IdRecipe,
                Name = recipeDto.Name,
                Description = recipeDto.Description,
                Photo = recipeDto.Photo,
                Instructions = recipeDto.Instructions,
                Ingredients = recipeDto.Ingredients,
                Category = recipeDto.Category,
                Tag = recipeDto.Tag,
                Date = recipeDto.Date,
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
}