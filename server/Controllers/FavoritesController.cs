namespace allspice.Controllers;

[ApiController, Route("api/[controller]")]
public class FavoritesController : ControllerBase
{
  private readonly Auth0Provider _auth0Provider;
  private readonly FavoritesService _favoritesService;
  public FavoritesController(FavoritesService favoritesService, Auth0Provider auth0Provider)
  {
    _auth0Provider = auth0Provider;
    _favoritesService = favoritesService;
  }


  [Authorize, HttpPost]
  public async Task<ActionResult<FavoritedRecipe>> CreateFavoriteRecipe([FromBody] Favorite favoriteData)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      favoriteData.AccountId = userInfo.Id;
      FavoritedRecipe favoritedRecipe = _favoritesService.CreateFavoriteRecipe(favoriteData);
      return Ok(favoritedRecipe);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [Authorize, HttpDelete("{favoriteId}")]
  public async Task<ActionResult<string>> DeleteFavorite(int favoriteId)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      string deletedMsg = _favoritesService.DeleteFavorite(userInfo.Id, favoriteId);
      return Ok(deletedMsg);

    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }
}