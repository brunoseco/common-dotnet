﻿using Common.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure.Log
{
    public class Log4netComponent : ILog   
    {
        private readonly log4net.ILog log;

        public Log4netComponent()
        {
             this.log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
             log4net.Config.XmlConfigurator.Configure();
        }

        public void Debug(string message)
        {
            log.Debug(message);
        }
        public void DebugPriority(string message)
        {
            log.Debug(message);
        }

        public void Debug(string message, Exception exception)
        {
            log.Debug(message,exception);
        }

        public void Info(string message)
        {
            log.Info(message);
        }

        public void Info(string message, Exception exception)
        {
            log.Info(message,exception);
        }

        public void Warn(string message)
        {
            log.Warn(message);
        }

        public void Warn(string message, Exception exception)
        {
            log.Warn(message,exception);
        }

        public void Error(string message)
        {
            log.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            log.Error(message, exception);
        }

        public void Fatal(string message)
        {
            log.Fatal(message);
        }

        public void Fatal(string message, Exception exception)
        {
            log.Fatal(message, exception);
        }

        public void Debug(string format, params object[] arg)
        {
            log.DebugFormat(format, arg);
        }

        public void Info(string format, params object[] arg)
        {
            log.InfoFormat(format, arg);
        }

        public void Warn(string format, params object[] arg)
        {
            log.WarnFormat(format, arg);
        }

        public void Error(string format, params object[] arg)
        {
            log.ErrorFormat(format, arg);
        }

        public void Fatal(string format, params object[] arg)
        {
            log.FatalFormat(format, arg);
        }
       
    }
}