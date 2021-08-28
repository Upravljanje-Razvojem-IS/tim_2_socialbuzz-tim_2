using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.Helper
{
    public static class FormatHelper
    {
        public static string ListIntToCsv(List<int> list)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < list.Count; i++)
            {
                result.Append(list[i]);

                if (i != list.Count - 1)
                {
                    result.Append(',');
                }
            }

            return result.ToString();
        }

        public static List<int> CsvToListInt(string csv)
        {
            List<int> result = new List<int>();

            if (string.IsNullOrEmpty(csv))
            {
                return result;
            }

            string[] ids = csv.Split(',');

            foreach (string id in ids)
            {
                result.Add(Convert.ToInt32(id));
            }

            return result;
        }
    }
}
