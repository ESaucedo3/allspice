namespace allspice.Controllers;

[ApiController, Route("api/[controller]")]
public class RecipesController : ControllerBase
{
  private readonly Auth0Provider _auth0Provider;
  private readonly RecipesService _recipesService;
  private readonly IngredientsService _ingredientsService;

  public RecipesController(Auth0Provider auth0Provider, RecipesService recipesService, IngredientsService ingredientsService)
  {
    _auth0Provider = auth0Provider;
    _recipesService = recipesService;
    _ingredientsService = ingredientsService;
  }


  [Authorize, HttpPost]
  public async Task<ActionResult<Recipe>> CreateRecipe([FromBody] Recipe recipeData)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      recipeData.CreatorId = userInfo.Id;
      Recipe createdRecipe = _recipesService.CreateRecipe(recipeData);
      return Ok(createdRecipe);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpGet]
  public ActionResult<List<Recipe>> GetAllRecipes()
  {
    try
    {
      List<Recipe> acquiredRecipes = _recipesService.GetAllRecipes();
      return Ok(acquiredRecipes);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpGet("{recipeId}")]
  public ActionResult<Recipe> GetRecipeById(int recipeId)
  {
    try
    {
      Recipe acquiredRecipe = _recipesService.GetRecipeById(recipeId);
      return Ok(acquiredRecipe);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpGet("{recipeId}/ingredients")]
  public ActionResult<List<Ingredient>> GetIngredientsForRecipe(int recipeId)
  {
    try
    {
      List<Ingredient> acquiredIngredientsForRecipe = _ingredientsService.GetIngredientsForRecipe(recipeId);
      return Ok(acquiredIngredientsForRecipe);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [Authorize, HttpPut("{recipeId}")]
  public async Task<ActionResult<Recipe>> UpdateRecipe(int recipeId, [FromBody] Recipe updatedRecipeData)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      Recipe updatedRecipe = _recipesService.UpdateRecipe(recipeId, updatedRecipeData, userInfo.Id);
      return Ok(updatedRecipe);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }

  }

  [Authorize, HttpDelete("{recipeId}")]
  public async Task<ActionResult<string>> DeleteRecipe(int recipeId)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      string deletedMsg = _recipesService.DeleteRecipe(recipeId, userInfo.Id);
      return Ok(deletedMsg);
    }
    catch (Exception e)
    {

      return BadRequest(e.Message);
    }
  }
}