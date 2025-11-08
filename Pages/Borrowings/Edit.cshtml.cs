using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab2.Data;
using Lab2.Models;

namespace Lab2.Pages.Borrowings
{
    public class EditModel : PageModel
    {
        private readonly Lab2Context _context;

        public EditModel(Lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Borrowing Borrowing { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Borrowing = await _context.Borrowing
                .Include(b => b.Book)
                    .ThenInclude(b => b.Author)
                .Include(b => b.Member)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Borrowing == null)
            {
                return NotFound();
            }

            var books = await _context.Book
                .Include(b => b.Author)
                .Select(b => new
                {
                    b.ID,
                    DisplayName = b.Title + " - " + b.Author.LastName + " " + b.Author.FirstName
                })
                .ToListAsync();

            ViewData["BookID"] = new SelectList(books, "ID", "DisplayName", Borrowing.BookID);
            ViewData["MemberID"] = new SelectList(_context.Member, "ID", "FullName", Borrowing.MemberID);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var books = await _context.Book
                    .Include(b => b.Author)
                    .Select(b => new
                    {
                        b.ID,
                        DisplayName = b.Title + " - " + b.Author.LastName + " " + b.Author.FirstName
                    })
                    .ToListAsync();

                ViewData["BookID"] = new SelectList(books, "ID", "DisplayName", Borrowing.BookID);
                ViewData["MemberID"] = new SelectList(_context.Member, "ID", "FullName", Borrowing.MemberID);

                return Page();
            }

            _context.Attach(Borrowing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Borrowing.Any(e => e.ID == Borrowing.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
