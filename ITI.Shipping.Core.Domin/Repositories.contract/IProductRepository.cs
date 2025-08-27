using ITI.Shipping.Core.Domin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Shipping.Core.Domin.Repositories.contract;
public interface IProductRepository:IGenericRepository<Product,int>
{
    Task<IEnumerable<Product>> GetProductsWithOrderIdAsync(int orderId);
}
