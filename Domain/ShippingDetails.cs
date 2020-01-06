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

        [Required(ErrorMessage = "Укажите телефон")]
        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Укажите электронную почту")]
        [Display(Name = "Электронная почта")]
        public string Mail { get; set; }

        [Display(Name = "Комментарий к заказу")]
        public string Comment { get; set; }

        [Display(Name = "Доставка")]
        [HiddenInput(DisplayValue = false)]
        public string Delivery { get; set; }

        [Required(ErrorMessage = "Примите пользовательское соглашение")]
        public bool UserAccess { get; set; } = true;

    }
}
