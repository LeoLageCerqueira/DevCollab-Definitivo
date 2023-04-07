using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DevCollab.Domain.Entities;
using DevCollab.Domain.Services;
using DevCollab.Infra.Context;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DevCollab.WebApp.Controllers
{
    //[Authorize]
    public class PublicacaoController : Controller
    {
        private readonly DevCollabDbContext _context;
        private readonly PublicacaoService _publicacaoService;

        public PublicacaoController(DevCollabDbContext context, PublicacaoService publicacaoService) {
            _context = context;
            _publicacaoService = publicacaoService;
        }

        public IActionResult Index() {
            var publicacoes = _publicacaoService.ObterTodasPublicacoes();
            return View(publicacoes);
        }

        public IActionResult Create() {
            Guid autorId = GetUserId();

            // Cria uma nova instância de Publicacao com o AutorId preenchido
            Publicacao publicacao = new Publicacao { AutorId = autorId };

            // Adiciona a instância à ViewData para exibição na view
            ViewData["AutorNome"] = _publicacaoService.ObterAutorNome(autorId);

            return View(publicacao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Publicacao publicacao) {
            if (ModelState.IsValid) {
                Guid autorId = GetUserId();

                // Preenche o AutorId com o id do autor logado
                publicacao.AutorId = autorId;

                _publicacaoService.CriarPublicacao(publicacao);
                return RedirectToAction(nameof(Index));
            }
            ViewData["AutorNome"] = _publicacaoService.ObterAutorNome(publicacao.AutorId);
            return View(publicacao);
        }

        public IActionResult Edit(Guid? id) {
            if (id == null || _publicacaoService.PublicacoesVazio()) {
                return NotFound();
            }

            var publicacao = _publicacaoService.ConsultarPublicacao(id.Value);
            if (publicacao == null) {
                return NotFound();
            }
            return View(publicacao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Publicacao publicacao)
        {
            if (id != publicacao.IdPublicacao) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                bool result = _publicacaoService.AlterarPublicacao(publicacao);
                if (!result) {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(publicacao);
        }

        // GET: Publicacao/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null || _publicacaoService.PublicacoesVazio())
            {
                return NotFound();
            }

            var publicacao = _publicacaoService.ConsultarPublicacao(id.Value);
            if (publicacao == null) {
                return NotFound();
            }
            return View(publicacao);
        }

        // POST: Publicacao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            if (_publicacaoService.PublicacoesVazio())
            {
                return Problem("Entity set 'DevCollabDbContext.Publicacoes'  is null.");
            }
            var publicacao = _publicacaoService.ConsultarPublicacao(id);
            if (publicacao != null)
            {
                _publicacaoService.ExcluirPublicacao(publicacao);
            }
            return RedirectToAction(nameof(Index));
        }

        private Guid GetUserId() {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
