using Domain;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Sber
{
    public class Sberbank
    {
        //https://developer.sberbank.ru/doc/v1/acquiring/webservice-requests1pay информация по документации 
        private Cart _cart;
        private ShippingDetails _shippingDetails;
        private string _merchantOrderNumber;
#if DEBUG
        private const string RETURN_URL = "https://localhost:44338/SuccessOrder";// "https://kladovayaltay.ru/";//"https://localhost:44338/Home/Feedback";
#else
        private const string RETURN_URL = "https://kladovayaltay.ru/SuccessOrder";
#endif



        public Sberbank(string merchantOrderNumber, Cart cart, ShippingDetails shippingDetails)
        {
            _merchantOrderNumber = merchantOrderNumber;
            _cart = cart;
            _shippingDetails = shippingDetails;
        }

        public string GetResponseSoap(string _url = "https://3dsec.sberbank.ru/payment/webservices/merchant-ws", string _method = "registerOrder", string _soapEnvelope = "")
        {
            StringBuilder Soap = new StringBuilder();
          //  CreateStartHeaderSoap(Soap); добавить заголвок ту ду
            GetOrderBundleSoap(Soap);
            _soapEnvelope = Soap.ToString(); //= File.ReadAllText(@"C:\Users\79504\source\repos\Klad2_0\Klad2_0\wwwroot\TestSoap.xml");
            _url = _url.Trim('/').Trim('\\'); // в конце адреса удалить слэш, если он имеется
            WebRequest _request = HttpWebRequest.Create(_url);
            _request.Method = "POST";
            _request.ContentType = "text/xml; charset=utf-8";
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] bytes = encoding.GetBytes(_soapEnvelope);
            _request.ContentLength = bytes.Length;
            _request.Headers.Add("SOAPAction", _url + @"/" + _method);
            // пишем тело
            Stream _streamWriter = _request.GetRequestStream();
            _streamWriter.Write(bytes, 0, bytes.Length);
            _streamWriter.Close();
            // читаем тело
            try
            {
                WebResponse _response = _request.GetResponse();
                StreamReader _streamReader = new StreamReader(_response.GetResponseStream());
                string _result = _streamReader.ReadToEnd(); // переменная в которую пишется результат (ответ) сервиса
                _result = _result.Substring(_result.IndexOf("<formUrl>") + "<formUrl>".Length, _result.IndexOf("</formUrl>") - _result.IndexOf("<formUrl>") - "<formUrl>".Length);
                return _result;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// Формируем основное тело запроса 
        /// </summary>
        /// <param name="soap"></param>
        private void GetOrderBundleSoap(StringBuilder soap)
        {
            soap.Append("<mer:registerOrder>");
            //заголовок Order
            soap.Append($"<order merchantOrderNumber=\"{_merchantOrderNumber}\" description = \"\" amount = \"{_cart.ComputeTotalValueWithDelivery().ToString()}00\">\n");
            // expirationDate = \"{DateTime.Now.ToUniversalTime().ToString("s")}\ currency = \"\" language = " +
            // $"\"\" pageView = \"\" sessionTimeoutSecs=\"0\" taxSystem=\"0\" bindingId=\"\" autocompletionDate=\"\""
            soap.Append($"<returnUrl>{RETURN_URL}</returnUrl>\n");
            soap.Append("<orderBundle>\n");
            {
                soap.Append($"<orderCreationDate>{DateTime.Now.ToUniversalTime().ToString("s")}</orderCreationDate>\n");
                soap.Append("<customerDetails>\n");
                {
                    soap.Append($"<email>{_shippingDetails.Mail}</email>\n");
                    soap.Append($"<phone>{_shippingDetails.Phone}</phone>\n");

                    soap.Append("<deliveryInfo>\n");
                    {
                        soap.Append($"<country>RU</country>\n");
                        soap.Append($"<city>{_shippingDetails.City}</city>\n");
                        soap.Append($"<postAddress>{_shippingDetails.Adress}</postAddress>\n");

                    }
                    soap.Append("</deliveryInfo>\n");
                    soap.Append($"<fullName>{_shippingDetails.Name + _shippingDetails.Surname + _shippingDetails.MiddleName}</fullName>\n");
                }
                soap.Append("</customerDetails>\n");
                // сборка корзины
                soap.Append("<cartItems>\n");
                GetCartItemSoap(soap);
                soap.Append("</cartItems>\n");
            }

            soap.Append("</orderBundle>\n");
            //end
            soap.Append("</order>");
            soap.Append("</mer:registerOrder>");
            soap.Append("</soapenv:Body>");
            soap.Append("</soapenv:Envelope>");

        }

        /// <summary>
        /// Получаепм товары из корзины плюс доставка
        /// </summary>
        /// <returns></returns>
        private void GetCartItemSoap(StringBuilder soap)
        {

            for (int i = 1; i <= _cart.TotalQuantity; i++)
            {
                CartLine cartLine = _cart.GetCartLine(i - 1);
                soap.Append($"<items positionId=\"{(i /*+ 1*/).ToString()}\">\n");
                soap.Append($"<name>{cartLine.productCart.Name + cartLine.productCart.Size}</name>\n");
                soap.Append($"<quantity measure=\"штук\">{cartLine.Quantity}</quantity>\n");
                soap.Append($"<itemAmount>{cartLine.LineAmount}00</itemAmount>\n");
                soap.Append($"<itemCode>{cartLine.productCart.Id.ToString()}</itemCode>\n");

                soap.Append("<tax>\n");
                soap.Append("<taxType>0</taxType>\n");
                soap.Append("<taxSum>0</taxSum>\n");
                soap.Append("</tax>\n");
                soap.Append($"<itemPrice>{cartLine.productCart.Price}00</itemPrice>\n");
                soap.Append("</items>\n");
            }
            if (_cart.DeliveryPrice > 0)
            {
                soap.Append($"<items positionId=\"{(_cart.TotalQuantity + 1).ToString()}\">\n");
                soap.Append($"<name>Доставка</name>\n");
                soap.Append($"<quantity measure=\"штук\">1</quantity>\n");
                soap.Append($"<itemAmount>{_cart.DeliveryPrice}00</itemAmount>\n");
                soap.Append($"<itemCode>0001</itemCode>\n");

                soap.Append("<tax>\n");
                soap.Append("<taxType>0</taxType>\n");
                soap.Append("<taxSum>0</taxSum>\n");
                soap.Append("</tax>\n");
                soap.Append($"<itemPrice>{ _cart.DeliveryPrice}00</itemPrice>\n");
                soap.Append("</items>\n");
            }
        }

    }
}
