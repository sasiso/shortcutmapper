using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace todobar
{
    /// <summary>
    /// 
    /// </summary>
    interface IDatabase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uniqueID"></param>
        /// <returns></returns>
        Data read(string uniqueID);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        bool save(Data input);

    }
}
