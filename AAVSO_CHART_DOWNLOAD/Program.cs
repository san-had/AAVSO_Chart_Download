namespace AAVSO_CHART_DOWNLOAD
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;

    internal class Program
    {
        private static void Main(string[] args)
        {
            Download();
        }

        private static void Download()
        {
            string host = @"https://www.aavso.org";
            string fileName = @"C:\AAVSO\test.png";
            string url = @"https://www.aavso.org/apps/vsp/chart/";
            string parametersString = @"?fov=900.0&scale=A&star=R+GEM&orientation=visual&maglimit=9.0&resolution=150&north=up&east=left&type=chart";
            string fullUrl = url + parametersString;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            try
            {
                using (var webClient = new WebClient())
                {
                    using (Stream output = File.OpenWrite(fileName))
                    {
                        string tempResult = webClient.DownloadString(fullUrl);

                        var downLoadLink = GetChartDownloadLink(host, tempResult);

                        byte[] receivedBytes = webClient.DownloadData(downLoadLink);

                        output.Write(receivedBytes, 0, receivedBytes.Length);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string GetChartDownloadLink(string host, string pageContent)
        {
            string chartDownloadLink = host;

            var lines = pageContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            var line = lines.Where(row => row.Contains("/apps/vsp/chart/")).FirstOrDefault();

            chartDownloadLink += line.Split(new[] { '"' })[1];

            return chartDownloadLink;
        }
    }
}