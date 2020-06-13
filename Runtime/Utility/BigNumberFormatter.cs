using System;
using System.Text;

namespace Gameframe.GUI.Utility
{

  public class BigNumberFormatter : IFormatProvider, ICustomFormatter
  {

    static string[] symbols =
    { "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
      "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"
    };

    static string[] specialSymbols = { "K", " M", "B", "T" };

    public string Format(string formatText, object arg, IFormatProvider formatProvider)
    {
      if (arg is float)
      {
        return InternalFormat(formatText, (double)arg, formatProvider);
      }
      else if (arg is double)
      {
        return InternalFormat(formatText, (double)arg, formatProvider);
      }
      else if (arg is long)
      {
        return InternalFormat(formatText, (double)arg, formatProvider);
      }
      else if (arg is int)
      {
        return InternalFormat(formatText, (double)arg, formatProvider);
      }
      else
      {
        return arg.ToString();
      }
    }

    private static string InternalFormat(string format, double arg, IFormatProvider formatProvider)
    {

      int significantDigits = (int)(Math.Log10(arg) + 1);

      //Number of commas is number of digits divided by 3 and minus 1. 
      int commas = (significantDigits / 3) - 1;

      var formatBuilder = new StringBuilder();
      formatBuilder.Append("#");
      if (commas > 0)
      {
        //Add a comma to format to skip 3 number digits
        //Subtract 1 because we want to show the digits past one comma
        for (int i = 0; i < commas; i++)
        {
          formatBuilder.Append(",");
        }

        int index = commas;
        if ( index > 0 && index <= specialSymbols.Length )
        {
          formatBuilder.Append(specialSymbols[index - 1]);
        }
        else
        {
          //Number of special symbols needs to be subtracted so we don't
          //skip symbols
          index -= specialSymbols.Length;

          string symbol = string.Empty;

          while (index > 0)
          {

            var symbolIndex = (index - 1) % symbols.Length;

            symbol = symbols[symbolIndex] + symbol;

            index = (index - 1) / symbols.Length;

          }

          formatBuilder.Append(symbol);

        }

      }

      return arg.ToString(formatBuilder.ToString());

    }

    public object GetFormat(Type formatType)
    {
      if ( formatType == typeof(double) || formatType == typeof(ICustomFormatter) )
      {
        return this;
      }
      return null;
    }

  }

}
