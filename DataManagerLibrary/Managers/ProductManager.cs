using DataManagerLibrary.Context;
using DataManagerLibrary.Managers.Abstract;
using DomainLibrary;
using Microsoft.EntityFrameworkCore;

namespace DataManagerLibrary.Managers
{
    public class ProductManager : EntityManagerBase<Product>
    {
        public ProductManager(ApplicationDataContext context) : base(context)
        {
        }
        public override async Task<ICollection<Product>> GetAllAsync()
        {
            try
            {
                return await _context.Products.ToListAsync();
            }
            catch (Exception)
            {
                return new List<Product>();
            }
        }

        public override async Task<Product?> GetAsync(int id)
        {
            try
            {
                return await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override async Task<bool> AddAsync(Product entity)
        {
            try
            {
                await _context.Products.AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override async Task<bool> UpdateAsync(Product entity)
        {
            try
            {
                _context.Products.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var item = await _context.Products.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
                if (item is null) return false;
                _context.Products.Remove(item);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}
