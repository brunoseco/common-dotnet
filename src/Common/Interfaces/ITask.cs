using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface ITask : ISchadule
    {
        string TaskName
        {
            get;
            set;
        }

        bool Running
        {
            get;
            set;
        }

        bool Schaduled
        {
            get;
            set;
        }


        bool Disabled
        {
            get;
            set;
        }


        void Execute(string token, string attributeBehavior, bool schaduleOn = false, Action<IAsyncResult> completed = null, int? escolaId = null);

        void ChkAuth();


    }
}
