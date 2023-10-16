using Microsoft.EntityFrameworkCore;
using OnlyPan.Models;
using OnlyPan.Models.Dtos;

namespace OnlyPan.Repositories;

public class RecipesRepository
{
    private readonly OnlyPanContext _context;

    public RecipesRepository(OnlyPanContext context)
    {
        _context = context;
    }

    public async Task<List<CategoryDto>> RequestCategories()
    {
        try
        {
            var categories = await _context.Categoria.ToListAsync();
            List<CategoryDto> result = new List<CategoryDto>();
            foreach (var category in categories)
            {
                CategoryDto categoryDto = new CategoryDto()
                {
                    IdCategory = category.IdCategoria,
                    NameCategory = category.NombreCategoria
                };
                result.Add(categoryDto);
            }

            return result;
        }
        catch (SystemException)
        {
            return null!;
        }
    }
    public async Task<List<IngredientDto>> RequestIngredients()
    {
        try
        {
            var ingredients = await _context.Ingredientes.ToListAsync();
            List<IngredientDto> result = new List<IngredientDto>();
            foreach (var ingredient in ingredients)
            {
                IngredientDto ingredientDto = new IngredientDto()
                {
                    IdIngredient = ingredient.IdIngrediente,
                    NameIngredient = ingredient.NombreIngrediente
                };
                result.Add(ingredientDto);
            }

            return result;
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<List<TagDto>> RequestTags()
    {
        try
        {
            var tags = await _context.Etiqueta.ToListAsync();
            List<TagDto> result = new List<TagDto>();
            foreach (var tag in tags)
            {
                TagDto tagDto = new TagDto()
                {
                    IdTag = tag.IdEtiqueta,
                    NameTag = tag.NombreEtiqueta
                };
                result.Add(tagDto);
            }

            return result;
        }
        catch (SystemException)
        {
            return null!;
        }
    }
    public List<UnitDto> RequestUnits()
    {
        try
        {
            List<UnitDto> result = new List<UnitDto>();
            var shortNames = new List<string>{ "g", "kg", "ml", "l", "oz", "lb" };
            var longNames = new List<string>{ "gramos", "kilogramos", "mililitros", "litros", "onzas", "libras" };
            for (int i = 0; i < shortNames.Count; i++)
            {
                UnitDto unitDto = new UnitDto()
                {
                    ShortName = shortNames[i],
                    LongName = longNames[i]
                };
                result.Add(unitDto);
            }
            
            return result;
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<bool> CreateRecipe(RecipeDto recipe)
    {
        try
        {
            var newRecipe = new Recetum()
            {
                NombreReceta = recipe.Name,
                DescripcionReceta = recipe.Description,
                IdCategoria = recipe.IdCategory,
                IdEtiqueta = recipe.IdTag,
                Instrucciones = recipe.Instructions,
                Foto = recipe.Photo,
                FechaCreacion = recipe.Date,
                IdEstado = 4
            };
            await _context.Receta.AddAsync(newRecipe);
            await _context.SaveChangesAsync();
            var idRecipe = newRecipe.IdReceta;
            for (int i = 0; i < recipe.IdsIngredients!.Count; i++)
            {
                var newRecipeIngredient = new RecetaIngrediente()
                {
                    IdReceta = idRecipe,
                    IdIngrediente = recipe.IdsIngredients[i],
                    Cantidad = recipe.IngredientsQuantity![i],
                    Unidad = recipe.IngredientsUnit![i]
                };
                await _context.RecetaIngredientes.AddAsync(newRecipeIngredient);
            }
            var newRecipeUser = new RecetaChef()
            {
                IdReceta = idRecipe,
                IdChef = recipe.IdUser,
                FechaActualizacion = DateTime.Now
            };
            await _context.RecetaChefs.AddAsync(newRecipeUser);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }
}