using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab2.Data;
using Lab2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Pages.Books
{
    [Authorize(Roles = "Admin")]

    public class EditModel : BookCategoriesPageModel
    {
        private readonly Lab2Context _context;

        public EditModel(Lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Book = await _context.Book
                .Include(b => b.Publisher)
                .Include(b => b.Author)
                .Include(b => b.BookCategories).ThenInclude(bc => bc.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Book == null) return NotFound();

            PopulateAssignedCategoryData(_context, Book);

            var authorList = await _context.Author
                .Select(a => new { a.ID, FullName = a.LastName + " " + a.FirstName })
                .ToListAsync();

            ViewData["AuthorID"] = new SelectList(authorList, "ID", "FullName", Book.AuthorID);
            ViewData["PublisherID"] = new SelectList(_context.Set<Publisher>(), "ID", "PublisherName", Book.PublisherID);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedCategories)
        {
            if (id == null) return NotFound();

            var bookToUpdate = await _context.Book
                .Include(b => b.Publisher)
                .Include(b => b.Author)
                .Include(b => b.BookCategories).ThenInclude(bc => bc.Category)
                .FirstOrDefaultAsync(b => b.ID == id);

            if (bookToUpdate == null) return NotFound();

            if (await TryUpdateModelAsync<Book>(
                    bookToUpdate, "Book",
                    b => b.Title,
                    b => b.Price,
                    b => b.PublishingDate,
                    b => b.PublisherID,
                    b => b.AuthorID))
            {
                UpdateBookCategories(_context, selectedCategories, bookToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            UpdateBookCategories(_context, selectedCategories, bookToUpdate);
            PopulateAssignedCategoryData(_context, bookToUpdate);

            var authorList = await _context.Author
                .Select(a => new { a.ID, FullName = a.LastName + " " + a.FirstName })
                .ToListAsync();

            ViewData["AuthorID"] = new SelectList(authorList, "ID", "FullName", bookToUpdate.AuthorID);
            ViewData["PublisherID"] = new SelectList(_context.Set<Publisher>(), "ID", "PublisherName", bookToUpdate.PublisherID);

            return Page();
        }
    }
}
