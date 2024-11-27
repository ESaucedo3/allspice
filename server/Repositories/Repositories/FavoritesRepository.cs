



namespace checkpoint7.Repositories;

public class FavoritesRepository
{
  private readonly IDbConnection _db;

  public FavoritesRepository(IDbConnection db)
  {
    _db = db;
  }

  internal FavoritedRecipe CreateFavoriteRecipe(Favorite favoriteData)
  {
    string sql = @"
    INSERT INTO favorites(recipeId, accountId)
    VALUES(@RecipeId, @AccountId);

    SELECT
    favorites.*,
    recipes.*,
    accounts.*
    FROM favorites 
    JOIN recipes ON favorites.recipeId = recipes.id
    JOIN accounts ON favorites.accountId = accounts.id
    WHERE favorites.id = LAST_INSERT_ID();";

    FavoritedRecipe favoritedRecipe = _db.Query<Favorite, FavoritedRecipe, Profile, FavoritedRecipe>(sql, (favorite, recipeFavorite, profile) =>
    {
      recipeFavorite.FavoriteId = favorite.Id;
      recipeFavorite.AccountId = favorite.AccountId;
      recipeFavorite.Creator = profile;
      return recipeFavorite;
    }, favoriteData).FirstOrDefault();
    return favoritedRecipe;
  }

  internal Favorite GetFavoriteById(int favoriteId)
  {
    string sql = "SELECT * FROM favorites WHERE id = @favoriteId;";
    Favorite favorite = _db.Query<Favorite>(sql, new { favoriteId }).FirstOrDefault();
    return favorite;
  }

  internal List<FavoritedRecipe> GetPersonalFavoriteRecipes(string userId)
  {
    string sql = @"
    SELECT
    favorites.*,
    recipes.*,
    accounts.*
    FROM favorites
    INNER JOIN recipes ON favorites.recipeId = recipes.id
    INNER JOIN accounts ON favorites.accountId = accounts.id
    WHERE favorites.accountId = @userId
    ;";

    List<FavoritedRecipe> accountFavoriteRecipes = _db.Query<Favorite, FavoritedRecipe, Profile, FavoritedRecipe>(sql, (favorite, recipeFavorite, profile) =>
    {
      recipeFavorite.FavoriteId = favorite.Id;
      recipeFavorite.AccountId = favorite.AccountId;
      recipeFavorite.Creator = profile;
      return recipeFavorite;
    }, new { userId }).ToList();
    return accountFavoriteRecipes;
  }

  internal void DeleteFavorite(int favoriteId)
  {
    string sql = "DELETE FROM favorites WHERE id = @favoriteId LIMIT 1;";
    int rowsAffected = _db.Execute(sql, new { favoriteId });

    if (rowsAffected == 0)
    {
      throw new Exception("Nothing was unfavorited something went wrong");
    }
    else if (rowsAffected > 1)
    {
      throw new Exception("More than 1 recipe was unfavorited not good at all!");
    }
  }
}