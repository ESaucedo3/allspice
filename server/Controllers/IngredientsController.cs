namespace allspice.Controllers;

[ApiController, Route("api/[controller]")]
public class IngredientsController : ControllerBase
{
  private readonly Auth0Provider _auth0Provider;
  private readonly IngredientsService _ingredientsService;

  public IngredientsController(Auth0Provider auth0Provider, IngredientsService ingredientsService)
  {
    _auth0Provider = auth0Provider;
    _ingredientsService = ingredientsService;
  }

  [Authorize, HttpPost]
  public async Task<ActionResult<Ingredient>> CreateIngredient([FromBody] Ingredient ingredientData)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      Ingredient createdIngredient = _ingredientsService.CreateIngredient(userInfo.Id, ingredientData);
      return Ok(createdIngredient);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [Authorize, HttpDelete("{ingredientId}")]
  public async Task<ActionResult<string>> DeleteIngredient(int ingredientId)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      string deletedMsg = _ingredientsService.DeleteIngredient(ingredientId, userInfo.Id);
      return Ok(deletedMsg);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }
}