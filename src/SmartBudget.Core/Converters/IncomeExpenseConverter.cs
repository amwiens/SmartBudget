using SmartBudget.Core.Models;

using System;
using System.Globalization;
using System.Windows.Data;

namespace SmartBudget.Core.Converters
{
    public class IncomeExpenseConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            TransactionType transactionType = (TransactionType)values[0];
            decimal amount = (decimal)values[1];
            int accountId = int.Parse(values[2].ToString());
            int targetAccountId = !(values[3] is null) ? int.Parse(values[3].ToString()) : 0;

            switch (transactionType)
            {
                case TransactionType.Income:
                    return $"+ {amount:c}";

                case TransactionType.Expense:
                    return $"- {amount:c}";

                case TransactionType.Transfer:
                    if (accountId != targetAccountId)
                        return $"- {amount:c}";
                    else
                        return $"+ {amount:c}";
            }

            return "";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}