using ITI.Shipping.Core.Domin.Entities;
using ITI.Shipping.Core.Domin.Repositories.contract;
using ITI.Shipping.Infrastructure.Presistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Shipping.Infrastructure.Presistence.Repositories;
public class ProductRepository:GenericRepository<Product,int>, IProductRepository
{
    private readonly ApplicationContext _applicationContext;

    public ProductRepository(ApplicationContext applicationContext) : base(applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<IEnumerable<Product>> GetProductsWithOrderIdAsync(int orderId)
    {
        return await _applicationContext.Products
          .Where(p => p.OrderId == orderId).ToListAsync();
    }
}
