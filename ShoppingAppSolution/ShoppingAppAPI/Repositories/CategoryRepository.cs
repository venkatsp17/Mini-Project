using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exeptions.CategoryExceptions;
using ShoppingAppAPI.Interfaces.Repositories;
using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Repositories
{
    public class CategoryRepository : IRepository<int, Category>
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
            await _context.SaveChangesAsync(true);
            return item;

        }

        public async Task<Category> Get(int key)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryID == key) ?? throw new CategoryNotFoundException();
        }

        public async Task<IEnumerable<Category>> Get()
        {
            var result = await _context.Categories.ToListAsync();
            if (result.Count == 0)
            {
                throw new NoAvailableCategory();
            }
            return result;
        }

        public async Task<Category> Update(Category item)
        {
            var result = await Get(item.CategoryID);
            if (result == null)
            {
                throw new CategoryNotFoundException();
            }
            _context.Update(item);
            await _context.SaveChangesAsync(true);
            return item;
        }
    }
}
