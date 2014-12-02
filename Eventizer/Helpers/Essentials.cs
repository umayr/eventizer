using Eventizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Eventizer.Helpers
{
    public static class Essentials
    {
        private static readonly string[] PRIORITY_COLORS = { "#d9534f", "#f0ad4e", "#5bc0de", "#3c763d", "#fcfcfc" };
        private static readonly string[] PRIORITY_CLASS = { "danger", "warning", "info", "success", "default" };
        public static string CalculateSHA1(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);

            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return Essentials.HexStringFromBytes(hashBytes);
        }
        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }
        public static bool CheckIfAuthenticated()
        {
            try
            {
                if (HttpContext.Current.Session["Auth"] == null)
                {
                    return false;
                }
                else
                {
                    if ((bool)HttpContext.Current.Session["Auth"] == false)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string TrimLongText(string Text, int TrimTo = 100)
        {
            if (string.IsNullOrEmpty(Text))
            {
                return "...";
            }
            else
            {
                if (TrimTo > Text.Length)
                {
                    return Text;
                }
                else
                {
                    return Text.Substring(0, TrimTo) + "...";
                }
            }

        }
        private static int CalculatePriority(DateTime Deadline, bool Status)
        {
            if (Status)
            {
                return 3;
            }
            else
            {
                TimeSpan T = Deadline.Subtract(DateTime.Now);
                double daysLeft = T.TotalDays;
                if (daysLeft < 2.5)
                {
                    return 0;
                }
                else if (daysLeft < 6)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }

        }

        public static string CalculatePriorityColor(DateTime Deadline, bool Status)
        {
            return Essentials.PRIORITY_COLORS[Essentials.CalculatePriority(Deadline, Status)];
        }
        public static string CalculatePriorityClass(DateTime Deadline, bool Status)
        {
            return Essentials.PRIORITY_CLASS[Essentials.CalculatePriority(Deadline, Status)];
        }

        public static bool IfDeadlineHasPassed(DateTime Deadline, bool Status)
        {
            if (Status)
            {
                return true;
            }
            else
            {
                TimeSpan T = Deadline.Subtract(DateTime.Now);
                double daysLeft = T.TotalDays;
                if (daysLeft < 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public static int GetFeedType(string Type)
        {
            int type = -1;
            if (Type == "Event")
            {
                type = 0;
            }
            else if (Type == "Task")
            {
                type = 1;
            }
            else if (Type == "Subtask")
            {
                type = 2;
            }
            return type;
        }
    }
}