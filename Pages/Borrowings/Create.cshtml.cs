using System.Linq;
using System.Threading.Tasks;
using Lab2.Data;
using Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Pages.Borrowings
{
    public class CreateModel : PageModel
    {
        private readonly Lab2Context _context;

        public CreateModel(Lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Borrowing Borrowing { get; set; }

        public IActionResult OnGet()
        {
            var bookList = _context.Book
                .Include(b => b.Author)
                .Select(x => new
                {
                    x.ID,
                    BookFullName = x.Title + " - " + x.Author.LastName + " - " + x.Author.FirstName
                })
                .ToList();

            ViewData["BookID"] = new SelectList(bookList, "ID", "BookFullName");
            ViewData["MemberID"] = new SelectList(_context.Member, "ID", "Email");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var bookList = _context.Book
                    .Include(b => b.Author)
                    .Select(x => new
                    {
                        x.ID,
                        BookFullName = x.Title + " - " + x.Author.LastName + " - " + x.Author.FirstName
                    })
                    .ToList();

                ViewData["BookID"] = new SelectList(bookList, "ID", "BookFullName");
                ViewData["MemberID"] = new SelectList(_context.Member, "ID", "Email");

                return Page();
            }

            _context.Borrowing.Add(Borrowing);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
