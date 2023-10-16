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
            var shortNames = new List<string> { "g", "kg", "ml", "l", "oz", "lb" };
            var longNames = new List<string> { "gramos", "kilogramos", "mililitros", "litros", "onzas", "libras" };
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

    public async Task<List<RecipeFeedDto>> RequestRecipesFeed()
    {
        try
        {
            var recipes = await _context.Receta.ToListAsync();
            List<RecipeFeedDto> recipesDtos = new List<RecipeFeedDto>();
            foreach (var recipe in recipes)
            {
                var recipeDto = new RecipeFeedDto()
                {
                    IdRecipe = recipe.IdReceta,
                    Name = recipe.NombreReceta,
                    Description = recipe.DescripcionReceta,
                    Photo = recipe.Foto
                };
                recipesDtos.Add(recipeDto);
            }

            return recipesDtos;
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<RecipeDto> RequestRecipe(int idRecipe)
    {
        try
        {
            Recetum recipe = (await _context.Receta.FindAsync(idRecipe))!;
            RecetaChef? recipeChef =
                await _context.RecetaChefs.Where(r => r.IdReceta == recipe.IdReceta).FirstOrDefaultAsync();
            string chef = ((await _context.Usuarios.FindAsync(recipeChef!.IdChef))!).Nombre!;
            var listComments = await _context.Comentarios.Where(c => c.IdReceta == recipe.IdReceta).ToListAsync();
            List<CommentDto> comments = new List<CommentDto>();
            foreach (var comentary in listComments)
            {
                var user = await _context.Usuarios.FindAsync(comentary.IdUsuario);
                CommentDto commentDto = new CommentDto()
                {
                    IdUser = user!.IdUsuario,
                    UserName = user.Nombre,
                    Comment = comentary.Comentario1
                };
                comments.Add(commentDto);
            }
            string? category = ((await _context.Categoria.FindAsync(recipe.IdCategoria))!).NombreCategoria;
            string? tag = ((await _context.Etiqueta.FindAsync(recipe.IdEtiqueta))!).NombreEtiqueta;
            var listIngredientsRecipe = await _context.RecetaIngredientes.Where(i => i.IdReceta == recipe.IdReceta).ToListAsync();
            List<string> ingredients = new List<string>();
            foreach (var ingredient in listIngredientsRecipe)
            {
                string? type = ((await _context.Ingredientes.FindAsync(ingredient.IdIngrediente))!).NombreIngrediente;
                string stringIngredient = type + " " + ingredient.Cantidad + " " + ingredient.Unidad;
                ingredients.Add(stringIngredient);
            }

            RecipeDto recipeDto = new RecipeDto()
            {
                IdRecipe = recipe.IdReceta,
                Name = recipe.NombreReceta,
                Description = recipe.DescripcionReceta,
                Category = category,
                Tag = tag,
                Ingredients = ingredients,
                Instructions = recipe.Instrucciones,
                Photo = recipe.Foto,
                Date = recipe.FechaCreacion,
                Chef = chef,
                CommentsDto = comments
            };
            return recipeDto;
        }
        catch (SystemException)
        {
            return null!;
        }
    }
}