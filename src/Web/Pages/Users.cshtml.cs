using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages;

public class UsersModel(IUserRepository userRepo) : PageModel
{
    readonly IUserRepository _userRepo = userRepo;
    public required List<UserDTO> Users {get; set;}

    public async Task<IActionResult> OnGet()
    {
        string? pageNum = HttpContext.Request.Query["pageNum"];
        if(pageNum != null)
        {
            int offset = int.Parse(pageNum) - 1;
            Users = await _userRepo.GetUsers(32, 32 * offset, null);
        } else
        {
            Users = await _userRepo.GetUsers(32, 0, null);
        }
        return Page();
    }
}