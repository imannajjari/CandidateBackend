using System.Globalization;
using System.Text.RegularExpressions;
using Candidate.Core.Widgets.Convertor;

namespace Candidate.Core.Widgets.Calendar;

public enum CalenderEnum
{
    Jalali = 1,
    Greg = 2,
    Hijri = 3
}
public static class CalenderWidget
{ 
    public static bool CheckJalaliDateRegex(this string date)
    {
        string pattern = @"^[1-4]\d{3}\/((0[1-6]\/((3[0-1])|([1-2][0-9])|(0[1-9])))|((1[0-2]|(0[7-9]))\/(30|([1-2][0-9])|(0[1-9]))))$";
        var reg = new Regex(pattern);
        var result = reg.IsMatch(date);
        return result;
    }

    public static string GetYearName(int year)
    {
        string result;
        string[] thousandsArray = new string[] { "", "یک هزار", "دو هزار" };
        string[] hundredsArray = new string[] { "", "صد", "دویست", "سیصد", "چهارصد", "پانصد", "ششصد", "هفتصد", "هشتصد", "نهصد" };
        string[] tenderdsArray = new string[] { "", "", "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود" };
        string[] irregularTenderdsArray = new string[] { "", "", "", "", "", "", "", "", "", "", "ده", "یازده", "دوازده", "سیزده", "چهارده", "پانزده", "شانزده", "هفده", "هجده", "نوزده", "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود" };
        string[] singlederdsArray = new string[] { "", "یک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه" };
        int thousands = Convert.ToInt32(year.ToString().Substring(0, 1));
        int hundreds = Convert.ToInt32(year.ToString().Substring(1, 1));
        int tenderds = Convert.ToInt32(year.ToString().Substring(2, 1));
        int singlederds = Convert.ToInt32(year.ToString().Substring(3, 1));
        if (tenderds == 0 && singlederds == 0)
        {
            tenderds = Convert.ToInt32(year.ToString().Substring(2, 2));
            result = thousandsArray[thousands] + " و " + hundredsArray[hundreds];
            return result;
        }
        else if (tenderds == 1)
        {
            tenderds = Convert.ToInt32(year.ToString().Substring(2, 2));
            result = thousandsArray[thousands] + " و " + hundredsArray[hundreds] + " و " + irregularTenderdsArray[tenderds];
            return result;
        }
        else if (tenderds == 0)
        {
            result = thousandsArray[thousands] + " و " + hundredsArray[hundreds] + " و " + singlederdsArray[singlederds];
            return result;
        }
        else if (singlederds == 0)
        {
            result = thousandsArray[thousands] + " و " + hundredsArray[hundreds] + " و " + tenderdsArray[tenderds];
            return result;
        }

        else
        {
            result = thousandsArray[thousands] + " و " + hundredsArray[hundreds] + " و " + tenderdsArray[tenderds] + " و " + singlederdsArray[singlederds];
            return result;
        }

    }


