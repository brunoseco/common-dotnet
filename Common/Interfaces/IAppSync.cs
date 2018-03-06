using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IApp
    {

        Task<IEnumerable<DataItem>> GetDataItem(IFilter filter);


    }
}
    