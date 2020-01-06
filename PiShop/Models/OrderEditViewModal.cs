using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PiShop.Models
{
 
    public class OrderEditViewModal
    {
        public Order order { get; set; }

        public List<XmlCartLine> XmlCartLineList { get; set; }


    }
    
}
