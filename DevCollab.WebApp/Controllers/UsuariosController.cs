using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevCollab.Domain.Entities;
using DevCollab.Infra.Context;
using DevCollab.Domain.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace DevCollab.WebApp.Controllers
{

    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly UsuarioService _service;
        //private readonly SeguidorSeguidoService _seguidorSeguidoService;

		public UsuariosController(UsuarioService service)
		{
			_service = service;
		}

        public IActionResult Index()
        {
            Guid userId = GetUserId();
            Usuario usuario = _service.ConsultarUsuario(userId);
            if (usuario == null) {
                return RedirectToAction("Create");
            }
            else {
                return RedirectToAction("Details");
            }
        }

        public IActionResult ListaUsers()
        {
			var seguidorId = GetUserId();
			var seguidor = _service.ConsultarUsuario(seguidorId);
			ICollection<Usuario> usuarioList = _service.ConsultarTodos();
			List<Usuario> seguidos = _service.ObterSeguidosPorSeguidorId(seguidorId);
			ViewBag.seguidor = seguidor;
			ViewBag.Seguidos = seguidos;
			return View(usuarioList);
        }

        public IActionResult Details(Guid? id)
        {
            if (_service.UsuariosVazio())
            {
                return RedirectToAction("Create");
            }
            if (id == null) {
                id = GetUserId();
            }
            var usuario = _service.ConsultarUsuario(id.Value);
            if (usuario == null)
            {
                return RedirectToAction("Create");
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.Id = GetUserId();
                _service.CriarUsuario(usuario);
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null || _service.UsuariosVazio())
            {
                return NotFound();
            }

            var usuario = _service.ConsultarUsuario(id.Value);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                bool result = _service.AlterarUsuario(usuario);
                if (!result) {
                    return NotFound(); 
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null || _service.UsuariosVazio())
            {
                return NotFound();
            }

            var usuario = _service.ConsultarUsuario(id.Value);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            if (_service.UsuariosVazio())
            {
                return Problem("Entity set 'DevCollabDbContext.Usuarios'  is null.");
            }
            var usuario = _service.ConsultarUsuario(id);
            if (usuario != null)
            {
                _service.ExcluirUsuario(usuario);
            }
            return RedirectToAction(nameof(Index));
        }

        private Guid GetUserId() {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}