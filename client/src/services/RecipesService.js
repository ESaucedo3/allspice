import {Recipe} from '@/models/Recipe.js';
import {api} from './AxiosService.js';
import {AppState} from '@/AppState.js';

class RecipesService {
  async createRecipe(recipeData) {
    const response = await api.post('api/recipes', recipeData);
    const newRecipe = new Recipe(response.data);
    AppState.recipes.push(newRecipe);
  }
  async getAllRecipes() {
    const response = await api.get('api/recipes');
    const acquiredRecipes = response.data.map((recipe) => new Recipe(recipe));
    AppState.recipes = acquiredRecipes;
  }
  async updateRecipe(recipeId, recipeData) {
    const response = await api.put(`api/recipes/${recipeId}`, recipeData);
    const updatedRecipeIndex = AppState.recipes.findIndex((recipe) => recipe.id === recipeId);
    const updatedRecipe = new Recipe(response.data);
    AppState.recipes.splice(updatedRecipeIndex, 1, updatedRecipe);
  }
  async deleteRecipe(recipeId) {
    await api.delete(`api/recipes/${recipeId}`);
    const deletedRecipeIndex = AppState.recipes.findIndex((recipe) => recipe.id === recipeId);
    AppState.recipes.splice(deletedRecipeIndex, 1);
  }
}

export const recipesService = new RecipesService();
