﻿using Common.Models;
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
        bool Async
        {
            get;
            set;
        }

        bool Running
        {
            get;
            set;
        }

        bool Executed
        {
            get;
            set;
        }

        bool ExecutedProcessDependency
        {
            get;
            set;
        }

        bool Disabled
        {
            get;
            set;
        }

        int? TaskGroupId
        {
            get;
            set;
        }

        void Execute(Action<IAsyncResult> completed);


    }
}
    