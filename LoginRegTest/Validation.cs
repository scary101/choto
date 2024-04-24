using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace LoginRegTest
{
    internal class Validation
    {
        public bool CheckCyrillicAndSpecialSymbol(string stroka)
        {
            bool empty = !string.IsNullOrWhiteSpace(stroka);
            var cyrillic = Enumerable.Range(1024, 256).Select(ch => (char)ch);
            bool res = stroka.Any(cyrillic.Contains);
            var symb = new Regex("[^a-zA-Z0-9_.]");
            if (res || symb.IsMatch(stroka.Replace(" ", "")) && !empty)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool CheckNameSurMiddle(string stroka)
        {
            bool empty = !string.IsNullOrWhiteSpace(stroka);
            Regex regex = new Regex(@"[^a-zA-Zа-яА-Я\s]");
            if (!regex.IsMatch(stroka) && stroka.Split().Count() == 3 && empty)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool CheckEmail(string email)
        {
            bool empty = !string.IsNullOrWhiteSpace(email);
            var cyrillic = Enumerable.Range(1024, 256).Select(ch => (char)ch);
            bool res = email.Any(cyrillic.Contains);
            if (!res && Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$") && empty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public  bool CheckPhone(string phone)
        {
            if (phone.Length != 11)
            {
                return false;
            }

            if (phone[0] != '8' && phone[0] != '7')
            {
                return false;
            }

            foreach (char c in phone)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            return true;
        }
        public bool IsValidCardNumber(string cardNumber)
        {
            if (cardNumber.Length != 16)
            {
                return false;
            }

            foreach (char c in cardNumber)
            {
                if (!Char.IsDigit(c))
                {
                    return false;
                }
            }
            Regex regex = new Regex(@"^[0-9]{16}$");
            return regex.IsMatch(cardNumber);
        }
        public bool IsValidCvvCard(string cvv)
        {
            if (cvv.Length == 3)
            {
                foreach (char c in cvv)
                {
                    if (c < '0' || c > '9')
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }
        public bool CheckValidNum(string cost)
        {
            try
            {
                Convert.ToInt32(cost);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public  bool IsOnlyBykvi(string stroka)
        {
            Regex regex = new Regex("[^a-zA-Zа-яА-Я\\s]");
            if (!regex.IsMatch(stroka))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
