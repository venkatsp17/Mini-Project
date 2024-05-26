using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class CategoryRepository : BaseRepository<int, Category>
    {

        public CategoryRepository(ShoppingAppContext context) : base(context)
        {

        }

        public async override Task<Category> Get(int key)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryID == key) ?? throw new NotFoundException("Category");
        }
    }
}
