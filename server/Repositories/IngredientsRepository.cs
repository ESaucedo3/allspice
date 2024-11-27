namespace allspice.Repositories;

public class IngredientsRepository
{
  private readonly IDbConnection _db;
  public IngredientsRepository(IDbConnection db)
  {
    _db = db;
  }

  internal Ingredient CreateIngredient(Ingredient ingredientData)
  {
    string sql = @"
    INSERT INTO ingredients(name, quantity, recipeId)
    VALUES(@Name, @Quantity, @RecipeId);

    SELECT * FROM ingredients
    WHERE ingredients.id = LAST_INSERT_ID();";

    Ingredient createdIngredient = _db.Query<Ingredient>(sql, ingredientData).FirstOrDefault();
    return createdIngredient;
  }

  internal List<Ingredient> GetIngredientsForRecipe(int recipeId)
  {
    string sql = "SELECT * FROM ingredients WHERE ingredients.recipeId = @RecipeId;";
    List<Ingredient> acquiredIngredients = _db.Query<Ingredient>(sql, new { recipeId }).ToList();
    return acquiredIngredients;
  }

  internal Ingredient GetSpecificIngredient(int ingredientId)
  {
    string sql = "SELECT * FROM ingredients WHERE id = @IngredientId;";
    Ingredient acquiredIngredient = _db.Query<Ingredient>(sql, new { ingredientId }).FirstOrDefault();
    return acquiredIngredient;
  }

  internal void DeleteIngredient(int ingredientId)
  {
    string sql = "DELETE FROM ingredients WHERE id = @IngredientId LIMIT 1";
    int rowsAffected = _db.Execute(sql, new { ingredientId });
    if (rowsAffected == 0)
    {
      throw new Exception("Nothing was deleted, something went wrong");
    }
    else if (rowsAffected > 1)
    {
      throw new Exception("More than one ingredient has been deleted that's probably not good!");
    }
  }
}