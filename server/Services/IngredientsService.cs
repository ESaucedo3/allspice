namespace allspice.Services;

public class IngredientsService
{
  private readonly IngredientsRepository _repository;
  private readonly RecipesService _recipesService;
  public IngredientsService(IngredientsRepository repository, RecipesService recipesService)
  {
    _repository = repository;
    _recipesService = recipesService;
  }

  internal Ingredient CreateIngredient(string userId, Ingredient ingredientData)
  {
    Recipe recipe = _recipesService.GetRecipeById(ingredientData.RecipeId);
    if (recipe.CreatorId != userId)
    {
      throw new Exception("You cannot add an ingredient to a recipe that you did not create");
    }

    Ingredient newIngredient = _repository.CreateIngredient(ingredientData);
    return newIngredient;
  }

  internal List<Ingredient> GetIngredientsForRecipe(int recipeId)
  {
    List<Ingredient> acquiredIngredientsForRecipe = _repository.GetIngredientsForRecipe(recipeId);
    return acquiredIngredientsForRecipe;
  }

  internal string DeleteIngredient(int ingredientId, string userId)
  {
    Ingredient acquiredIngredient = _repository.GetSpecificIngredient(ingredientId);
    Recipe acquiredRecipe = _recipesService.GetRecipeById(acquiredIngredient.RecipeId);

    if (acquiredRecipe.CreatorId != userId)
    {
      throw new Exception("You cannot delete an ingredient from a recipe that you did not create!");
    }

    _repository.DeleteIngredient(ingredientId);
    return $"{acquiredIngredient.Name} was deleted from {acquiredRecipe.Title}";
  }

}