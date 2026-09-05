using System.Collections.Concurrent;
using ProductsApi.Models;

namespace ProductsApi.Services;

public class InMemoryProductService : IProductService
{
    private readonly ConcurrentDictionary<int, Product> _products = new();
    private int _nextId = 1;

    public InMemoryProductService()
    {
        // Datos semilla para poder probar la API apenas arranca
        Create(new Product { Name = "Teclado mecánico", Description = "Teclado mecánico RGB", Price = 45000m, Stock = 15 });
        Create(new Product { Name = "Mouse inalámbrico", Description = "Mouse ergonómico 2.4GHz", Price = 18000m, Stock = 30 });
        Create(new Product { Name = "Monitor 27\"", Description = "Monitor IPS 144Hz", Price = 210000m, Stock = 8 });
    }

    public IEnumerable<Product> GetAll()
    {
        return _products.Values.OrderBy(p => p.Id);
    }

    public Product? GetById(int id)
    {
        _products.TryGetValue(id, out var product);
        return product;
    }

    public Product Create(Product product)
    {
        var id = Interlocked.Increment(ref _nextId) - 1;
        product.Id = id;
        _products[id] = product;
        return product;
    }

    public bool Update(int id, Product product)
    {
        if (!_products.ContainsKey(id))
            return false;

        product.Id = id;
        _products[id] = product;
        return true;
    }

    public bool Delete(int id)
    {
        return _products.TryRemove(id, out _);
    }
}