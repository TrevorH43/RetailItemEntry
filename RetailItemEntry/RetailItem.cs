using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
namespace RetailItemEntry
{
    public class RetailItem
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder($"Description = {Description}\n");
            sb.AppendLine($"Units On Hand = {UnitsOnHand}");
            sb.AppendLine($"Price = {Price}");
            return sb.ToString();
        }

        public RetailItem()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Description property used for getting/setting value
        /// </summary>
        [DisplayName("Item Description")]
        public string Description { get; set; }

        /// <summary>
        /// Units On Hand property used for getting/setting value
        /// </summary>
        [DisplayName("Units On Hand")]
        public double UnitsOnHand { get; set; }

        /// <summary>
        /// Price property used for getting/setting value
        /// </summary>
        [DisplayName("Price")]
        public double Price { get; set; }


        /// <summary>
        /// All parameter Constructor
        /// </summary>
        /// <param name="description">Description to populate object instance with</param>
        /// <param name="unitsOnHand">Units On Hand to populate object instance with</param>
        /// <param name="price">Price to populate object instance with</param>
        public RetailItem(string description, double unitsOnHand, double price)
        {
            Description = description;
            UnitsOnHand = unitsOnHand;
            Price = price;
        }
        

    }
}