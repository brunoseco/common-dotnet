using System;
using System.IO;
using System.Net;
using iTextSharp.text.pdf;

namespace Common.Document
{
    public class PdfComponentItextSharp : IDisposable
    {

        private iTextSharp.text.Document htmlToPdfConverter;
        private string token;
        public PdfComponentItextSharp(string token)
        {
            this.token = token;
        }
        public PdfComponentItextSharp()
        {
            this.htmlToPdfConverter = new iTextSharp.text.Document();
        }
        public byte[] CreatePdfBytesFromUrl(string url)
        {
            return CreatePdfBytesFromUrl(url, string.Empty, string.Empty);
        }
        public byte[] CreatePdfBytesFromUrl(string url, string strLandscape, string htmlStringWithPageNumbers)
        {
            var html = GetHtmlFromUrl(url);
            return CreatePdfBytesFromContent(html, strLandscape, htmlStringWithPageNumbers);
        }



        public byte[] CreatePdfBytesFromContent(string html, string strLandscape, string htmlStringWithPageNumbers)
        {
            var pathTemp = CreatePdfFromContent(html, strLandscape, htmlStringWithPageNumbers);
            var bytes = File.ReadAllBytes(pathTemp);
            File.Delete(pathTemp);
            return bytes;
        }

        public string CreatePdfFromContent(string html, string strLandscape, string htmlStringWithPageNumbers)
        {
            var pathTemp = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, String.Format("temp-{0}-{1}.pdf", this.token, DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")));
            PdfWriter.GetInstance(this.htmlToPdfConverter, new FileStream(pathTemp, FileMode.Create));
            this.htmlToPdfConverter.Open();

            //var pdfImage = Image.GetInstance("");
            //pdfImage.ScaleToFit(100, 50);
            //pdfImage.Alignment = iTextSharp.text.Image.UNDERLYING; pdfImage.SetAbsolutePosition(180, 760);
            //this.htmlToPdfConverter.Add(pdfImage);

            var styles = new iTextSharp.text.html.simpleparser.StyleSheet();
            var hw = new iTextSharp.text.html.simpleparser.HTMLWorker(this.htmlToPdfConverter);
            hw.Parse(new StringReader(html));
            this.htmlToPdfConverter.Close();
            return pathTemp;
        }
        public byte[] CreatePdfBytesFromContent(string html, string htmlStringWithPageNumbers)
        {
            return CreatePdfBytesFromContent(html, "", htmlStringWithPageNumbers);
        }

        public void SecurityAddCookie(string name, string value)
        {

        }

        private string GetHtmlFromUrl(string url)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            var html = "";
            if (httpWebResponse.StatusCode == HttpStatusCode.OK)
            {
                using (Stream responseStream = httpWebResponse.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                        html = reader.ReadToEnd();
                }
            }
            httpWebResponse.Close();
            return html;
        }


        public void Dispose()
        {
            htmlToPdfConverter.Dispose();
        }
    }
}
