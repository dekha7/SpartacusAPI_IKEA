using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpartacusAPI
{
    public class Component
    {
        /// <summary>
        /// ComponentNo which is associated with any Article/Item
        /// </summary>
        public string CompNo { get; set; }

        /// <summary>
        /// Description of the component
        /// </summary>
        public string CompDesc { get; set; }

        /// <summary>
        /// Location of the component in Store
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Stock quantity which is available against the component in particular store
        /// </summary>
        public int? Stock { get; set; } = null;

        /// <summary>
        /// Description of the component image provided by the coWorker(s) during upload
        /// </summary>
        public string ImageDesc { get; set; }

        /// <summary>
        /// Image of the component
        /// </summary>
        //public byte[] Image { get; set; }

        /// <summary> Unique image ID associated against the component image </summary>
        public string ImageRequestID { get; set; }

    }
}

