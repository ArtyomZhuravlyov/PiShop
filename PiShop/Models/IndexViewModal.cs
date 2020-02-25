using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PiShop.Models
{
    public class IndexViewModal
    {
       public IEnumerable<Product> Products;

        public List<Product> HitsProducts;

        /// <summary>
        /// Флаг для окна об успешной оплате
        /// </summary>
        public bool SuccessOrder;
    }
}
