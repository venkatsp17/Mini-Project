using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly ShoppingAppContext _context;
        public CategoryRepository(ShoppingAppContext context)
        {
            _context = context;
        }

        public async Task<Category> Add(Category item)
        {
            _context.Categories.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Category> Delete(int key)
        {
            var item = await Get(key);
            _context.Categories.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<Category>> Get()
        {
            return await _context.Categories.ToListAsync() ?? throw new NoAvailableItemException("Categories");
        }

        public async Task<Category> Update(Category item)
        {
            _context.Categories.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Category> GetCategoryByName(string Name)
        {
            return await _context.Categories.Include(c => c.Products).ThenInclude(p => p.Seller).FirstOrDefaultAsync(c => c.Name == Name) ?? throw new NotFoundException("Category");
        }

        public async Task<Category> Get(int key)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryID == key) ?? throw new NotFoundException("Category");
        }
    }
}
