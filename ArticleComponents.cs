using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpartacusAPI
{
    public class ArticleComponents
    {
        /// <summary>
        /// ArticleNo of the Article/Item
        /// </summary>
        public string ArticleNo { get; set; }

        /// <summary>
        /// Description of the Article/Item
        /// </summary>
        public string ArticleDesc { get; set; }

        /// <summary>
        /// Type of the Article/Item
        /// </summary>
        public string ArticleType { get; set; }

        /// <summary>
        /// Business Unit/StoreID of the component
        /// </summary>
        public string businessUnit { get; set; }

        public List<Component> Components { get; set; }
    }
}
