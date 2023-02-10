using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudAvancado.Contexts;
using CRUDAvancado.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CrudAvancado
{
    public class HomeController : Controller
    {

        private readonly DatabaseContext _databaseContext;

        public HomeController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Subtitulo = "Sistema de Pedidos";
            
            List<PedidoModel> pedidos = await _databaseContext.Pedidos
                .Where(p => !p.DataPedido.HasValue)
                .Include(p => p.Cliente)
                .OrderByDescending(p => p.IdPedido)
                .AsNoTracking().ToListAsync();

            return View(pedidos);
        }
    }
}