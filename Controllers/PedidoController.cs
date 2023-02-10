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
            if (cid.HasValue)
            {
                ClienteModel cliente = await _databaseContext.Clientes.FindAsync(cid.Value);

                if (cliente != null)
                {
                    IEnumerable<PedidoModel> pedidos = _databaseContext.Pedidos
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

        [HttpGet]
        public async Task<IActionResult> Excluir(int? pid)
        {
            if (!pid.HasValue)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Pedido não informado.", TipoMensagem.Erro);
                return RedirectToAction(nameof(Index));
            }

            if (!await PedidoExiste(pid.Value))
            {
                TempData["mensagem"] = MensagemModel.Serializar("Pedido não encontrado.", TipoMensagem.Erro);
                return RedirectToAction(nameof(Index));
            }

            PedidoModel pedido = await _databaseContext.Pedidos
                .Include(c => c.Cliente)
                .Include(i => i.ItensPedido)
                .ThenInclude(p => p.Produto)
                .FirstOrDefaultAsync(p => p.IdPedido == pid.Value);

            return View(pedido);
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(PedidoModel pedido)
        {
            PedidoModel pedidoModel = await _databaseContext.Pedidos.FindAsync(pedido.IdPedido);

            if (pedidoModel == null)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Pedido não encontrada.", tipo: TipoMensagem.Erro);
                return RedirectToAction(nameof(Index));
            }

            _databaseContext.Remove(pedidoModel);

            if (await _databaseContext.SaveChangesAsync() > 0)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Pedido excluida com sucesso!");
                return RedirectToAction(nameof(Index), new { cid = pedido.IdCliente });
            }

            TempData["mensagem"] = MensagemModel.Serializar("Erro ao excluir pedido.", tipo: TipoMensagem.Erro);
            return RedirectToAction(nameof(Index), "Cliente");
        }

        [HttpGet]
        public async Task<IActionResult> Fechar(int? pid)
        {
            if (!pid.HasValue)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Pedido não informado.", TipoMensagem.Erro);
                return RedirectToAction(nameof(Index));
            }

            if (!await PedidoExiste(pid.Value))
            {
                TempData["mensagem"] = MensagemModel.Serializar("Pedido não encontrado.", TipoMensagem.Erro);
                return RedirectToAction(nameof(Index));
            }

            PedidoModel pedido = await _databaseContext.Pedidos
                .Include(c => c.Cliente)
                .Include(i => i.ItensPedido)
                .ThenInclude(p => p.Produto)
                .FirstOrDefaultAsync(p => p.IdPedido == pid.Value);

            return View(pedido);
        }

        [HttpPost]
        public async Task<IActionResult> Fechar(int idPedido)
        {
            PedidoModel pedido = await _databaseContext.Pedidos
                .Include(c => c.Cliente)
                .Include(i => i.ItensPedido)
                .ThenInclude(p => p.Produto)
                .FirstOrDefaultAsync(p => p.IdPedido == idPedido);

            if (pedido == null)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Pedido não encontrada.", tipo: TipoMensagem.Erro);
                return RedirectToAction(nameof(Index));
            }

            if (pedido.ItensPedido.Count() > 0)
            {
                pedido.DataPedido = DateTime.Now;

                foreach (var item in pedido.ItensPedido)
                    item.Produto.Quantidade = -item.Quantidade;

                TempData["mensagem"] = await _databaseContext.SaveChangesAsync() > 0 ?
                    MensagemModel.Serializar("Pedido fechado com sucesso!") :
                    MensagemModel.Serializar("Erro ao fechar pedido.", TipoMensagem.Erro);

                return RedirectToAction(nameof(Index), new { cid = pedido.IdCliente });
            }

            TempData["mensagem"] = MensagemModel.Serializar("Erro ao fechar pedido.", tipo: TipoMensagem.Erro);
            return RedirectToAction(nameof(Index), "Cliente");
        }

        public async Task<IActionResult> Entregar(int? pid)
        {
            if (!pid.HasValue)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Pedido não informado.", TipoMensagem.Erro);
                return RedirectToAction(nameof(Index));
            }

            if (!await PedidoExiste(pid.Value))
            {
                TempData["mensagem"] = MensagemModel.Serializar("Pedido não encontrado.", TipoMensagem.Erro);
                return RedirectToAction(nameof(Index));
            }

            PedidoModel pedido = await _databaseContext.Pedidos
                .Include(c => c.Cliente)
                .ThenInclude(e => e.Enderecos)
                .Include(i => i.ItensPedido)
                .ThenInclude(p => p.Produto)
                .FirstOrDefaultAsync(p => p.IdPedido == pid.Value);

            return View(pedido);
        }

        [HttpPost]
        public async Task<IActionResult> Entregar(int idPedido, int idEndereco)
        {
            PedidoModel pedido = await _databaseContext.Pedidos
                .Include(c => c.Cliente)
                .ThenInclude(e => e.Enderecos)
                .FirstOrDefaultAsync(p => p.IdPedido == idPedido);

            EnderecoModel endereco = pedido.Cliente.Enderecos.FirstOrDefault(e => e.IdEndereco == idEndereco);

            if (pedido == null)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Pedido não encontrada.", tipo: TipoMensagem.Erro);
                return RedirectToAction(nameof(Index), new { cid = pedido.Cliente.IdUsuario });
            }

            if (endereco == null)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Endereço não encontrada.", tipo: TipoMensagem.Erro);
                return RedirectToAction(nameof(Index), new { cid = pedido.Cliente.IdUsuario });
            }

            pedido.DataPedido = DateTime.Now;
            pedido.DataEntrega = DateTime.Now;
            pedido.EnderecoEntrega = endereco;

            _databaseContext.Pedidos.Update(pedido);

            TempData["mensagem"] = await _databaseContext.SaveChangesAsync() > 0 ?
                MensagemModel.Serializar("Entrega registrada com sucesso!") :
                MensagemModel.Serializar("Erro ao registrar entrega do pedido.", TipoMensagem.Erro);

            return RedirectToAction(nameof(Index), new { cid = pedido.IdCliente });
        }

        #region Metodos Privados
        private async Task<bool> PedidoExiste(int idPedido)
        {
            PedidoModel pedido = await _databaseContext.Pedidos.FindAsync(idPedido);
            return !(pedido == null);
        }

        private async Task<bool> AtualizaValorPedido(ItemPedidoModel itemPedido)
        {
            PedidoModel pedido = await _databaseContext.Pedidos.FindAsync(itemPedido.IdPedido);
            pedido.ValorPedido = _databaseContext.ItensPedido
                        .Where(i => i.IdPedido == itemPedido.IdPedido)
                        .Sum(x => x.ValorUnitario * x.Quantidade);

            _databaseContext.Update(pedido);

            return await _databaseContext.SaveChangesAsync() > 0;
        }

        #endregion
    }
}