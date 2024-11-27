namespace allspice.Repositories;

public class RecipesRepository
{
  private readonly IDbConnection _db;

  public RecipesRepository(IDbConnection db)
  {
    _db = db;
  }

  internal Recipe CreateRecipe(Recipe recipeData)
  {
    string sql = @"
    INSERT INTO recipes(title, instructions, img, category, creatorId)
    VALUES(@Title, @Instructions, @Img, @Category, @CreatorId);

    SELECT
    recipes.*,
    accounts.*
    FROM recipes
    INNER JOIN accounts ON recipes.creatorId = accounts.id
    WHERE recipes.id = LAST_INSERT_ID();";

    Recipe createdRecipe = _db.Query<Recipe, Profile, Recipe>(sql, (recipe, profile) =>
    {
      recipe.Creator = profile;
      return recipe;
    }, recipeData).FirstOrDefault();
    return createdRecipe;
  }


  internal List<Recipe> GetAllRecipes()
  {
    string sql = @"
    SELECT
    recipes.*,
    accounts.*
    FROM recipes
    INNER JOIN accounts ON recipes.creatorId = accounts.id
    LIMIT 10;";

    List<Recipe> recipes = _db.Query<Recipe, Profile, Recipe>(sql, (recipe, profile) =>
    {
      recipe.Creator = profile;
      return recipe;
    }).ToList();
    return recipes;
  }

  internal Recipe GetRecipeById(int recipeId)
  {
    string sql = @"
    SELECT
    recipes.*,
    accounts.*
    FROM recipes
    INNER JOIN accounts ON recipes.creatorId = accounts.id
    WHERE recipes.id = @recipeId;";

    Recipe acquiredRecipe = _db.Query<Recipe, Profile, Recipe>(sql, (recipe, profile) =>
    {
      recipe.Creator = profile;
      return recipe;
    }, new { recipeId }).FirstOrDefault();
    return acquiredRecipe;
  }

  internal Recipe UpdateRecipe(Recipe recipe)
  {
    string sql = @"
    UPDATE recipes
    SET
    title = @Title,
    instructions = @Instructions,
    img = @Img,
    category = @Category
    WHERE id = @Id
    LIMIT 1;

    SELECT recipes.*, accounts.*
    FROM recipes
    INNER JOIN accounts ON recipes.creatorId = accounts.id
    WHERE recipes.id = @Id;";


    Recipe updatedRecipe = _db.Query<Recipe, Profile, Recipe>(sql, (recipe, profile) =>
    {
      recipe.Creator = profile;
      return recipe;
    }, recipe).FirstOrDefault();

    return updatedRecipe;
  }

  internal void DeleteRecipe(int recipeId)
  {
    string sql = "DELETE FROM recipes WHERE id = @recipeId LIMIT 1;";

    int rowsAffected = _db.Execute(sql, new { recipeId });

    if (rowsAffected == 0)
    {
      throw new Exception("Something went wrong, nothing was deleted");
    }
    else if (rowsAffected > 1)
    {
      throw new Exception("More than 1 recipe was deleted not very good at all!");
    }
  }
}