using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages;

public class UserPageModel(IUserRepository userRepo) : PageModel
{
    readonly IUserRepository _userRepo = userRepo;
    public required string Username {get; set;}
    public UserDTO? UserInfo {get; set;}

    public async Task<IActionResult> OnGet(string user)
    {
        Username = user;
        UserInfo = await _userRepo.GetUserByName(user);
        return Page();
    }
}
