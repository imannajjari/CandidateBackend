using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Candidate.Core.Widgets.Stream;

public sealed class StreamWidget : IDisposable
{
    private readonly string _directory;

    private readonly Dictionary<string, StreamWriter> _streams;

    private readonly Timer _timer;

    private readonly object _lock;

    public StreamWidget()
    {

    }

    public StreamWidget(string directory)
    {

        _directory = directory;
        _streams = new Dictionary<string, StreamWriter>();
        _lock = new object();
        _timer = new Timer(ClosePastStreams, null, 0, (int)TimeSpan.FromHours(2).TotalMilliseconds);

    }

    public void Dispose()
    {
        _timer.Dispose();
        CloseAllStreams();
    }


    /// <summary>
    /// خواندن محتویات یک فایل متنی
    /// </summary>
    /// <param name="fileName">مسیر و نام فایل</param>
    /// <returns></returns>
    public string Read(string fileName)
    {
        string result = String.Empty;
        if (File.Exists(fileName))
        {
            result = File.ReadAllText(fileName);
        }
        return result;
    }

    /// <summary>
    /// نوشتن محتویات در ادامه فایل قبلی
    /// </summary>
    /// <param name="date">تاریخ ایجاد فایل</param>
    /// <param name="fileName">نام فایل</param>
    /// <param name="content">محتویات فایل</param>
    public void Append(DateTime date, string fileName, string content)
    {
        try
        {
            lock (_lock)
            {
                CreateOrGetStream(date.Date, fileName).WriteLine(content);
            }
        }
        catch
        {
            //در اینجا از خطا جلوگیری می کنیم که باعث خطا در روند اپلیکیشن و ایجاد لوپ نشود
        }
    }

    /// <summary>
    /// ایجاد فایل 
    /// </summary>
    /// <param name="fileName">نام فایل</param>
    /// <param name="extension">فرمت فایل</param>
    /// <param name="content">محتویات فایل</param>
    public void CreateFile(string fileName, string extension, string content)
    {
        lock (_lock)
        {
            CreateOrGetStream($"{fileName}.{extension}").WriteLine(content);
            _streams[$"{fileName}.{extension}"].Close();
            _streams.Remove($"{fileName}.{extension}");

        }
    }




    /// <summary>
    /// بستن فایل  قبلی
    /// </summary>
    /// <param name="ignored"></param>
    private void ClosePastStreams(object ignored)
    {
        lock (_lock)
        {
            var today = DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture).Replace("-", "");

            var past = _streams.Where(kvp => string.CompareOrdinal(kvp.Key.Substring(kvp.Key.Length - 8), today) < 0).ToList();

            foreach (var kvp in past)
            {
                kvp.Value.Dispose();
                _streams.Remove(kvp.Key);
            }
        }
    }

    /// <summary>
    /// بستن همه فایل های قبلی
    /// </summary>
    private void CloseAllStreams()
    {
        lock (_lock)
        {
            foreach (var stream in _streams.Values)
                stream.Dispose();

            _streams.Clear();
        }
    }

    /// <summary>
    /// ایجاد فایل جدید یا خوانش فایل قبلی در صورت وجود
    /// </summary>
    /// <param name="date"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "It's disposed on this class Dispose.")]
    private StreamWriter CreateOrGetStream(DateTime date, string name)
    {
        var strDate = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        return CreateOrGetStream($"{name}-{strDate}.log");
    }

    [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
        Justification = "It's disposed on this class Dispose.")]
    private StreamWriter CreateOrGetStream(string filename)
    {
        try
        {


            // Opening the stream if needed
            if (!_streams.ContainsKey(filename))
            {
                // Building stream's filepath


                var filepath = Path.Combine(_directory, filename);

                // Making sure the directory exists
                Directory.CreateDirectory(_directory);

                // Opening the stream
                var stream = new StreamWriter(
                    File.Open(filepath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)
                );
                stream.AutoFlush = true;

                // Storing the created stream
                _streams[filename] = stream;
            }

            return _streams[filename];
        }
        catch
        {
            //در اینجا از خطا جلوگیری می کنیم که باعث خطا در روند اپلیکیشن و ایجاد لوپ نشود
            return null;
        }
    }



    internal string[] Filepaths() =>
      _streams.Values.Select(s => s.BaseStream).Cast<FileStream>().Select(s => s.Name).ToArray();

}