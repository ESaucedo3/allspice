namespace allspice.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
  private readonly AccountService _accountService;
  private readonly Auth0Provider _auth0Provider;
  private readonly FavoritesService _favoritesService;

  public AccountController(AccountService accountService, Auth0Provider auth0Provider, FavoritesService favoritesService)
  {
    _accountService = accountService;
    _auth0Provider = auth0Provider;
    _favoritesService = favoritesService;
  }

  [HttpGet]
  public async Task<ActionResult<Account>> Get()
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      return Ok(_accountService.GetOrCreateAccount(userInfo));
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpGet("favorites")]
  public async Task<ActionResult<List<FavoritedRecipe>>> GetPersonalFavoriteRecipes()
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      List<FavoritedRecipe> acquiredFavoriteRecipes = _favoritesService.GetPersonalFavoriteRecipes(userInfo.Id);
      return Ok(acquiredFavoriteRecipes);
    }
    catch (Exception e)
    {

      return BadRequest(e.Message);
    }
  }
}
