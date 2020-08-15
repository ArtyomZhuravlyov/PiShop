using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
   public class Product
    {

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Хиты")]
        public bool Favourite { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }



        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }


        [Display(Name = "Категория1")]
        public string Category { get; set; }

     


        /// <summary>
        /// Цена
        /// </summary>
        [Display(Name = "Цена")]
        // public string Company { get; set; }
        public int Price { get; set; }


        /// <summary>
        /// Цена без Скидки
        /// </summary>
        [Display(Name = "Цена без Скидки")]
        public int PriceWithoutSales { get; set; }

        [Display(Name = "Адрес картинки")]
        /// <summary>
        /// Адрес картинки (формируется по ID)
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Описание в popupFade
        /// </summary>
        public string DescriptionPopup { get; set; }
        

        ///// <summary>
        ///// Отображается ли в ТОП
        ///// </summary>
        //public bool Favourite { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }

        public string Size { get; set; }

        [Display(Name = "Адрес второй картинки")]
        /// <summary>
        /// Адрес картинки (формируется по ID)
        /// </summary>
        public string Image2Address { get; set; }

        public int Sale { get; set; }

        public string TitleP { get; set; }

        public string Info { get; set; }

        public string Info2 { get; set; }

        //public string ImgAlt { get; set; }

        public static int RoundFive(double d)
        {
            // Округляем с точностью до 5
            if (d % 5 > 2.5)
                return (int)(d / 5) * 5 + 5;
            else
                return (int)(d / 5) * 5;
        }

    }
}
