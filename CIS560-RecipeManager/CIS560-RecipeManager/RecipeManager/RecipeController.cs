﻿

using System.Collections.Generic;

namespace CIS560_RecipeManager.RecipeManager
{
    public class RecipeController
    {
        private IQuery _queryRepository;
        private RecipeInventory _recipeInventory;
        private Pantry _pantry;
        public delegate void AddRecipeDelegate(string name, string description, ICollection<Ingredient> ingredients);

        public RecipeController(
            IQuery query, 
            RecipeInventory recipeInventory,
            Pantry pantry)
        {
            _queryRepository = query;
            _recipeInventory = recipeInventory;
            _pantry = pantry;
        }

        public void LaunchRecipeForm()
        {
            new uiRecipe(LaunchAddRecipeForm).Show();
        }

        public void LaunchAddRecipeForm()
        {
            new uiAddRecipeForm(AddRecipe).Show();
        }

        public void CookRecipe(Recipe recipe)
        {
            foreach (KeyValuePair<Ingredient, int> item in recipe.MeasuredIngredients)
            {
                //If we don't allow the user to delete Ingredients, there should
                //never be an Ingredient that doesn't exist in the Pantry, so we should
                //just let the IDictionary through a KeyNotFoundException
                int updatedQuantity = (_pantry.PantryContents[item.Key] -= item.Value);

                //update the Ingredient quantity in the database
                _queryRepository.UpdateIngredientQuantity(updatedQuantity,item.Key);
            }
        }

        public void AddRecipe(string recipeName, string recipeDescription, ICollection<Ingredient> ingredients)
        {
            Recipe recipe = _queryRepository.CreateRecipe(recipeName, recipeDescription, ingredients);
            _recipeInventory.AddRecipe(recipe);
        }
    }
}