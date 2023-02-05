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
    public class ClienteController : Controller
    {
        private readonly DatabaseContext _databaseContext;

        public ClienteController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Subtitulo = "Listagem de Clientes";

            List<ClienteModel> clientes = await _databaseContext.Clientes
                .OrderBy(x => x.IdUsuario)
                .AsNoTracking()
                .ToListAsync();

            return View(clientes);
        }

        [HttpGet]
        public async Task<IActionResult> Cadastrar(int? id)
        {
            if (!id.HasValue)
                return View(new ClienteModel());

            ViewBag.Subtitulo = "Listagem de Clientes";
            ClienteModel cliente = await _databaseContext.Clientes.FindAsync(id);

            if (cliente != null)
                return View(cliente);

            TempData["mensagem"] = MensagemModel.Serializar("Cliente não encontrado.", TipoMensagem.Erro);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(int? id, [FromForm] ClienteModel cliente)
        {
            // Modelo Nao Preenchido
            if (!ModelState.IsValid)
                return View(cliente);

            // Modelo Preenchido
            if (id.HasValue && id.Value != 0) // ID in route? Edit
            {
                ClienteModel clienteModel = _databaseContext.Clientes.Where(x => x.IdUsuario == id.Value).First();
                if (clienteModel != null)
                {
                    clienteModel.Nome = cliente.Nome;
                    clienteModel.CPF = cliente.CPF;
                    clienteModel.Email = cliente.Email;
                    clienteModel.DataNascimento = cliente.DataNascimento;
                    clienteModel.DataUltimaAtualizacao = DateTime.Now;

                    _databaseContext.Entry(cliente).Property(c => c.Senha).IsModified = false;

                    _databaseContext.Update(clienteModel);

                    TempData["mensagem"] = (await _databaseContext.SaveChangesAsync() > 0) ?
                    MensagemModel.Serializar("Cliente alterado com sucesso!") :
                    MensagemModel.Serializar("Erro ao altera cliente.", TipoMensagem.Erro);
                }
            }

            // ID not in route? Create
            else
            {
                cliente.DataUltimaAtualizacao = DateTime.Now;
                await _databaseContext.AddAsync(cliente);

                TempData["mensagem"] = (await _databaseContext.SaveChangesAsync() > 0) ?
                MensagemModel.Serializar("Cliente salvo com sucesso!") :
                MensagemModel.Serializar("Erro ao salvar cliente.", TipoMensagem.Erro);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Excluir(int? id)
        {
            ClienteModel cliente = await _databaseContext.Clientes.FindAsync(id.Value);

            if (cliente != null)
                return View(cliente);

            TempData["mensagem"] = MensagemModel.Serializar("Erro ao excluir cliente.", TipoMensagem.Erro);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(ClienteModel cliente)
        {
            ClienteModel clienteModel = await _databaseContext.Clientes.FindAsync(cliente.IdUsuario);

            if (clienteModel == null)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Categoria não encontrada.", tipo: TipoMensagem.Erro);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _databaseContext.Remove(clienteModel);
            }

            TempData["mensagem"] = (await _databaseContext.SaveChangesAsync() > 0) ?
            MensagemModel.Serializar("Cliente excluida com sucesso!") :
            MensagemModel.Serializar("Erro ao excluir cliente.", tipo: TipoMensagem.Erro);

            return RedirectToAction("Index");
        }
    }
}