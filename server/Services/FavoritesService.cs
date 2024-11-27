namespace allspice.Services;

public class FavoritesService
{
  private readonly FavoritesRepository _repository;

  public FavoritesService(FavoritesRepository repository)
  {
    _repository = repository;
  }

  internal FavoritedRecipe CreateFavoriteRecipe(Favorite favoriteData)
  {
    FavoritedRecipe favoritedRecipe = _repository.CreateFavoriteRecipe(favoriteData);
    return favoritedRecipe;
  }

  private Favorite GetFavoriteById(int favoriteId)
  {
    Favorite favorite = _repository.GetFavoriteById(favoriteId);
    if (favorite == null)
    {
      throw new Exception($"Invalid Favorite ID: {favoriteId}");
    }
    return favorite;
  }

  internal List<FavoritedRecipe> GetPersonalFavoriteRecipes(string userId)
  {
    List<FavoritedRecipe> acquiredFavoriteRecipes = _repository.GetPersonalFavoriteRecipes(userId);
    return acquiredFavoriteRecipes;
  }

  internal string DeleteFavorite(string userId, int favoriteId)
  {
    Favorite favorite = GetFavoriteById(favoriteId);
    if (favorite.AccountId != userId)
    {
      throw new Exception("You cannot delete someone else's favorite recipe!");
    }
    _repository.DeleteFavorite(favoriteId);
    return $"Unfavorited Recipe";
  }
}