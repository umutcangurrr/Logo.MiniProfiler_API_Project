using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAPI;
using Microsoft.EntityFrameworkCore;

namespace BusinessAPI
{
    public class ProductService
    {
        private readonly LogoApiDbContext _context;

        // ProductService yapılandırıcısı, LogoApiDbContexti alır
        public ProductService(LogoApiDbContext context)
        {
            _context = context;
        }

        // Tüm ürünleri asenkron olarak getirir
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        // Belirli bir ürünü kimliğe getirir.
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        // Yeni bir ürünü asenkron olarak ekler
        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        // Mevcut bir ürünü asenkron olarak günceller
        public async Task UpdateProductAsync(Product product)
        {
            var existingProduct = await _context.Products.FindAsync(product.Id);
            if (existingProduct != null)
            {
                _context.Entry(existingProduct).CurrentValues.SetValues(product);
                await _context.SaveChangesAsync();
            }
        }

        // Belirli bir ürünü kimliğe göre asenkron olarak siler
        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
