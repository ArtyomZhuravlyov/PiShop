using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Domain
{
    public class Cart
    {

        private List<CartLine> lineCollection = new List<CartLine>();

        public int TotalQuantity { get { return lineCollection.Count(); } }

        /// <summary>
        /// to do Скидка по всем товарам
        /// </summary>
        public long Discount { get; set; }

        /// <summary>
        /// Значение суммы товаров, после которой доставка будет бесплатной
        /// </summary>
        public const int SUMM_FOR_SALE = 3;//000;

        /// <summary>
        /// Изначальная Цена доставки
        /// </summary>
        public const int DELIVERY_PRICE = 350;//000;

        /// <summary>
        /// Минимальная сумма для покупки
        /// </summary>
        public const int LIMIT_AMOUNT = 3;//000;

        /// <summary>
        /// Стоимость доствкпи (зависит от общей стоимости)
        /// </summary>
        public int DeliveryPrice
        {
            get
            {
                if (ComputeTotalValue() > SUMM_FOR_SALE)
                    return 0;
                else return DELIVERY_PRICE;
            }
        }

        public void AddItem(Product product, int quantity)
        {
            CartLine line = lineCollection
                .Where(g => g.productCart.Id == product.Id && g.productCart.Size == product.Size) //исключительно для магзинов с размерами
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    productCart = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        /// <summary>
        /// Удаляет только одну штуку выбранного товара
        /// </summary>
        /// <param name="product"></param>
        /// <param name="quantity"></param>
        public void RemoveItem(Product product, int quantity)
        {
            CartLine line = lineCollection
                .Where(g => g.productCart.Id == product.Id && g.productCart.Size == product.Size)
                .FirstOrDefault();

            if (line != null)
            {
                line.Quantity -= quantity;

            }
            else
            {
                //такое не должно произойти
            }
        }

        /// <summary>
        /// Удаляет полностью товар из корзины (неважно какое количество этого товара было)
        /// </summary>
        /// <param name="product"></param>
        public void RemoveLine(Product product)
        {
            lineCollection.RemoveAll(l => l.productCart.Id == product.Id && l.productCart.Size == product.Size);
        }

        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.productCart.Price * e.Quantity);

        }

        public int ComputeTotalValueWithDelivery()
        {
            return lineCollection.Sum(e => e.productCart.Price * e.Quantity) + DeliveryPrice;

        }
        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }

        public CartLine GetCartLine(int number)
        {
            return lineCollection[number];
        }


        /// <summary>
        /// Получаем Приобретённые товары пользователем в xml формате
        /// </summary>
        /// <returns></returns>
        public string GetXmlLineCollection()
        {
            List<XmlCartLine> xmlCartLines = new List<XmlCartLine>();

            foreach (var Line in this.lineCollection)
            {
                xmlCartLines.Add(new XmlCartLine()
                {
                    ProductName = Line.productCart.Name,
                    Quantity = Line.Quantity,
                    Price = Line.productCart.Price,
                    Id = Line.productCart.Id,
                    PictureAddress = Line.productCart.Address,
                    Size = Line.productCart.Size
                });
            }
            // передаем в конструктор тип класса
            XmlSerializer formatter = new XmlSerializer(typeof(List<XmlCartLine>));

            string xml;
            using (StringWriter sw = new StringWriter())
            {
                formatter.Serialize(sw, xmlCartLines);
                xml = sw.ToString();
            }
            return xml;
        }


        /// <summary>
        /// Переводим из базы xml формат в обычный класс
        /// </summary>
        /// <param name="xmlCartLines">берём строку хмл формата из базы</param>
        /// <returns></returns>
        static public List<XmlCartLine> GetLineCollecionFromXML(string xmlCartLines)
        {
            // передаем в конструктор тип класса
            XmlSerializer formatter = new XmlSerializer(typeof(List<XmlCartLine>));

            using (MemoryStream sw = new MemoryStream())
            {
                List<XmlCartLine> XmlCartLines = (List<XmlCartLine>)formatter.Deserialize(sw);
                return XmlCartLines;// xml = sw.ToString();
            }
        }

    }



    public class CartLine
    {
        public Product productCart { get; set; }
        public int Quantity { get; set; }

        public long LineAmount
        {
            get
            {
                return Quantity * productCart.Price;
            }

        }
    }

    [Serializable]
    public class XmlCartLine
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }

        public int Price { get; set; }

        /// <summary>
        /// Всего по одному типутоваров
        /// </summary>
        public int PriceTotal { get { return Quantity * Price; } }

        public string PictureAddress { get; set; }

        public string Size { get; set; }

    }
}
