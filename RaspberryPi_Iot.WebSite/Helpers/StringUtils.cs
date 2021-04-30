using System;
using System.Globalization;

/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


namespace LupenM.WebSite.Helpers
{
  public class StringUtils
  {
    public static string DecimalDigitFormat(string input, int digits)
    {
      string replace = input.Replace(',', '.');
      decimal number = Math.Round(Decimal.Parse(replace), digits);

      return number.ToString();
    }

    public static string FormatDate(DateTime? value, CultureInfo ci)
    {
      if (value == null)
      {
        return string.Empty;
      }

      return ci != null ? value.Value.ToString(ci) : value.Value.ToString();
    }

    public static string FormatDate(DateTime? value)
    {
      if (value == null)
      {
        return string.Empty;
      }

      return value.Value.ToString("dd.MM.yyyy HH:mm:ss");
    }

    public static string FormatDateExport(DateTime? value)
    {
      if (value == null)
      {
        return string.Empty;
      }

      return value.Value.ToString("dd.MM.yyyy_HH.mm.ss");
    }
  }
}