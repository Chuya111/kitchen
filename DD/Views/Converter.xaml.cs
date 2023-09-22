using System;
using System.Globalization;
using System.Windows.Data;

namespace DD
{
    /// <summary>
    /// Interaction logic for Converter.xaml
    /// </summary>
    public partial class MultiplyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int multiplier = System.Convert.ToInt16(parameter);
            int result = System.Convert.ToInt16(System.Convert.ToInt16(value) * 0.5) * multiplier;
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string input && double.TryParse(input, out double number))
            {
                int multiplier = System.Convert.ToInt16(parameter);
                int result = 0;
                if (number > 50)
                {
                    result = (int)(number * 2 / multiplier);
                }
                else
                {
                    result = (int)number;
                }
                //int result = System.Convert.ToInt16(System.Convert.ToInt16(value) * 0.5) ;
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
