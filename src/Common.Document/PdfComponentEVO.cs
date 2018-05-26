using EvoPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Document
{
    class PdfComponentEVO : IDisposable
    {

        private HtmlToPdfConverter htmlToPdfConverter;

        public PdfComponentEVO()
        {
            this.htmlToPdfConverter = new HtmlToPdfConverter();
        }

        public byte[] CreatePdfBytesFromUrl(string url, string strLandscape, string htmlStringWithPageNumbers)
        {
            bool booLandscape = ((string.IsNullOrEmpty(strLandscape) || strLandscape == "0") ? false : true);

            if (!string.IsNullOrEmpty(url))
            {

                if (booLandscape)
                    this.htmlToPdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Landscape;
                else
                    this.htmlToPdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;

                var HeaderHtmlWithPageNumbers = new HtmlToPdfVariableElement(htmlStringWithPageNumbers, string.Empty);
                this.htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;
                this.htmlToPdfConverter.PdfHeaderOptions.HeaderHeight = 0;
                this.htmlToPdfConverter.PdfHeaderOptions.AddElement(HeaderHtmlWithPageNumbers);

                var outPdfBuffer = htmlToPdfConverter.ConvertUrl(url);
                return outPdfBuffer;
            }

            throw new InvalidOperationException("url não especificada");
        }

        public byte[] CreatePdfBytesFromContent(string html, string htmlStringWithPageNumbers)
        {

            return CreatePdfBytesFromContent(html, string.Empty, htmlStringWithPageNumbers);
        }
        public byte[] CreatePdfBytesFromContent(string html, string strLandscape, string htmlStringWithPageNumbers)
        {
            bool booLandscape = ((string.IsNullOrEmpty(strLandscape) || strLandscape == "0") ? false : true);

            if (!string.IsNullOrEmpty(html))
            {

                if (booLandscape)
                    this.htmlToPdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Landscape;
                else
                    this.htmlToPdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;

                var HeaderHtmlWithPageNumbers = new HtmlToPdfVariableElement(htmlStringWithPageNumbers, string.Empty);
                this.htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;
                this.htmlToPdfConverter.PdfHeaderOptions.HeaderHeight = 0;
                this.htmlToPdfConverter.PdfHeaderOptions.AddElement(HeaderHtmlWithPageNumbers);

                var memoryStream = new MemoryStream();
                htmlToPdfConverter.ConvertHtmlToStream(html, "", memoryStream);


                var biteArray = new byte[memoryStream.Length];
                memoryStream.Position = 0;
                memoryStream.Read(biteArray, 0, (int)memoryStream.Length);

                return biteArray;
            }

            throw new InvalidOperationException("html não especificada");
        }
        public void SecurityAddCookie(string name, string value)
        {
            this.htmlToPdfConverter.HttpRequestCookies.Add(name, value);
        }


        public void Dispose()
        {
            if (this.htmlToPdfConverter != null)
                this.htmlToPdfConverter = null; ;
        }
    }
}
