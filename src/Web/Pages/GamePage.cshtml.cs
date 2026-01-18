using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages;

public class GamePageModel(IGameRepository gameRepo) : PageModel
{
    readonly IGameRepository _gameRepo = gameRepo;
    public required string Username {get; set;}
    public required bool ViewingFromWhite { get; set; }
    public GameDTO? Game {get; set;}

    public async Task<IActionResult> OnGet(int gameId)
    {
        string? from = HttpContext.Request.Query["fromWhite"];
        ViewingFromWhite = from != "false";
        Game = await _gameRepo.GetGameById(gameId);
        return Page();
    }
}
