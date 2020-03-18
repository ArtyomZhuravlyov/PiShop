using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PiShop.Models
{
    public class IndexViewModal
    {
       //public IEnumerable<Product> Products;

        public List<Product> HitsProducts;

        public IEnumerable<Product> SimpleTop;

        public IEnumerable<Product> ShortTop;

        public IEnumerable<Product> Phlis;

        public IEnumerable<Product> Pants;

        /// <summary>
        /// Флаг для окна об успешной оплате
        /// </summary>
        public bool SuccessOrder;
    }
}
