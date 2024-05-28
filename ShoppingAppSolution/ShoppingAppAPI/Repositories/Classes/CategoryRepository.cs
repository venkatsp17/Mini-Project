using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class CategoryRepository : BaseRepository<int, Category>, ICategoryRepository
    {

        public CategoryRepository(ShoppingAppContext context) : base(context)
        {

        }

        public async override Task<Category> Get(int key)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryID == key) ?? throw new NotFoundException("Category");
        }

        public async Task<Category> GetCategoryByName(string Name)
        {
            return await _context.Categories.Include(c => c.Products).ThenInclude(p => p.Seller).FirstOrDefaultAsync(c => c.Name == Name) ?? throw new NotFoundException("Category");
        }
    }
}
