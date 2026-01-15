using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages;

public class UserPageModel : PageModel
{

    readonly IUserRepository _userRepo;
    public required bool ViewingFromWhite { get; set; }
    public required string Username {get; set;}
    public UserDTO? UserInfo {get; set;}

    public UserPageModel(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<IActionResult> OnGet(string user)
    {
        string? from = HttpContext.Request.Query["fromWhite"];
        ViewingFromWhite = from != "false";
        Username = user;
        UserInfo = await _userRepo.GetUserByName(user);
        return Page();
    }
}
