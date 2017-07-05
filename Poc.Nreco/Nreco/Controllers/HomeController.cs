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
			var pdfBytes = new PrintEngine.PdfEngine().GeneratePdf(170);
			return File(pdfBytes, "application/pdf", "NRecoTestFile.pdf");
		}

	}
}