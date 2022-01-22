using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebExplorer.Models
{
    public enum TaskStates
    {
        /// <summary>
        /// в процессе выполнения
        /// </summary>
        InProcess=1,
        /// <summary>
        /// зависла
        /// </summary>
        Stuck=2,
        /// <summary>
        /// на проверке
        /// </summary>
        Checking=3,
        /// <summary>
        /// создана
        /// </summary>
        Created=4,
        /// <summary>
        /// завершена
        /// </summary>
        Finished=5
    }
    public class TaskStatesFormatter
    {

        public static string ToString(TaskStates value)
        {
            if ((value == TaskStates.InProcess))
            {
                return "в процессе выполнения";
            }
            if ((value == TaskStates.Stuck))
            {
                return "зависла";
            }
            if ((value == TaskStates.Checking))
            {
                return "на проверке";
            }
            if ((value == TaskStates.Created))
            {
                return "создана";
            }
            if ((value == TaskStates.Finished))
            {
                return "завершена";
            }
            throw new System.ArgumentException("Неизвестное значение");
        }
        public static int ToInt(TaskStates value)
        {
            if ((value == TaskStates.InProcess))
            {
                return 1;
            }
            if ((value == TaskStates.Stuck))
            {
                return 2;
            }
            if ((value == TaskStates.Checking))
            {
                return 3;
            }
            if ((value == TaskStates.Created))
            {
                return 4;
            }
            if ((value == TaskStates.Finished))
            {
                return 5;
            }
            throw new System.ArgumentException("Неизвестное значение");
        }
    }
}