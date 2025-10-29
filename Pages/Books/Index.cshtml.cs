using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab2.Data;
using Lab2.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Pages.Books
{
    public class IndexModel : PageModel
    {
        private readonly Lab2Context _context;

        public IndexModel(Lab2Context context)
        {
            _context = context;
        }

        public IList<Book> Books { get; set; } = new List<Book>();

        public BookData BookD { get; set; }
        public int BookID { get; set; }
        public int CategoryID { get; set; }
        public string TitleSort { get; set; }
        public string AuthorSort { get; set; }

        public string CurrentFilter { get; set; }
        public async Task OnGetAsync(int? id, int? categoryID, string sortOrder, string searchString)
        {
            BookD = new BookData();
            TitleSort = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            AuthorSort = sortOrder == "author" ? "author_desc" : "author";

            CurrentFilter = searchString;


            BookD.Books = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
                .AsNoTracking()
                .OrderBy(b => b.Title)
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var term = searchString.Trim().ToLower();

                BookD.Books = BookD.Books.Where(s =>
                    (s.Title?.ToLower().Contains(term) ?? false) ||
                    (s.Author?.FirstName?.ToLower().Contains(term) ?? false) ||
                    (s.Author?.LastName?.ToLower().Contains(term) ?? false) ||
                    (($"{s.Author?.FirstName} {s.Author?.LastName}")?.ToLower().Contains(term) ?? false)
                );
            }


            if (id!=null)
            {
                BookID = id.Value;
                Book book = BookD.Books
                    .Where(i => i.ID == id.Value).Single();
                BookD.Categories = book.BookCategories.Select(s => s.Category);
            }

            switch (sortOrder)
            {
                case "title_desc":
                    BookD.Books = BookD.Books.OrderByDescending(s =>
                   s.Title);
                    break;
                case "author_desc":
                    BookD.Books = BookD.Books.OrderByDescending(s =>
                   s.Author.FullName);
                    break;
                case "author":
                    BookD.Books = BookD.Books.OrderBy(s =>
                   s.Author.FullName);
                    break;
                default:
                    BookD.Books = BookD.Books.OrderBy(s => s.Title);
                    break;
            }

            Books = BookD.Books.ToList();
        }
    }
}
