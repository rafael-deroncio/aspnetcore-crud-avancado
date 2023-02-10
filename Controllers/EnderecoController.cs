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
    public class EnderecoController : Controller
    {
        private readonly DatabaseContext _databaseContext;

        public EnderecoController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IActionResult> Index(int? cid)
        {
            ViewBag.Subtitulo = "Listagem de Endereços";

            if (cid.HasValue)
            {
                var cliente = await _databaseContext.Clientes.FindAsync(cid.Value);

                if (cliente != null)
                {
                    ViewBag.Cliente = cliente;
                    _databaseContext.Entry(cliente).Collection(c => c.Enderecos).Load();
                    return View(cliente.Enderecos);
                }
            }

            TempData["existe"] = false;
            return RedirectToAction(nameof(Index), "Cliente");
        }

        [HttpGet]
        public async Task<IActionResult> Cadastrar(int? cid, int? eid)
        {
            if (cid.HasValue)
            {
                var cliente = await _databaseContext.Clientes.FindAsync(cid);
                if (cliente != null)
                {
                    ViewBag.Cliente = cliente;
                    if (eid.HasValue)
                    {
                        _databaseContext.Entry(cliente).Collection(c => c.Enderecos).Load();
                        var endereco = cliente.Enderecos.FirstOrDefault(e => e.IdEndereco == eid);
                        if (endereco != null)
                        {
                            return View(endereco);
                        }
                        else
                        {
                            TempData["mensagem"] = MensagemModel.Serializar("Endereço não encontrado.", TipoMensagem.Erro);
                        }
                    }
                    else
                    {
                        return View(new EnderecoModel());
                    }
                }
                else
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Cliente não encontrado.", TipoMensagem.Erro);
                }
                return RedirectToAction("Index");
            }
            else
            {
                TempData["mensagem"] = MensagemModel.Serializar("Nenhum proprietário de endereços foi informado.", TipoMensagem.Erro);
                return RedirectToAction("Index", "Cliente");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromForm] int? idUsuario, [FromForm] EnderecoModel endereco)
        {
            // Modelo Nao Preenchido
            if (!ModelState.IsValid)
                return View(endereco);

            // Modelo Preenchido
            // Alteração de endereço
            if (idUsuario.HasValue)
            {
                ClienteModel cliente = await _databaseContext.Clientes.FindAsync(idUsuario.Value);
                ViewBag.Cliente = cliente;

                endereco.CEP = ObterCepNormalizado(endereco.CEP);

                if (cliente.Enderecos.Count() == 0) endereco.Selecionado = true;

                if (endereco.IdEndereco > 0)
                {
                    if (endereco.Selecionado) cliente.Enderecos.ToList().ForEach(e => e.Selecionado = false);

                    if (EnderecoExiste(idUsuario.Value, endereco.IdEndereco))
                    {
                        EnderecoModel enderecoAtual = cliente.Enderecos.FirstOrDefault(e => e.IdEndereco == endereco.IdEndereco);
                        _databaseContext.Entry(enderecoAtual).CurrentValues.SetValues(endereco);

                        TempData["mensagem"] =
                        (_databaseContext.Entry(enderecoAtual).State == EntityState.Unchanged) ?
                        MensagemModel.Serializar("Nenhum dado do endereço foi alterado.", TipoMensagem.Erro) :

                        (await _databaseContext.SaveChangesAsync() > 0) ?
                        MensagemModel.Serializar("Endereço alterado com sucesso!") :
                        MensagemModel.Serializar("Erro ao alterar endereço.", TipoMensagem.Erro);
                    }
                    else
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Endereço não encontrado");
                    }
                }
                // Inclusão de endereço
                else
                {
                    endereco.IdEndereco = cliente.Enderecos.Count() > 0 ?
                        cliente.Enderecos.Max(e => e.IdEndereco) + 1 : 1;

                    _databaseContext.Clientes.FirstOrDefault(c => c.IdUsuario == idUsuario).Enderecos.Add(endereco);

                    TempData["mensagem"] = (await _databaseContext.SaveChangesAsync() > 0) ?
                    MensagemModel.Serializar("Endereço salvo com sucesso!") :
                    MensagemModel.Serializar("Erro ao salvar endereço.", TipoMensagem.Erro);
                }

                return RedirectToAction(nameof(Index), "Endereco", new { cid = cliente.IdUsuario });
            }

            TempData["mensagem"] = MensagemModel.Serializar("Nenhum cliente foi encontrado.", TipoMensagem.Erro);

            return RedirectToAction(nameof(Index), "Cliente");
        }

        [HttpGet]
        public async Task<IActionResult> Excluir(int? cid, int? eid)
        {
            if (!cid.HasValue)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Nenhum cliente foi informado", TipoMensagem.Erro);
                return RedirectToAction(nameof(Index), "Cliente");
            }

            if (!eid.HasValue)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Nenhum endereço foi informado", TipoMensagem.Erro);
                return RedirectToAction(nameof(Index), new { cid = cid });
            }

            ClienteModel cliente = await _databaseContext.Clientes.FindAsync(cid.Value);
            EnderecoModel endereco = cliente.Enderecos.FirstOrDefault(e => e.IdEndereco == eid.Value);

            if (endereco == null)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Endereço não encontrado.", TipoMensagem.Erro);
                return RedirectToAction(nameof(Index), new { cid = cid });
            }

            ViewBag.Cliente = cliente;
            return View(endereco);
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(int idUsuario, int idEndereco)
        {
            ClienteModel cliente = await _databaseContext.Clientes.FindAsync(idUsuario);

            if (cliente != null)
            {
                EnderecoModel endereco = cliente.Enderecos.FirstOrDefault(e => e.IdEndereco == idEndereco);

                if (endereco != null) cliente.Enderecos.Remove(endereco);

                if (_databaseContext.SaveChanges() > 0)
                {
                    if (endereco.Selecionado && cliente.Enderecos.Count() > 0)
                    {
                        cliente.Enderecos.FirstOrDefault().Selecionado = true;
                        await _databaseContext.SaveChangesAsync();
                    }

                    TempData["mensagem"] = MensagemModel.Serializar("Endereço excluido com sucesso!");
                }
                else
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Erro ao excluir endereço.", TipoMensagem.Erro);
                }
            }

            return RedirectToAction(nameof(Index));
        }
    
    #region Metodos Privados
        private bool EnderecoExiste(int idUsuario, int idEndereco)
        {
            return _databaseContext.Clientes.FirstOrDefault(cliente => cliente.IdUsuario == idUsuario)
                                   .Enderecos.Any(endereco => endereco.IdEndereco == idEndereco);
        }

        private string ObterCepNormalizado(string cep)
        {
            string cepNormalizado = cep.Replace("-", "").Replace(".", "").Trim();
            return cepNormalizado.Insert(5, "-");
        }
    #endregion

    }
}