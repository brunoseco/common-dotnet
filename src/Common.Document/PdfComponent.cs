using System;

namespace Common.Document
{
    public class PdfComponent : IDisposable
    {

        private PdfComponentSelect component;

        public PdfComponent()
        {
            this.component = new PdfComponentSelect();
        }

        public SelectPdf.PdfDocument GetInstance()
        {
            return this.component.GetInstance();
        }

        public byte[] CreatePdfBytesFromUrl(string url, PdfConfiguration configuracao)
        {
            return this.component.CreatePdfBytesFromUrl(url, configuracao);
        }

        public byte[] CreatePdfBytesFromContent(string html, PdfConfiguration configuracao)
        {
            return this.component.CreatePdfBytesFromContent(html, configuracao);
        }

        public void SecurityAddCookie(string name, string value)
        {
            this.component.SecurityAddCookie(name, value);
        }

        public void Dispose()
        {
            this.component.Dispose();
        }
    }
}
