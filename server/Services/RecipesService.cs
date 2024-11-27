namespace allspice.Services;

public class RecipesService
{
  private readonly RecipesRepository _repository;

  public RecipesService(RecipesRepository repository)
  {
    _repository = repository;
  }

  internal Recipe CreateRecipe(Recipe recipeData)
  {
    Recipe recipe = _repository.CreateRecipe(recipeData);
    return recipe;
  }

  internal List<Recipe> GetAllRecipes()
  {
    List<Recipe> recipes = _repository.GetAllRecipes();
    return recipes;
  }

  internal Recipe GetRecipeById(int recipeId)
  {
    Recipe recipe = _repository.GetRecipeById(recipeId);
    if (recipe == null)
    {
      throw new Exception($"ID: {recipeId} does not exist");
    }

    return recipe;
  }

  internal Recipe UpdateRecipe(int recipeId, Recipe updatedRecipeData, string userId)
  {
    Recipe recipe = GetRecipeById(recipeId);

    if (recipe.CreatorId != userId)
    {
      throw new Exception("You cannot update something that isn't yours");
    }

    recipe.Title = updatedRecipeData.Title ?? recipe.Title;
    recipe.Instructions = updatedRecipeData.Instructions ?? recipe.Instructions;
    recipe.Img = updatedRecipeData.Img ?? recipe.Img;
    recipe.Category = updatedRecipeData.Category ?? recipe.Category;

    Recipe updatedRecipe = _repository.UpdateRecipe(recipe);
    return updatedRecipe;
  }

  internal string DeleteRecipe(int recipeId, string userId)
  {
    Recipe recipe = GetRecipeById(recipeId);
    if (recipe.CreatorId != userId)
    {
      throw new Exception("You cannot delete something that doesn't belong to you!");
    }

    _repository.DeleteRecipe(recipeId);
    return $"{recipe.Title} was successfully deleted";
  }
}