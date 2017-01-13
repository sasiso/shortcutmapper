using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortCutMapper
{
    /*!
     * This class represent Data asssociated with a todo time
     */
    public class Data
    {
        /// <summary>
        /// 
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RunInDirectory { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UniqueId { get; set; }

        static public string GenerateUniqueID()
        {
            return string.Format("{0}.txt", (object)Guid.NewGuid());
        }
    }
}
