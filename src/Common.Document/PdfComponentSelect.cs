using System;

namespace Common.Document
{
    public class PdfComponentSelect : IDisposable
    {

        private SelectPdf.HtmlToPdf htmlToPdfConverter;
        private string token;
        public PdfComponentSelect(string token)
            : this()
        {
            this.token = token;
        }

        public PdfComponentSelect()
        {
            SelectPdf.GlobalProperties.LicenseKey = "gaqwobO0sKG0t6GwtK+xobKwr7Czr7i4uLg=";
            this.htmlToPdfConverter = new SelectPdf.HtmlToPdf();
        }

        public SelectPdf.PdfDocument GetInstance()
        {
            return new SelectPdf.PdfDocument();
        }

        public byte[] CreatePdfBytesFromUrl(string url, PdfConfiguration configuracao)
        {
            this.configOrientation(configuracao);

            var doc = htmlToPdfConverter.ConvertUrl(url);
            var bytes = doc.Save();
            doc.Close();
            return bytes;
        }

        private void configOrientation(PdfConfiguration configuracao)
        {
            if (configuracao.landscape)
            {
                this.htmlToPdfConverter.Options.PdfPageOrientation = SelectPdf.PdfPageOrientation.Landscape;

                this.htmlToPdfConverter.Options.MarginTop = 10;
                this.htmlToPdfConverter.Options.MarginRight = 05;
                this.htmlToPdfConverter.Options.MarginBottom = 10;
                this.htmlToPdfConverter.Options.MarginLeft = 05;


                this.htmlToPdfConverter.Options.WebPageFixedSize = false;
                this.htmlToPdfConverter.Options.WebPageWidth = 1100;
                this.htmlToPdfConverter.Options.WebPageHeight= 0;


                this.htmlToPdfConverter.Options.AutoFitWidth = SelectPdf.HtmlToPdfPageFitMode.NoAdjustment;
                this.htmlToPdfConverter.Options.AutoFitHeight = SelectPdf.HtmlToPdfPageFitMode.NoAdjustment;

            }
            else
            {
                this.htmlToPdfConverter.Options.PdfPageOrientation = SelectPdf.PdfPageOrientation.Portrait;

                this.htmlToPdfConverter.Options.MarginTop = 20;
                this.htmlToPdfConverter.Options.MarginRight = 20;
                this.htmlToPdfConverter.Options.MarginBottom = 20;
                this.htmlToPdfConverter.Options.MarginLeft = 20;
            }

            if (configuracao.useWebScale.HasValue && configuracao.height.HasValue && configuracao.width.HasValue)
            {
                this.htmlToPdfConverter.Options.PdfPageSize = SelectPdf.PdfPageSize.A4;
                this.htmlToPdfConverter.Options.WebPageFixedSize = false;
                this.htmlToPdfConverter.Options.WebPageWidth = configuracao.width.Value;
                this.htmlToPdfConverter.Options.WebPageHeight = configuracao.height.Value;

                if (configuracao.boletoCarne.HasValue && configuracao.boletoCarne.Value)
                {
                    this.htmlToPdfConverter.Options.MarginTop = 1;
                    this.htmlToPdfConverter.Options.MarginRight = 5;
                    this.htmlToPdfConverter.Options.MarginBottom = 1;
                    this.htmlToPdfConverter.Options.MarginLeft = 5;
                    this.htmlToPdfConverter.Options.AutoFitWidth = SelectPdf.HtmlToPdfPageFitMode.AutoFit;
                    this.htmlToPdfConverter.Options.AutoFitHeight = SelectPdf.HtmlToPdfPageFitMode.NoAdjustment;
                }
                else
                {                    
                    this.htmlToPdfConverter.Options.MarginLeft = 50;
                    this.htmlToPdfConverter.Options.MarginTop = 60;
                    this.htmlToPdfConverter.Options.AutoFitWidth = SelectPdf.HtmlToPdfPageFitMode.NoAdjustment;
                    this.htmlToPdfConverter.Options.AutoFitHeight = SelectPdf.HtmlToPdfPageFitMode.NoAdjustment;
                }
            }
        }

        public byte[] CreatePdfBytesFromContent(string html, PdfConfiguration configuracao)
        {
            this.configOrientation(configuracao);

            var doc = this.htmlToPdfConverter.ConvertHtmlString(html);
            var bytes = doc.Save();
            doc.Close();
            return bytes;
        }

        public void SecurityAddCookie(string name, string value)
        {
            htmlToPdfConverter.Options.HttpCookies.Add(name, value);
        }

        public void Dispose()
        {

        }
    }
}
