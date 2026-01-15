using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages;

public class PlaySelfModel : PageModel
{

    public required bool ViewingFromWhite { get; set; }

    public IActionResult OnGet()
    {
        string? from = HttpContext.Request.Query["fromWhite"];
        ViewingFromWhite = from != "false";
        return Page();
    }
}
