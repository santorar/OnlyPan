using System.ComponentModel.DataAnnotations;
using MimeKit.Cryptography;

namespace OnlyPan.Models.ViewModels.RecipesViewModels;

public class RecipeCreateViewModel
{
    [Display(Name="Nombre de la receta"), DataType(DataType.Text), Required(ErrorMessage = "El nombre de la receta es requerido")]
    public string? Name { get; set; }
    [Display(Name="Descripción"), DataType(DataType.Text), Required(ErrorMessage = "La descripción de la receta es requerida")]
    public string? Description { get; set; }
    [Display(Name="Categoria")]
    public int IdCategory { get; set; }
    [Display(Name="Etiqueta")]
    public int IdTag { get; set; }
    [Required(ErrorMessage = "Los ingredientes de la receta son requeridos")]
    public string? IdsIngredients { get; set; }
    public string? IngredientsQuantity { get; set; }
    public string? IngredientsUnit { get; set; }
    
    [Display(Name="Ingrediente"), DataType(DataType.Text)]
    public int IdIngredient { get; set; }
    [Display(Name="Cantidad"), DataType(DataType.Text)]
    public int Quantity { get; set; }
    [Display(Name="Unidad de medida"), DataType(DataType.Text)]
    public string? Unit { get; set; }
    [Display(Name="Instrucciones"), DataType(DataType.Text), Required(ErrorMessage = "Las instrucciones de la receta son requeridas")]
    public string? Instructions { get; set; }
    [Display(Name="Foto"), DataType(DataType.Upload), Required(ErrorMessage = "La foto de la receta es requerida")]
    public List<IFormFile>? Photos { get; set; }
    
}