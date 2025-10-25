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

        public async Task OnGetAsync()
        {
            Books = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.BookCategories).ThenInclude(bc => bc.Category)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