    public static string GetMonthName(int month, CalenderEnum type)
    {
        string result = string.Empty;
        string[] monthes = new string[] { "ناشناخته", "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };
        if (type == CalenderEnum.Jalali)
        {
            result = monthes[month];
            return result;
        }
        else if (type == CalenderEnum.Greg)
        {
            throw new Exception("Not Implemeneted!");
        }
        else if (type == CalenderEnum.Hijri)
        {
            throw new Exception("Not Implemeneted!");
        }
        else
            throw new Exception("Calender type is Invalid.");
    }



    public static string GetDayName(int day)
    {
        string[] days = new string[] {"ناشناخته","اول", "دوم", "سوم", "چهارم", "پنجم", "ششم", "هفتم", "هشتم", "نهم", "دهم", "یازدهم", "دوازدهم", "سیزدهم", "چهاردهم",
            "پانزدهم", "شانزدهم", "هفدهم", "هجدهم", "نوزدهم", "بیستم", "بیست و یکم", "بیست و دوم", "بیست و سوم", "بیست و چهارم", " بیست و پنجم",
            "بیست و ششم","بیست و هفتم","بیست و هشتم","بیست و نهم","سی ام","سی و یکم" };
        string result = days[day];
        return result;
    }

    public static int DateToKey(string date)
    {
        int result = 0;
        string res = date.Replace("/", "");

        try
        {
            result = Convert.ToInt32(res);
            return result;
        }
        catch 
        {
            result = 0;
            return result;
        }
    }

    public static int GetSeason(string date, CalenderEnum type)
    {
        var month = Convert.ToInt32(date.Substring(5, 2));
        if (type == CalenderEnum.Jalali)
        {
            if (month >= 1 && month <= 3)
                return 1;
            if (month >= 4 && month <= 6)
                return 2;
            if (month >= 7 && month <= 9)
                return 3;
            if (month >= 10 && month <= 12)
                return 4;
            else
                return 0;
        }
        else if (type == CalenderEnum.Greg)
        {
            if (month >= 3 && month <= 5)
                return 1;
            if (month >= 6 && month <= 8)
                return 2;
            if (month >= 9 && month <= 11)
                return 3;
            if (month == 1 || month == 2 || month == 12)
                return 4;
            else
                return 0;
        }
        else if (type == CalenderEnum.Hijri)
        {
            throw new Exception("Not Implemeneted!");
        }
        else
            throw new Exception("Calender type is Invalid.");

    }

    public static string GetSeasonName(int season)
    {
        string[] seasons = new string[] { "ناشناخته", "بهار", "تابستان", "پاییز", "زمستان", };
        string result = seasons[season];
        return result;
    }


    public static int GetJalaliDayOfWeek(DateTime gregDate)
    {
        int[] dayOfWeeksJalali = new int[] { 2, 3, 4, 5, 6, 7, 1 };
        int index = ((int)gregDate.DayOfWeek);
        int result = dayOfWeeksJalali[index];
        return result;
    }

    public static string GetJalaliDayOfWeekName(DateTime gregDate)
    {
        string[] dayOfWeeksJalali = new string[] { "یک شنبه", "دوشنبه", "سه شنبه", "چهارشنبه", "پنج شنبه", "جمعه", "شنبه", "ناشناخته" };
        int index = ((int)gregDate.DayOfWeek);
        string result = dayOfWeeksJalali[index];
        return result;
    }


    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی
    /// در مواقع غیرمتقن استفاده شود
    /// </summary>
    /// <param name="date"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string ToJalaliDate(this DateTime date, string separator = "/")
    {
        PersianCalendar myPersianCalender = new PersianCalendar();
        string day = myPersianCalender.GetDayOfMonth(date).ToString();
        string year = myPersianCalender.GetYear(date).ToString();
        string month = myPersianCalender.GetMonth(date).ToString();
        string result = year + separator + month.PadLeft(2, '0') + separator + day.PadLeft(2, '0');
        return (result);
    }
    /// <summary>
    /// تبدیل تاریخ شمسی صرفا عددی به تاریخ شمسی
    /// </summary>
    /// <param name="date">تاریخ شمسی صرفا عددی</param>
    /// <returns></returns>
    public static string ToJalaliDate(this string date)
    {
        if (string.IsNullOrEmpty(date)) return null;
        if (date.Length != 8) throw new Exception("طول رشته باید 8 کاراکتر باشد");
        try
        {
            Convert.ToInt32(date);
        }
        catch (Exception)
        {

            throw new Exception("نوع داده ورودی صحیح نمی‌باشد.");
        }
        date = date.Insert(6, "/");
        date = date.Insert(4, "/");
        return date;

    }

    /// <summary>
    /// متد تبديل تاريخ ميلادي به تاريخ و ساعت شمسي
    /// در مواقع غیرمتقن استفاده شود
    /// </summary>
    /// <param name="date">تاريخ ميلادي</param>
    /// <returns></returns>
    public static string ToJalaliDateTime(this DateTime date)
    {
        PersianCalendar myPersianCalender = new PersianCalendar();
        string day = myPersianCalender.GetDayOfMonth(date).ToString();
        string year = myPersianCalender.GetYear(date).ToString();
        string month = myPersianCalender.GetMonth(date).ToString();
        string result = year + "/" + month.PadLeft(2, '0') + "/" + day.PadLeft(2, '0') + " | " + date.ToString("HH:mm:ss");
        return (result);
    }

    /// <summary>
    /// تبدیل تاریخ شمسی به میلادی
    /// </summary>
    /// <param name="jalaliDate"></param>
    /// <returns></returns>
    public static DateTime ToGregDateTime(string jalaliDate)
    {
        DateTime result;
        if (!string.IsNullOrEmpty(jalaliDate))
        {
            int year = Convert.ToInt32(jalaliDate.Substring(0, 4));
            int month = Convert.ToInt32(jalaliDate.Substring(5, 2));
            int day = Convert.ToInt32(jalaliDate.Substring(8, 2));
            result = new DateTime(year, month, day, new System.Globalization.PersianCalendar());
        }
        else
        {
            result = new DateTime();
        }

        return result;
    }


    /// <summary>
    /// اعتبارسنجی تاریخ فارسی
    /// </summary>
    /// <param name="jalaliDate"></param>
    /// <returns></returns>
    public static bool ValidateJalaliDate(string jalaliDate)
    {
        bool isValid = false;
        try
        {
            int year = Convert.ToInt32(jalaliDate.Substring(0, 4));
            int month = Convert.ToInt32(jalaliDate.Substring(5, 2));
            int day = Convert.ToInt32(jalaliDate.Substring(8, 2));
            DateTime result = new DateTime(year, month, day, new System.Globalization.PersianCalendar());
            isValid = true;
        }
        catch (Exception)
        {

            isValid = false;
        }

        return isValid;
    }

    public static int GetJalaliYear(this DateTime date)
    {
        var jalaliDate = ToJalaliDate(date);
        var result = jalaliDate.Substring(0, 4);
        return result.ToInt();
    }
}