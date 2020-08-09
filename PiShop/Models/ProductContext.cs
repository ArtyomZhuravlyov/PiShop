using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PiShop.Models
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Для статистики купленных пижам
        /// </summary>
        public DbSet<OrderPizham> OrderPizham { get; set; }

        //public DbSet<Feedback> Feedback { get; set; }

        //public DbSet<WordsSearch> WordsSearch { get; set; }

        public ProductContext(DbContextOptions<ProductContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }


        /// <summary>
        /// Отдаёт самые популярые товары
        /// </summary>
        /// <returns></returns>
        public List<Product> GetFavoutiteProducts()
        {
            List<Product> items = Products.Where(x => x.Favourite == true).ToList();
            return items;
        }

        public void SaveProduct(Product product)
        {
            if (product.Id == 0)
                this.Products.Add(product);
            else
            {
                Product dbEntry = this.Products.Find(product.Id);

                #region сохранение свойств
                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Favourite = product.Favourite;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.PriceWithoutSales = product.PriceWithoutSales;
                    dbEntry.Sale = product.Sale;
                    dbEntry.Category = product.Category;
                    dbEntry.Address = product.Address;
                    dbEntry.ImageData = product.ImageData;
                    dbEntry.ImageMimeType = product.ImageMimeType;
                    dbEntry.Image2Address = product.Image2Address;
                    //dbEntry.ImgAlt = product.ImgAlt;
                    dbEntry.TitleP = product.TitleP;
                    dbEntry.Info = product.Info;
                    dbEntry.Info2 = product.Info2;
                }
                #endregion

            }
            this.SaveChanges();
        }


        public Product DeleteProduct(int productId)
        {
            Product dbEntry = this.Products.Find(productId);
            if (dbEntry != null)
            {
                this.Products.Remove(dbEntry);
                this.SaveChanges();
            }
            return dbEntry;
        }

        public int FindNextId(int Id)
        {
            
                int LastId = this.Products.Last().Id;
                for (int i = Id + 1; i <= LastId; i++)
                {
                    if (Products.Find(i) != null)
                        return i;
                }
                return 0;
            
           
        }
    }   
}
