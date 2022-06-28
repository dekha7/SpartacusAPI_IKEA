using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpartacusAPI
{
    public class UpdateComponentStock
    {
        /// <summary>
        /// Business Unit/StoreID of the component
        /// </summary>
        public string businessUnit { get; set; }

        /// <summary>
        /// ArticleNo of the Article/Item
        /// </summary>
        public string articleNo { get; set; }

        /// <summary>
        /// ComponentNo which is associated with any Article/Item
        /// </summary>
        public string compNo { get; set; }

        /// <summary>
        /// Quantity of the Component, It can be in +/- both based on addition/substraction need to perform against the component stock(ie. +4/-4)
        /// </summary>
        public int? quantity { get; set; }
    }
}
