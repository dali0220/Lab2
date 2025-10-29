using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab2.Data;
using Lab2.Models;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly Lab2Context _context;

        public IndexModel(Lab2Context context)
        {
            _context = context;
        }

        public IList<Category> Category { get; set; } = new List<Category>();
        public CategoryIndexData CategoryData { get; set; }
        public int CategoryID { get; set; }

        public async Task OnGetAsync(int? id)
        {
            CategoryData = new CategoryIndexData();

            CategoryData.Categories = await _context.Category
                .Include(c => c.BookCategories)
                    .ThenInclude(bc => bc.Book)
                        .ThenInclude(b => b.Author)
                .AsNoTracking()
                .OrderBy(c => c.CategoryName)
                .ToListAsync();

            if (id != null)
            {
                CategoryID = id.Value;
                var category = CategoryData.Categories.Single(c => c.ID == id.Value);

                CategoryData.Books = category.BookCategories
                    .Select(bc => bc.Book)
                    .Where(b => b != null);
            }
        }
    }
}
