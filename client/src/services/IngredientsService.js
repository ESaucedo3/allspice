import {Ingredient} from '@/models/Ingredient.js';
import {api} from './AxiosService.js';
import {AppState} from '@/AppState.js';

class IngredientsService {
  async deleteIngredient(ingredientId) {
    await api.delete(`api/ingredients/${ingredientId}`);
    const deleteIngredientIndex = AppState.activeRecipeIngredients.findIndex((ingredient) => ingredient.id === ingredientId);
    AppState.activeRecipeIngredients.splice(deleteIngredientIndex, 1);
  }
  async createIngredient(ingredientData) {
    const response = await api.post('api/ingredients', ingredientData);
    const newIngredient = new Ingredient(response.data);
    AppState.activeRecipeIngredients.push(newIngredient);
  }
  async getIngredientsForRecipe(recipe) {
    const response = await api.get(`api/recipes/${recipe.id}/ingredients`);
    const recipeIngredients = response.data.map((ingredient) => new Ingredient(ingredient));
    AppState.activeRecipeIngredients = recipeIngredients;
  }
}

export const ingredientsService = new IngredientsService();
