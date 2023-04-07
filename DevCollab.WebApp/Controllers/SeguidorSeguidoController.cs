using DevCollab.Domain.Entities;
using DevCollab.Domain.Services;
using DevCollab.Infra.Context;
using DevCollab.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DevCollab.WebApp.Controllers
{
    public class SeguidorSeguidoController : Controller {
        private readonly SeguidorSeguidoService _seguidorSeguidoService;

        public SeguidorSeguidoController(SeguidorSeguidoService seguidorSeguidoService) {
            _seguidorSeguidoService = seguidorSeguidoService;
        }

        public IActionResult ListaAmigos()
        {
            var seguidorId = GetUserId();
            var seguidos = _seguidorSeguidoService.ObterSeguidosPorSeguidorId(seguidorId);
            return View(seguidos);
        }

        public IActionResult Seguir(Guid? id) {
            var seguido = _seguidorSeguidoService.ObterClicadoPorId(id.Value);
            return View(seguido);
        }

        [HttpPost, ActionName("Seguir")]
        [ValidateAntiForgeryToken]
        public ActionResult SeguirConfirmed(Guid Id) {
            var seguidorId = GetUserId();
            var seguidor = ObterPorId(seguidorId);
            var seguido = ObterPorId(Id);
            _seguidorSeguidoService.CriarRelacaoUsuarios(seguidor, seguido);
            return RedirectToAction("ListaAmigos");
        }
		public IActionResult Deletar(Guid? Id)
		{
			var seguido = _seguidorSeguidoService.ObterClicadoPorId(Id.Value);
			return View(seguido);
		}

		[HttpPost, ActionName("Deletar")]
		[ValidateAntiForgeryToken]
		public ActionResult DeletarConfirmed(Guid Id)
		{
			var seguidorId = GetUserId();
			var seguidor = ObterPorId(seguidorId);
			var seguido = ObterPorId(Id);
			_seguidorSeguidoService.DeletarRelacaoUsuarios(seguidor, seguido);
			return RedirectToAction("ListaAmigos");
		}

		public Usuario ObterPorId(Guid id) {
            return _seguidorSeguidoService.ObterClicadoPorId(id);
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}