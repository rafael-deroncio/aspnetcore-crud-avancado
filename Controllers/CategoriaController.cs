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
    public class CategoriaController : Controller
    {
        private readonly DatabaseContext _databaseContext;

        public CategoriaController(DatabaseContext DatabaseContext)
        {
            _databaseContext = DatabaseContext;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Subtitulo = "Listagem de Categorias";

            List<CategoriaModel> categorias = await _databaseContext.Categorias
                .OrderBy(x => x.IdCategoria)
                .AsNoTracking()
                .ToListAsync();

            return View(categorias);
        }

        [HttpGet]
        public async Task<IActionResult> Cadastrar(int? id)
        {
            if (id.HasValue)
            {
                ViewBag.Subtitulo = "Listagem de Categorias";
                CategoriaModel categoriaModel = await _databaseContext.Categorias.FindAsync(id);

                if (categoriaModel != null)
                {
                    return View(categoriaModel);
                }
                return NotFound();
            }
            return View(new CategoriaModel());
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(int? id, [FromForm] CategoriaModel categoria)
        {
            // Modelo Preenchido
            if (ModelState.IsValid)
            {
                // ID in route? Edit
                if (id.HasValue && id.Value != 0)
                {
                    var categoriaModel = _databaseContext.Categorias.Where(x => x.IdCategoria == id.Value).First();
                    if (categoriaModel != null)
                    {
                        categoriaModel.Nome = categoria.Nome;
                        categoriaModel.Ativo = categoria.Ativo;
                        categoriaModel.DataUltimaAtualizacao = DateTime.Now;

                        _databaseContext.Update(categoriaModel);
                    }
                }

                // ID not in route? Create
                else
                {
                    categoria.DataCadastro = DateTime.Now;
                    categoria.DataUltimaAtualizacao = DateTime.Now;
                    categoria.Ativo = true;
                    await _databaseContext.AddAsync(categoria);    
                }
                
                TempData["salvou"] = (await _databaseContext.SaveChangesAsync() > 0);
                return RedirectToAction("Index");
            }
            // Modelo Nao Preenchido
            else
            {
                return View(categoria);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Excluir(int? id)
        {
            CategoriaModel categoriaModel = await _databaseContext.Categorias.FindAsync(id.Value);

            if (!id.HasValue | categoriaModel == null)
            {
                TempData["existe"] = false;
                return RedirectToAction("Index");
            }
            
            return View(categoriaModel);
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(CategoriaModel categoria)
        {
            CategoriaModel categoriaModel = await _databaseContext.Categorias.FindAsync(categoria.IdCategoria);

            if (categoriaModel != null)
            {
                _databaseContext.Remove(categoriaModel);
                 
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