using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
  public  class Order
    {
        public int Id { get; set; }

        [Display(Name = "Принято")]
        public bool Received { get; set; }

        [Display(Name = "Отправлено")]
        public bool Shipped { get; set; }

        [Display(Name = "Доставлено")]
        public bool Delivered { get; set; }

        /// <summary>
        /// Итоговая сумма с доставкой
        /// </summary>
        [Display(Name = "Итоговая сумма с доставкой")]
        public int Amount { get; set; }


        [Display(Name = "Оплачено")]
        public bool Paid { get; set; }

        /// <summary>
        /// Включает товары и их количество
        /// </summary>
        public string OrdersAndQuantity { get; set; }
        // public List<CartLine> aasd;

        [Display(Name = "Дата и время")]
        public DateTime TimeOrder { get; set; }


        [Display(Name = "Имя")]
        public string Name { get; set; }


        [Display(Name = "Фамилия")]
        public string Surname { get; set; }


        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }


        [Display(Name = "Город")]
        public string City { get; set; }


        [Display(Name = "Адрес")]
        public string Adress { get; set; }


        [Display(Name = "Телефон")]
        public string Phone { get; set; }


        [Display(Name = "Электронная почта")]
        public string Mail { get; set; }

        public string Comment { get; set; }

        //[HiddenInput(DisplayValue = false)]
        //public string Delivery { get; set; }


        /// <summary>
        /// Вид оплаты
        /// </summary>
        public string TypePay { get; set; }

    }
}
