using MyCrudProducts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCrudProducts.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
    }
}
