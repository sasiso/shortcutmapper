using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace todobar
{
    /*!
     * This class represent Data asssociated with a todo time
     */
    public class Data
    {
        /// <summary>
        /// 
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ColorName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UniqueId{ get; set; }
        public Data()
        {
            ColorName = "Grey";
        }

    }
}
