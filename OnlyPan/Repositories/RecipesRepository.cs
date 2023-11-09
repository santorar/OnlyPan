using Microsoft.EntityFrameworkCore;
using OnlyPan.Models;
using OnlyPan.Models.Dtos;
using OnlyPan.Models.Dtos.RecipesDtos;
using OnlyPan.Models.Dtos.UserDtos;
using OnlyPan.Utilities.Classes;

namespace OnlyPan.Repositories;

public class RecipesRepository
{
    private readonly OnlyPanDbContext _dbContext;

    public RecipesRepository(OnlyPanDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<CategoryDto>> RequestCategories()
    {
        try
        {
            var categories = await _dbContext.Categoria.ToListAsync();
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
            var ingredients = await _dbContext.Ingredientes.ToListAsync();
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
            var tags = await _dbContext.Etiqueta.ToListAsync();
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

    public async Task<List<UnitDto>> RequestUnits()
    {
        try
        {
            var units = await _dbContext.Unidads.ToListAsync();
            var result = new List<UnitDto>();
            foreach (var unit in units)
            {
                result.Add(new UnitDto()
                {
                    IdUnit = unit.IdUnidad,
                    ShortName = unit.NombreCorto,
                    LongName = unit.NombreLargo
                });
            }

            return result;
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<double> RequestRating(int recipeId)
    {
        double finalRating = 0;
        var recipe = await _dbContext.Receta.FindAsync(recipeId);
        if (recipe!.NValoraciones == 0)
        {
            finalRating = 0;
        }
        else
        {
            finalRating = (double)(recipe.Valoracion / recipe.NValoraciones)!;
        }

        return finalRating;
    }

    public async Task<int> RequestPersonalRating(int recipeId, int userId)
    {
        var rating = await _dbContext.Valoracions.FindAsync(userId, recipeId);
        if (rating == null) return 0;
        return rating.Valoration!.Value;
    }

    //TODO: Check if the user has already rated the recipe
    public async Task<bool> SetPersonalRating(int recipeId, int userId, int rating)
    {
        try
        {
            var request = await _dbContext.Valoracions.FindAsync(userId, recipeId);
            var recipe = await _dbContext.Receta.FindAsync(recipeId);
            if (request == null)
            {
                var newRating = new Valoracion()
                {
                    IdReceta = recipeId,
                    IdUsuario = userId,
                    Valoration = rating,
                    FechaInteracion = DateTime.Now,
                    IdEstado = 5
                };
                recipe!.Valoracion += rating;
                recipe.NValoraciones++;
                await _dbContext.Valoracions.AddAsync(newRating);
            }
            else
            {
                if (rating > 5 || rating < 0) return false;
                recipe!.Valoracion -= request!.Valoration;
                request!.Valoration = rating;
                recipe.Valoracion += rating;
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (SystemException)
        {
            return false;
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
                FechaCreacion = recipe.Date,
                IdEstado = 4
            };
            await _dbContext.Receta.AddAsync(newRecipe);
            await _dbContext.SaveChangesAsync();
            var idRecipe = newRecipe.IdReceta;
            for (int i = 0; i < recipe.IdsIngredients!.Count; i++)
            {
                var newRecipeIngredient = new RecetaIngrediente()
                {
                    IdReceta = idRecipe,
                    IdIngrediente = recipe.IdsIngredients[i],
                    Cantidad = recipe.IngredientsQuantity![i],
                    IdUnidad = recipe.IngredientsUnit![i]
                };
                await _dbContext.RecetaIngredientes.AddAsync(newRecipeIngredient);
            }

            if (recipe.Photos!.Count != 0)
            {
                foreach (var image in recipe.Photos)
                {
                    var newImage = new ImagenRecetum()
                    {
                        IdReceta = idRecipe,
                        Imagen = image
                    };
                    await _dbContext.ImagenReceta.AddAsync(newImage);
                }
            }

            var newRecipeUser = new RecetaChef()
            {
                IdReceta = idRecipe,
                IdChef = recipe.ChefId,
                FechaActualizacion = DateTime.Now
            };
            await _dbContext.RecetaChefs.AddAsync(newRecipeUser);
            await _dbContext.SaveChangesAsync();
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
            var recipes = await _dbContext.Receta.Where(r => r.IdEstado == 5).ToListAsync();
            List<RecipeFeedDto> recipesDtos = new List<RecipeFeedDto>();
            foreach (var recipe in recipes)
            {
                var image = await _dbContext.ImagenReceta
                    .Where(i => i.IdReceta == recipe.IdReceta).FirstOrDefaultAsync();
                byte[]? imageBytes;
                if (image == null)
                {
                    imageBytes =
                        new PhotoUtilities().GetPhotoFromFile(Directory.GetCurrentDirectory() +
                                                              "/wwwroot/icons/recipeDefault.jpg");
                }
                else
                {
                    imageBytes = image.Imagen;
                }

                var recipeDto = new RecipeFeedDto()
                {
                    IdRecipe = recipe.IdReceta,
                    Name = recipe.NombreReceta,
                    Rating = await RequestRating(recipe.IdReceta),
                    Description = recipe.DescripcionReceta,
                    Photo = imageBytes
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

    public async Task<RecipeDto> RequestRecipe(int recipeId, int userId)
    {
        try
        {
            Recetum? recipe = await _dbContext.Receta.FindAsync(recipeId);
            if (recipe!.IdEstado != 5) throw new SystemException();
            List<ImagenRecetum>? photos =
                await _dbContext.ImagenReceta.Where(p => p.IdReceta == recipeId).ToListAsync();
            RecetaChef? recipeChef =
                await _dbContext.RecetaChefs.Where(r => r.IdReceta == recipe!.IdReceta).FirstOrDefaultAsync();
            var chef = await _dbContext.Usuarios.FindAsync(recipeChef!.IdChef);
            var listComments = await _dbContext.Comentarios
                .Where(c => c.IdReceta == recipe!.IdReceta && c.Estado != 6).ToListAsync();
            List<CommentDto> comments = new List<CommentDto>();
            foreach (var comentary in listComments)
            {
                var user = await _dbContext.Usuarios.FindAsync(comentary.IdUsuario);
                CommentDto commentDto = new CommentDto()
                {
                    IdComment = comentary.IdComentario,
                    IdUser = user!.IdUsuario,
                    UserName = user.Nombre,
                    Comment = comentary.Comentario1
                };
                comments.Add(commentDto);
            }

            string? category = ((await _dbContext.Categoria.FindAsync(recipe!.IdCategoria))!).NombreCategoria;
            string? tag = ((await _dbContext.Etiqueta.FindAsync(recipe.IdEtiqueta))!).NombreEtiqueta;
            var listIngredientsRecipe =
                await _dbContext.RecetaIngredientes
                    .Where(i => i.IdReceta == recipe.IdReceta)
                    .Include(i => i.IdUnidadNavigation).ToListAsync();
            List<string> ingredients = new List<string>();
            foreach (var ingredient in listIngredientsRecipe)
            {
                string? type = ((await _dbContext.Ingredientes.FindAsync(ingredient.IdIngrediente))!).NombreIngrediente;
                string stringIngredient =
                    type + " " + ingredient.Cantidad + " " + ingredient.IdUnidadNavigation!.NombreCorto;
                ingredients.Add(stringIngredient);
            }

            double rating = await RequestRating(recipe.IdReceta);
            int personalRating = await RequestPersonalRating(recipe.IdReceta, userId);
            RecipeDto recipeDto = new RecipeDto()
            {
                IdRecipe = recipe.IdReceta,
                Name = recipe.NombreReceta,
                Description = recipe.DescripcionReceta,
                Rating = rating,
                PersonalRating = personalRating,
                Category = category,
                Tag = tag,
                Ingredients = ingredients,
                Instructions = recipe.Instrucciones,
                Photos = photos!.Select(p => p.Imagen).ToList()!,
                Date = recipe.FechaCreacion,
                ChefId = recipeChef.IdChef,
                Chef = chef!.Nombre,
                CommentsDto = comments
            };
            return recipeDto;
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<RecipeDto> RequestRecipeAdmin(int? recipeId)
    {
        try
        {
            Recetum? recipe = await _dbContext.Receta.FindAsync(recipeId);
            List<ImagenRecetum>? photos =
                await _dbContext.ImagenReceta.Where(p => p.IdReceta == recipeId).ToListAsync();
            RecetaChef? recipeChef =
                await _dbContext.RecetaChefs.Where(r => r.IdReceta == recipe!.IdReceta).FirstOrDefaultAsync();
            var chef = await _dbContext.Usuarios.FindAsync(recipeChef!.IdChef);
            string? category = ((await _dbContext.Categoria.FindAsync(recipe!.IdCategoria))!).NombreCategoria;
            string? tag = ((await _dbContext.Etiqueta.FindAsync(recipe.IdEtiqueta))!).NombreEtiqueta;
            var listIngredientsRecipe =
                await _dbContext.RecetaIngredientes
                    .Where(i => i.IdReceta == recipe.IdReceta)
                    .Include(i => i.IdUnidadNavigation).ToListAsync();
            List<string> ingredients = new List<string>();
            foreach (var ingredient in listIngredientsRecipe)
            {
                string? type = ((await _dbContext.Ingredientes.FindAsync(ingredient.IdIngrediente))!).NombreIngrediente;
                string stringIngredient =
                    type + " " + ingredient.Cantidad + " " + ingredient.IdUnidadNavigation!.NombreCorto;
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
                Photos = photos!.Select(p => p.Imagen).ToList()!,
                Date = recipe.FechaCreacion,
                ChefId = recipeChef.IdChef,
                Chef = chef!.Nombre
            };
            return recipeDto;
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<bool> CreateComment(CommentDto comment)
    {
        try
        {
            var newComment = new Comentario()
            {
                IdUsuario = comment.IdUser,
                IdReceta = comment.IdRecipe,
                Comentario1 = comment.Comment,
                Estado = 5,
                Fecha = DateTime.Now,
            };
            await _dbContext.Comentarios.AddAsync(newComment);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<bool> ReportComment(int commentId)
    {
        try
        {
            var comment = await _dbContext.Comentarios.FindAsync(commentId);
            comment!.Estado = 7;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<bool> SearchReportedComment(int commentId)
    {
        try
        {
            var comment = await _dbContext.Comentarios.FindAsync(commentId);
            if (comment!.Estado == 7)
                return true;
            return false;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<bool> MakeDonation(float amount, int userId, int recipeId)
    {
        try
        {
            var user = await _dbContext.Usuarios.FindAsync(userId);
            var recipe = await _dbContext.Receta.FindAsync(recipeId);
            var recipeChef = await _dbContext.RecetaChefs.Where(r => r.IdReceta == recipeId).FirstOrDefaultAsync();
            if (user == null || recipe == null) return false;
            var newDonation = new Donacion()
            {
                IdUsuario = userId,
                Fecha = DateTime.Now,
                IdChef = recipeChef!.IdChef,
                Monto = (decimal?)amount,
                Estado = 4
            };
            await _dbContext.Donacions.AddAsync(newDonation);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<List<RecipeFeedDto>> SearchRecipes(string searchText)
    {
        try
        {
            var recipes = await _dbContext.Receta.Where(r => r.NombreReceta!.Contains(searchText) && r.IdEstado == 5)
                .ToListAsync();
            List<RecipeFeedDto> recipesDtos = new List<RecipeFeedDto>();
            foreach (var recipe in recipes)
            {
                var image = await _dbContext.ImagenReceta
                    .Where(i => i.IdReceta == recipe.IdReceta).FirstOrDefaultAsync();
                byte[]? imageBytes;
                if (image == null)
                {
                    imageBytes =
                        new PhotoUtilities().GetPhotoFromFile(Directory.GetCurrentDirectory() +
                                                              "/wwwroot/icons/recipeDefault.jpg");
                }
                else
                {
                    imageBytes = image.Imagen;
                }

                var recipeDto = new RecipeFeedDto()
                {
                    IdRecipe = recipe.IdReceta,
                    Name = recipe.NombreReceta,
                    Rating = await RequestRating(recipe.IdReceta),
                    Description = recipe.DescripcionReceta,
                    Photo = imageBytes
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

    public async Task<bool> ReplicateRecipe(int recipeId, int idUser)
    {
        try
        {
            ReplicaUsuario recetaUsuario = new ReplicaUsuario()
            {
                IdReceta = recipeId,
                IdUsuario = idUser,
                FechaReplica = DateTime.Now
            };
            await _dbContext.ReplicaUsuarios.AddAsync(recetaUsuario);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<ReplicDto> RequestReplic(int replicId)
    {
        try
        {
            var replic = await _dbContext.ReplicaUsuarios.FindAsync(replicId);
            var recipe = await RequestRecipeAdmin(replic!.IdReceta);
            return new ReplicDto()
            {
                IdReplic = replic.IdReplica,
                Name = recipe.Name,
                Description = recipe.Description,
                Category = recipe.Category,
                Tag = recipe.Tag,
                Ingredients = recipe.Ingredients,
                Instructions = recipe.Instructions,
                Photos = recipe.Photos,
                Date = recipe.Date,
                Chef = recipe.Chef,
                Rating = recipe.Rating,
                DateReplicated = replic.FechaReplica,
                Comentary = replic.Comentario
            };
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<bool> DeleteReplic(int replicId)
    {
        try
        {
            var replic = await _dbContext.ReplicaUsuarios.FindAsync(replicId);
            _dbContext.ReplicaUsuarios.Remove(replic!);
            await _dbContext.SaveChangesAsync();
            return true;

        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<bool> UpdateReplic(int replicId, string? comentary)
    {
        try
        {
            var replic = await _dbContext.ReplicaUsuarios.FindAsync(replicId);
            if (replic!.Comentario != null || comentary != replic!.Comentario)
                replic.Comentario = comentary;
            _dbContext.ReplicaUsuarios.Update(replic);
            await _dbContext.SaveChangesAsync();
            return true;

        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<DonationDto> RequestDonation(int recipeId, int userId)
    {
        try
        {
            var recipeChef = await _dbContext.RecetaChefs.Where(r => r.IdReceta == recipeId).FirstOrDefaultAsync();
            var donation = await _dbContext.Donacions.Where(r => r.IdChef == recipeChef.IdChef || r.IdUsuario == userId).FirstOrDefaultAsync();
            if (donation == null)
                throw new SystemException();
            return new DonationDto()
            {
                amount = (float)donation.Monto,
                recipeId = recipeId,
                donationId = donation.IdDonacion
            };
        }
        catch (SystemException)
        {
            return null!;
        }
    }
}