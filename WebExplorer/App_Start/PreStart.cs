using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebActivatorEx;
using WebExplorer;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(PreStartApp), "Start")]
namespace WebExplorer
{
    public static class PreStartApp
    {
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Метод запускается один раз перед стартом приложения        
        /// </summary>
        public static void Start()
        {
            logger.Info("Application PreStart");
        }
    }
}
