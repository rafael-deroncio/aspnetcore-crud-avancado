using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUDAvancado.Models;
using CrudAvancado.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CrudAvancado.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly DatabaseContext _databaseContext;

        public ProdutoController(DatabaseContext DatabaseContext)
        {
            _databaseContext = DatabaseContext;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Subtitulo = "Listagem de Produtos";

            List<ProdutoModel> produtos = await _databaseContext.Produtos
                .OrderBy(x => x.IdProduto)
                .AsNoTracking()
                .ToListAsync();

            produtos.ForEach(produto => 
            {
                produto.Categoria = _databaseContext.Categorias.Find(produto.IdCategoria);
            });

            return View(produtos);
        }

        [HttpGet]
        public async Task<IActionResult> Cadastrar(int? id)
        {
            ViewBag.Categorias = new SelectList(
                await _databaseContext.Categorias.OrderBy(x => x.IdCategoria).AsNoTracking().ToListAsync(),
                 nameof(CategoriaModel.IdCategoria), nameof(CategoriaModel.Nome));

            if (id.HasValue)
            {
                ViewBag.Subtitulo = "Listagem de Produtos";
                ProdutoModel produtoModel = await _databaseContext.Produtos.FindAsync(id);

                if (produtoModel != null)
                {
                    return View(produtoModel);
                }
                return NotFound();
            }
            return View(new ProdutoModel());
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(int? id, [FromForm] ProdutoModel produto)
        {
            // Modelo Preenchido
            if (ModelState.IsValid)
            {
                // ID in route? Edit
                if (id.HasValue && id.Value != 0)
                {
                    ProdutoModel produtoModel = await _databaseContext.Produtos.FindAsync(id);

                    if (produtoModel != null)
                    {
                        produtoModel.Nome = produto.Nome;
                        produtoModel.DataUltimaAtualizacao = DateTime.Now;

                        _databaseContext.Update(produtoModel);
                    }
                }

                // ID not in route? Create
                else
                {
                    ProdutoModel produtoModel = new();
                    produtoModel.Nome = produto.Nome;
                    produtoModel.Valor = produto.Valor;
                    produtoModel.Quantidade = produto.Quantidade;
                    produtoModel.IdCategoria = produto.IdCategoria;
                    produtoModel.Categoria = await _databaseContext.Categorias.FindAsync(produto.IdCategoria);

                    produtoModel.DataUltimaAtualizacao = DateTime.Now;
                    await _databaseContext.AddAsync(produtoModel);
                }
                
                TempData["salvou"] = (await _databaseContext.SaveChangesAsync() > 0);
                return RedirectToAction("Index");
            }
            // Modelo Nao Preenchido
            else
            {
                return View(produto);
            }

        }
    
        [HttpGet]
        public async Task<IActionResult> Excluir(int? id)
        {
            ProdutoModel produtoModel = await _databaseContext.Produtos.FindAsync(id.Value);

            if (!id.HasValue | produtoModel == null)
            {
                TempData["existe"] = false;
                return RedirectToAction("Index");
            }
            
            return View(produtoModel);
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(ProdutoModel produto)
        {
            ProdutoModel produtoModel = await _databaseContext.Produtos.FindAsync(produto.IdProduto);

            if (produtoModel != null)
            {
                _databaseContext.Remove(produtoModel);
                 
                TempData["excluiu"] = (await _databaseContext.SaveChangesAsync() > 0) ? true : false;
            }
            else
            {
                TempData["existe"] = false;
            }
            
            return RedirectToAction("Index");
        }
    }
}