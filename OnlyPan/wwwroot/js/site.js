// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// JavaScript para ocultar la notificación al recargar la página
if (performance.navigation.type === 1) {
    var notificationDiv = document.getElementById("notificationDiv");
    if (notificationDiv) {
        notificationDiv.style.display = "none";
    }
}

//Creating Recipes Functionality


var ingredients = [];
var quantities = [];
var units = [];
function AddIngredient() {
    var option = document.getElementById("IdIngredient");
    var idIngredient = option.value;
    var ingredientName = option.options[option.selectedIndex].text;
    var quantity = document.getElementById("Quantity").value;
    var unit = document.getElementById("Unit").value;
    var unitText = document.getElementById("Unit").options[document.getElementById("Unit").selectedIndex].text;

    // Check if the ingredient is already in the list
    var index = ingredients.indexOf(idIngredient);

    if (index !== -1) {
        // Ingredient already exists, update quantity and unit
        if (units[index] !== unit) {
            // Unit has changed, throw an error
            alert("No puedes cambiar la unidad del ingrediente");
            return;
        }

        // Update quantity
        quantities[index] = quantity;

        // Update the displayed list
        var ul = document.getElementById("ingredients-list");
        ul.childNodes[index].textContent = ingredientName + " " + quantity + " " + unitText;
    } else {
        // Ingredient is not in the list, add a new entry
        ingredients.push(idIngredient);
        quantities.push(quantity);
        units.push(unit);

        // Display the new entry in the list
        var ul = document.getElementById("ingredients-list");
        var li = document.createElement("li");
        li.appendChild(document.createTextNode(ingredientName + " " + quantity + " " + unitText));
        ul.appendChild(li);
    }
}

function RemoveIngredient() {
    var option = document.getElementById("IdIngredient");
    var idIngredient = option.value;

    // Check if the ingredient is in the list
    var index = ingredients.indexOf(idIngredient);

    if (index !== -1) {
        // Remove the ingredient from the arrays
        ingredients.splice(index, 1);
        quantities.splice(index, 1);
        units.splice(index, 1);

        // Remove the ingredient from the displayed list
        var ul = document.getElementById("ingredients-list");
        ul.removeChild(ul.childNodes[index]);
    } else {
        alert("Error: Ingrediente no encontrado en la Lista");
    }
}
