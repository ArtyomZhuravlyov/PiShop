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

        /// <summary>
        /// Комплект
        /// </summary>
        public IEnumerable<Product> Complex;

        /// <summary>
        /// Шорты
        /// </summary>
        public IEnumerable<Product> Pants;

        /// <summary>
        /// Алладины
        /// </summary>
        public IEnumerable<Product> Alladins;

        /// <summary>
        /// Подарочные упаковки
        /// </summary>
        public IEnumerable<Product> Gift;


        /// <summary>
        /// Флаг для окна об успешной оплате
        /// </summary>
        public bool SuccessOrder;
    }
}
