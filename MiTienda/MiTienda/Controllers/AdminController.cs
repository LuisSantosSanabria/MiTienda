using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiTienda.Services;

namespace MiTienda.Controllers
{
    [Authorize(Roles = "Adm")]
    public class AdminController : Controller
    {
        private readonly PedidoService _pedidoService;

        public AdminController(PedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "Panel Administrativo";
            return View();
        }

        // para que el admin vea el historial de pedidos
        public async Task<IActionResult> HistorialPedidos()
        {
            var pedidos = await _pedidoService.GetAllAsync();
            return View(pedidos);
        }

        public async Task<IActionResult> GenerarPDF()
        {
            var pedidos = await _pedidoService.GetAllAsync();

            using (var ms = new MemoryStream())
            {
                var document = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter.GetInstance(document, ms);

                document.Open();

                // Título
                var titulo = new Paragraph("Historial de Pedidos\n\n",
                    FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20));
                titulo.Alignment = Element.ALIGN_CENTER;
                document.Add(titulo);

                document.Add(new Paragraph("Fecha de generación: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "\n\n"));

                foreach (var pedido in pedidos)
                {
                    // Encabezado del pedido
                    var header = new Paragraph(
                        $"Usuario: {pedido.UsuarioEmail}   |   Fecha: {pedido.PedidoDatos}   |   Total: {pedido.TotalPedido}",
                        FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)
                    );
                    document.Add(header);
                    document.Add(new Paragraph("Artículos:", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11)));

                    // Listado de artículos
                    foreach (var art in pedido.Articulos)
                    {
                        document.Add(new Paragraph(
                            $"- {art.NombreProducto} — {art.Cantidad} x {art.Precio}",
                            FontFactory.GetFont(FontFactory.HELVETICA, 10)
                        ));
                    }

                    document.Add(new Paragraph("\n------------------------------------------------------------\n"));
                }

                document.Close();

                return File(ms.ToArray(), "application/pdf", "HistorialPedidos.pdf");
            }
        }
    }
}

