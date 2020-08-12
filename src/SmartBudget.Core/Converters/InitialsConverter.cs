using System;
using System.Globalization;
using System.Windows.Data;

namespace SmartBudget.Core.Converters
{
    public class InitialsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = value as string;
            string i = string.Empty;

            if (s != null)
            {
                string[] split = s.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (string piece in split)
                {
                    i += piece[0];
                }
            }

            if (i.Length > 2)
                return i.Substring(0, 2);

            return i;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}