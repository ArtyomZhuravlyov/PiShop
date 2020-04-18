using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class ShippingDetails
    {

        [Required(ErrorMessage = "Укажите как вас зовут")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Укажите вашу фамилию")]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Укажите ваше отчество")]
        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Укажите город")]
        [Display(Name = "Город")]
        public string City { get; set; }

        [Required(ErrorMessage = "Укажите адрес доставки")]
        [Display(Name = "Адрес")]
        public string Adress { get; set; }

        [Required(ErrorMessage = "Укажите Ваш телефон (для уточнения информации)")]
        [Display(Name = "Ваш телефон")]
        public string OurPhone { get; set; }

        [Display(Name = "Телефон Получателя")]
        public string Phone { get; set; }


        [Display(Name = "Электронная почта")]
        public string Mail { get; set; }

        [Display(Name = "Комментарий к заказу")]
        public string Comment { get; set; }

        [Display(Name = "Доставка")]
        [HiddenInput(DisplayValue = false)]
        public string Delivery { get; set; }

        //пока без enum
       // [Required(ErrorMessage = "Выберите вид оплаты")]
       // [Display(Name = "Вид оплаты")]
        public bool TypePay { get; set; } = false;

        [Required(ErrorMessage = "Примите пользовательское соглашение")]
        public bool UserAccess { get; set; } = true;

    }

    public enum TypePay
    {
        /// <summary>
        /// картой
        /// </summary>
        Card,
        /// <summary>
        /// При получении
        /// </summary>
        Arm
    }
}
