using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages;

public class UsersModel(IUserRepository userRepo) : PageModel
{
    readonly IUserRepository _userRepo = userRepo;
    public required List<UserDTO> Users {get; set;}

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    public async Task<IActionResult> OnGet()
    {
        string? pageNum = HttpContext.Request.Query["pageNum"];
        string? search = HttpContext.Request.Query["search"];
        if(pageNum != null)
        {
            int offset = int.Parse(pageNum) - 1;
            Users = await _userRepo.GetUsers(32, 32 * offset, search);
        } else
        {
            Users = await _userRepo.GetUsers(32, 0, search);
        }
        return Page();
    }

    public ActionResult OnPostSearch()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        return RedirectToPage(null, new { search = Search });
    }
}