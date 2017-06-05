using System;
using System.Net.Mime;
using System.Web.Mvc;
using NReco.PdfGenerator;

namespace Nreco.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		public ActionResult Print()
		{
			return View();
		}

		public FileContentResult GetPdf()
		{
			var nreco = new NReco.PdfGenerator.HtmlToPdfConverter();

			//margins
			nreco.Margins = new PageMargins { Top = 30, Bottom = 20, Left = 30, Right = 10 };

			//Table of content
			nreco.TocHeaderText = "Table of content";
			nreco.GenerateToc = true;

			//Header & Footer, page number
			nreco.PageHeaderHtml = GetPageHeader();
			nreco.PageFooterHtml = GetPageFooter();
			
			//Cover Page without header&footer
			var coverPage = GetCoverPage();

			var content = GetSamplePdfString(20);
			var pdfBytes = nreco.GeneratePdf(content, coverPage);

//			System.IO.File.WriteAllText(@"c:\NReco.POC.cover.html", coverPage);
//			System.IO.File.WriteAllText(@"c:\NReco.POC.content.html", content);

			return File(pdfBytes, "application/pdf", "NRecoTestFile.pdf");
		}

		private string GetCoverPage()
		{
			return $@"<div style='height: 300px;'></div><div style='font-size: 30px'><b>Portfolio PDF</b><br/><br/>Cover page with no header and footer</div>";
		}

		private string GetPageHeader()
		{
			return $@"<div style='text-align: right; margin-top: 10px; margin-bottom: 20px;'>
						<div><img alt='7Star' src='data:image/png;base64,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'></div>
						<div>Portfolio Management Tool: Get your projects done!</div>
					</div>";
		}

		private string GetPageFooter()
		{
			return $@"<div style='text-align: center'>© 7star 2017</div><div>page <span class=""page""></span> of <span class=""topage""></span></div>";
		}

		private string GetSamplePdfString(int numberOfPages = 10)
		{
			var result = GetSampleHtmlSchema();
			var rows = "";
			for (var i = 0; i < numberOfPages * 20; i++)
				rows += GetSampleRow(i);
			return result.Replace("{row}", rows);
		}

		private string GetSampleHtmlSchema()
		{
			return @"<html>
<head>
	<meta http-equiv='content-type' content='text/html; charset=utf-8' />
	<style>
		.7Star { background-image: url(data:image/png;base64,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); }
		table { border:1px solid silver; border-collapse:collapse;} table td { border-bottom:1px solid silver; }
		table, tr, td, th, tbody, thead, tfoot { page-break-inside: avoid !important; }
	</style>
</head>
	<body>
		<h1 style='text-align:center;'>Order #23</h1>
		<div style='float:left;'>
			Donald<br />
			47338 Park Avenue<br />
			Big City<br /><br />
			Date: 6/3/2017
		</div>
		<div style='float:right;'>
			<div style='border:1px solid gray;padding:10px;'>
				A company<br />
				321 City Street<br />
				Industry Park<br />
				Another Country
			</div>
		</div>
		<div style='clear:both;'></div>
		<br /><br />
		Ordered items list:
		<table width='100%' style='margin-bottom:20px;'>
			{row}
		</table>


		<div style='float:right;'>
			Signed: Saturday, June 3, 2017 12:06 PM<br />
			From IP: 31.134.60.75
			Signature:<br />
			<img src='https://www.nrecosite.com/img/pdfgenerator/donald_sign.jpg'/>
		</div>
		<div style='float:left;'>Sincerely,<br />Donald</div>
		<div style='clear:both;'></div>

	</body>
</html>";
		}

		private string GetSampleRow(int rowNumber)
		{
			var result = "";
			if (rowNumber % 15 == 0)
				result += $"</table><div style='page-break-after:always'></div><h1>Header number {rowNumber/15:0000000}</h1><table>";
			result += $@"<tr>
				<td>{rowNumber:0000000}</td>
				<td><b>Sample name</b></td>
				<td><i>Lorem ipsum dolor sit amet enim. Etiam ullamcorper. Suspendisse a pellentesque dui, Lorem ipsum dolor sit amet enim. Etiam ullamcorper. Suspendisse a pellentesque dui, Lorem ipsum dolor sit amet enim. Etiam ullamcorper. Suspendisse a pellentesque dui</i></td>
			</tr>";
			return result;
		}
	}
}