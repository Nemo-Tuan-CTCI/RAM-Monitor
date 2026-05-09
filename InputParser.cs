using System;
using System.Globalization;

public static class InputParser
{
    public static int ParseDuration(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("請輸入監控時長。");

        // 支援 mm:ss 或純秒數
        if (input.Contains(":"))
        {
            var parts = input.Split(':');
            if (parts.Length != 2)
                throw new FormatException("時長格式錯誤，請輸入 mm:ss 或秒數。");
            int min = int.Parse(parts[0]);
            int sec = int.Parse(parts[1]);
            return min * 60 + sec;
        }
        else
        {
            return int.Parse(input);
        }
    }

    public static int ParseInterval(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return 1;
        return int.Parse(input);
    }
}