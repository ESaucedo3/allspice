import {AppState} from '@/AppState.js';
import {api} from './AxiosService.js';
import {RecipeFavorite} from '@/models/Favorite.js';

class FavoritesService {
  async getPersonalFavoriteRecipes() {
    const response = await api.get('account/favorites');
    AppState.recipeFavorites = response.data.map((favorite) => new RecipeFavorite(favorite));
  }
  async favoriteRecipe(recipeId) {
    const recipeData = {recipeId: recipeId};
    const response = await api.post('api/favorites', recipeData);
    const newFavorited = new RecipeFavorite(response.data);
    AppState.recipeFavorites.push(newFavorited);
  }
  async deleteFavoriteRecipe(recipeId) {
    await api.delete(`api/favorites/${recipeId}`);
    const favoriteIndex = AppState.recipeFavorites.findIndex((fave) => fave.id === recipeId);
    AppState.recipeFavorites.splice(favoriteIndex, 1);
  }
}

export const favoritesService = new FavoritesService();
