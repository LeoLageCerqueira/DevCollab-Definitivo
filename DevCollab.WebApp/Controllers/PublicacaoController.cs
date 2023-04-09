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
    [Authorize]
    public class PublicacaoController : Controller
    {
        private readonly PublicacaoService _publicacaoService;
        private readonly SeguidorSeguidoService _seguidorSeguidoService;

        public PublicacaoController(PublicacaoService publicacaoService, SeguidorSeguidoService seguidorSeguidoService) {
            _publicacaoService = publicacaoService;
            _seguidorSeguidoService = seguidorSeguidoService;
        }

        public IActionResult Index() {
            Guid autorId = GetUserId();
            List<Publicacao> publicacoesFeed = PublicacoesFeed();
			ViewBag.autorId = autorId;
            return View(publicacoesFeed);
        }

        public List<Publicacao> PublicacoesFeed()
        {
			Guid autorId = GetUserId();
			Usuario seguidor = ObterPorId(autorId);

			List<Usuario> usuariosSeguidos = _seguidorSeguidoService.ObterSeguidosPorSeguidorId(autorId);
            usuariosSeguidos.Add(seguidor);
			List<Publicacao> publicacoesAll = _publicacaoService.ObterTodasPublicacoes();
			List<Publicacao> publicacoesFeed = new List<Publicacao>();
			foreach (Usuario usuario in usuariosSeguidos)
			{
				publicacoesFeed.AddRange(publicacoesAll.Where(p => p.Autor == usuario));
			}
            return publicacoesFeed;
		}

        public IActionResult Create() {
            Guid autorId = GetUserId();

            Publicacao publicacao = new Publicacao { AutorId = autorId };

            return View(publicacao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Publicacao publicacao) {
            if (ModelState.IsValid) {
                Guid autorId = GetUserId();

                publicacao.AutorId = autorId;

                _publicacaoService.CriarPublicacao(publicacao);
                return RedirectToAction(nameof(Index));
            }
            return View(publicacao);
        }

        public IActionResult Edit(int Id)
		{
            //if (IdPublicacao == null || _publicacaoService.PublicacoesVazio())
            //{
            //    return NotFound();
            //}
            Console.WriteLine(Id);
			var publicacao = _publicacaoService.ConsultarPublicacao(Id);
            //if (publicacao == null) {
            //    return NotFound();
            //}
            return View(publicacao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int Id, Publicacao publicacao)
        {
            if (Id != publicacao.IdPublicacao) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                Guid autorId = GetUserId();
                publicacao.AutorId = autorId;
                Console.WriteLine(autorId);
                bool result = _publicacaoService.AlterarPublicacao(publicacao);
                if (!result) {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
			return View(publicacao);
        }

		public IActionResult Delete(int Id)
        {
            if (_publicacaoService.PublicacoesVazio())
            {
                return NotFound();
            }

            var publicacao = _publicacaoService.ConsultarPublicacao(Id);
            if (publicacao == null) {
                return NotFound();
            }
            return View(publicacao);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int IdPublicacao)
        {
            if (_publicacaoService.PublicacoesVazio())
            {
                return Problem("Entity set 'DevCollabDbContext.Publicacoes'  is null.");
            }
            var publicacao = _publicacaoService.ConsultarPublicacao(IdPublicacao);
            if (publicacao != null)
            {
                _publicacaoService.ExcluirPublicacao(publicacao);
            }
			return RedirectToAction(nameof(Index));
        }

        private Guid GetUserId() {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
		public Usuario ObterPorId(Guid id)
		{
			return _seguidorSeguidoService.ObterClicadoPorId(id);
		}
	}
}
