using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUDAvancado.Models;
using CrudAvancado.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudAvancado.Controllers
{
    public class PedidoController : Controller
    {
        private readonly DatabaseContext _databaseContext;

        public PedidoController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IActionResult> Index(int? cid)
        {
            ViewBag.Subtitulo = "Listagem de Pedidos";

            if (cid.HasValue)
            {
                ClienteModel cliente = await _databaseContext.Clientes.FindAsync(cid.Value);

                if (cliente != null)
                {
                    var pedidos = _databaseContext.Pedidos
                        .Where(p => p.IdCliente == cid.Value)
                        .OrderByDescending(x => x.Cliente)
                        .AsNoTracking().AsEnumerable();

                    ViewBag.Cliente = cliente;
                    return View(pedidos);
                }
                else
                {
                    TempData["menagem"] = MensagemModel.Serializar("Cliente não encontrado.", TipoMensagem.Erro);
                }
            }
            else
            {
                TempData["menagem"] = MensagemModel.Serializar("Cliente não informado.", TipoMensagem.Erro);
            }

            return RedirectToAction(nameof(Index), "Cliente");

        }

        [HttpGet]
        public async Task<IActionResult> Cadastrar(int? cid)
        {
            ViewBag.Subtitulo = "Listagem de Pedidos";

            if (cid.HasValue)
            {
                ClienteModel cliente = await _databaseContext.Clientes.FindAsync(cid.Value);

                if (cliente != null)
                {
                    ViewBag.Cliente = cliente;

                    _databaseContext.Entry(cliente).Collection(p => p.Pedidos).Load();
                    PedidoModel pedido = null;

                    if (_databaseContext.Pedidos.Any(p => p.IdCliente == cid && !p.DataPedido.HasValue))
                    {
                        pedido = await _databaseContext.Pedidos.FirstOrDefaultAsync(p => p.IdCliente == cid && !p.DataPedido.HasValue);
                    }
                    else
                    {
                        pedido = new PedidoModel { IdCliente = cid.Value, ValorPedido = 0 };
                        cliente.Pedidos.Add(pedido);
                        await _databaseContext.SaveChangesAsync();
                    }

                    return RedirectToAction(nameof(Index), "ItemPedido", new { pid = pedido.IdPedido });
                }
                TempData["menagem"] = MensagemModel.Serializar("Cliente não encontrado.", TipoMensagem.Erro);
                return RedirectToAction(nameof(Index), "Cliente");
            }
                TempData["menagem"] = MensagemModel.Serializar("Cliente não informado.", TipoMensagem.Erro);
                return RedirectToAction(nameof(Index), "Cliente");
        }

        //     [HttpPost]
        //     public async Task<IActionResult> Excluir(PedidoModel pedido)
        //     {
        //         PedidoModel pedidoModel = await _databaseContext.Pedidos.FindAsync(pedido.IdPedido);

        //         if (pedidoModel == null)
        //         {
        //             TempData["mensagem"] = MensagemModel.Serializar("Pedido não encontrada.", tipo: TipoMensagem.Erro);
        //             return RedirectToAction(nameof(Index));
        //         }
        //         else
        //         {
        //             _databaseContext.Remove(pedidoModel);
        //         }

        //         TempData["mensagem"] = (await _databaseContext.SaveChangesAsync() > 0) ?
        //             MensagemModel.Serializar("Pedido excluida com sucesso!") :
        //         MensagemModel.Serializar("Erro ao excluir pedido.", tipo: TipoMensagem.Erro);

        //         return RedirectToAction("Index");
        //     }
    }
}