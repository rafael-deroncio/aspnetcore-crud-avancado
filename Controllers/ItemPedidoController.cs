using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUDAvancado.Models;
using CrudAvancado.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrudAvancado.Controllers
{
    public class ItemPedidoController : Controller
    {
        private readonly DatabaseContext _databaseContext;

        public ItemPedidoController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IActionResult> Index(int? pid)
        {
            if (pid.HasValue)
            {
                if (_databaseContext.Pedidos.Any(p => p.IdPedido == pid.Value))
                {
                    PedidoModel pedido = await _databaseContext.Pedidos
                        .Include(p => p.Cliente)
                        .Include(p => p.ItensPedido.OrderBy(i => i.Produto.Nome))
                        .ThenInclude(i => i.Produto)
                        .ThenInclude(c => c.Categoria)
                        .FirstOrDefaultAsync(p => p.IdPedido == pid.Value);

                    ViewBag.Pedido = pedido;
                    return View(pedido.ItensPedido);
                }
                TempData["menagem"] = MensagemModel.Serializar("Pedido não encontrado", TipoMensagem.Erro);
                return RedirectToAction(nameof(Index), "Cliente");
            }
            TempData["menagem"] = MensagemModel.Serializar("Pedido não informado", TipoMensagem.Erro);
            return RedirectToAction(nameof(Index), "Cliente");
        }

        [HttpGet]
        public async Task<IActionResult> Cadastrar(int? pid, int? prod)
        {
            if (pid.HasValue)
            {
                if (_databaseContext.Pedidos.Any(p => p.IdPedido == pid.Value))
                {
                    ViewBag.Produtos = new SelectList(await _databaseContext.Produtos
                        .OrderBy(p => p.Nome)
                        .Select(p => new { p.IdProduto, NomePreco = $"{p.Nome} ({p.Valor.ToString("")})" })
                        .AsNoTracking().ToListAsync(),
                        "IdProduto", "NomePreco");

                    if (prod.HasValue && await ItemPedidoExiste(pid.Value, prod.Value))
                    {
                        ItemPedidoModel itemPedido = await _databaseContext.ItensPedido
                            .Include(i => i.Produto)
                            .FirstOrDefaultAsync(i => i.IdPedido == pid && i.IdProduto == prod);

                        return View(itemPedido);
                    }
                    else
                    {
                        return View(new ItemPedidoModel()
                        { IdPedido = pid.Value, ValorUnitario = 0, Quantidade = 1 });
                    }
                }
                TempData["menagem"] = MensagemModel.Serializar("Pedido não encontrado.", TipoMensagem.Erro);
                return RedirectToAction(nameof(Index), "Cliente");
            }
            TempData["menagem"] = MensagemModel.Serializar("Pedido não informado.", TipoMensagem.Erro);
            return RedirectToAction(nameof(Index), "Cliente");
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromForm] ItemPedidoModel itemPedido)
        {
            if (ModelState.IsValid)
            {
                if (itemPedido.IdPedido > 0)
                {
                    ProdutoModel produto = await _databaseContext.Produtos.FindAsync(itemPedido.IdProduto);
                    itemPedido.ValorUnitario = produto.Valor;

                    if (await ItemPedidoExiste(itemPedido.IdPedido, itemPedido.IdProduto))
                    {
                        _databaseContext.ItensPedido.Update(itemPedido);

                        TempData["mensagem"] = (await _databaseContext.SaveChangesAsync() > 0) ?
                            MensagemModel.Serializar("Item do pedido alterado com sucesso!") :
                            MensagemModel.Serializar("Erro ao alterar item do pedido.", TipoMensagem.Erro);
                    }
                    else
                    {
                        _databaseContext.ItensPedido.Add(itemPedido);

                        TempData["mensagem"] = (await _databaseContext.SaveChangesAsync() > 0) ?
                            MensagemModel.Serializar("Item do pedido cadastrado com sucesso!") :
                            MensagemModel.Serializar("Erro ao cadastrar item do pedido.", TipoMensagem.Erro);
                    }

                    PedidoModel pedido = await _databaseContext.Pedidos.FindAsync(itemPedido.IdPedido);
                    pedido.ValorPedido = _databaseContext.ItensPedido
                        .Where(i => i.IdPedido == itemPedido.IdPedido)
                        .Sum(x => x.ValorUnitario * x.Quantidade);

                    _databaseContext.Pedidos.Update(pedido);
                    await _databaseContext.SaveChangesAsync();

                    return RedirectToAction(nameof(Index), "ItemPedido", new { pid = itemPedido.IdPedido });
                }
                else
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Pedido não informado.", TipoMensagem.Erro);
                    return RedirectToAction(nameof(Index), "Cliente");
                }
            }
            TempData["mensagem"] = MensagemModel.Serializar("Item do pedido salvo com sucesso!");

            ViewBag.Produtos = new SelectList(await _databaseContext.Produtos
                .OrderBy(p => p.Nome)
                .Select(p => new { p.IdProduto, NomePreco = $"{p.Nome} ({p.Valor.ToString("")})" })
                .AsNoTracking().ToListAsync(),
                "IdProduto", "NomePreco");

            return View(itemPedido);
        }

        [HttpGet]
        public async Task<IActionResult> Excluir(int? pid, int? prod)
        {
            if (!pid.HasValue || !prod.HasValue)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Item de pedido nao informado.", TipoMensagem.Erro);
                return RedirectToAction(nameof(Index), "Cliente");
            }

            if (!await ItemPedidoExiste(pid: pid.Value, prod: prod.Value))
            {
                TempData["mensagem"] = MensagemModel.Serializar("Item de pedido nao localizado.", TipoMensagem.Erro);
                return RedirectToAction(nameof(Index), "Cliente");
            }

            ItemPedidoModel itemPedido = await _databaseContext.ItensPedido.FindAsync(pid.Value, prod.Value);

            _databaseContext.Entry(itemPedido).Reference(p => p.Produto).Load();

            return View(itemPedido);
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(int pid, int prod)
        {
            ItemPedidoModel itemPedido = await _databaseContext.ItensPedido.FindAsync(pid, prod);

            if (itemPedido != null)
            {
                _databaseContext.ItensPedido.Remove(itemPedido);

                if (await _databaseContext.SaveChangesAsync() > 0)
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Item de pedido excluído com sucesso!");

                    await AtualizaValorPedido(itemPedido);

                }
                else
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Erro ao excluir item do pedido.", tipo: TipoMensagem.Erro);
                }
            }
            else
            {
                TempData["mensagem"] = MensagemModel.Serializar("Erro ao excluir item do pedido.", tipo: TipoMensagem.Erro);
            }

            return RedirectToAction(nameof(Index), new { pid = pid });
        }

        #region Métodos Privados
        private async Task<bool> ItemPedidoExiste(int pid, int prod)
        {
            return await _databaseContext.ItensPedido.AnyAsync(i => i.IdPedido == pid && i.IdProduto == prod);
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