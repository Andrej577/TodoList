using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace TodoList.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    [BindProperty]
    public string? NewText { get; set; }

    [BindProperty]
    public int? EditId { get; set; }

    [BindProperty]
    public string? EditedText { get; set; }

    public List<TodoItem> Items { get; set; } = new();

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        LoadFromSession();
    }

    public IActionResult OnPost()
    {
        LoadFromSession();

        if (!string.IsNullOrWhiteSpace(NewText))
        {
            var newItem = new TodoItem
            {
                Id = Items.Count + 1,
                Text = NewText
            };

            Items.Add(newItem);
            SaveToSession();
            NewText = string.Empty;
        }
        return RedirectToPage();
    }

    public IActionResult OnPostDelete(int id)
    {
        LoadFromSession(); // Why?

        var item = Items.FirstOrDefault(x => x.Id == id);
        if (item != null)
        {
            Items.Remove(item);
            SaveToSession();
        }

        return RedirectToPage();
    }


    public IActionResult OnPostEdit(int id)
    {
        LoadFromSession();
        var item = Items.FirstOrDefault(x => x.Id == id);
        if (item != null)
        {
            EditId = id;
            EditedText = item.Text;
        }
        return Page();
    }

    public IActionResult OnPostUpdate(int id)
    {
        LoadFromSession();
        var item = Items.FirstOrDefault(x => x.Id == id);
        if (item != null && !string.IsNullOrWhiteSpace(EditedText))
        {
            item.Text = EditedText;
            SaveToSession();
        }
        EditId = null;
        EditedText = null;
        return RedirectToPage();
    }

    private void LoadFromSession()
    {
        var cookie = HttpContext.Request.Cookies["TodoList"];
        if (!string.IsNullOrEmpty(cookie))
        {
            try
            {
                Items = System.Text.Json.JsonSerializer.Deserialize<List<TodoItem>>(cookie) ?? new List<TodoItem>();
            }
            catch
            {
                Items = new List<TodoItem>();
            }
        }
        else
        {
            Items = new List<TodoItem>();
        }
    }

    private void SaveToSession()
    {
        var json = System.Text.Json.JsonSerializer.Serialize(Items);
        HttpContext.Response.Cookies.Append("TodoList", json, new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(30) });
    }
}
