using System;
using System.Net.Mime;
using System.Web.Mvc;

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
			var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
			htmlToPdf.TocHeaderText = "Table of content";
			htmlToPdf.GenerateToc = true;
			var pdfBytes = htmlToPdf.GeneratePdf(GetSamplePdfString(20));
			return File(pdfBytes, "application/pdf", "NRecoTestFile.pdf");
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