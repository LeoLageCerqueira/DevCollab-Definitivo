using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevCollab.Domain.Entities;
using DevCollab.Infra.Context;
using DevCollab.Domain.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Azure.Storage.Blobs;

namespace DevCollab.WebApp.Controllers
{

    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly UsuarioService _service;
        private const string connectionString = @"DefaultEndpointsProtocol=https;AccountName=storage4pb;AccountKey=yl2aPSDz28eDjMoNH1YBVZQ05jaZTz6Dx/GLkaUFyWvHxODfTnn+3V8zBIe77gHm3qf0cYmAGZ0h+AStjQwhlA==;EndpointSuffix=core.windows.net";
        private const string containerName = "pbteste";

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
        public async Task<IActionResult> Create(Usuario usuario, IFormFile CaminhoFotoPerfil) {
            if (ModelState.IsValid)
            {
                usuario.Id = GetUserId();
                if (CaminhoFotoPerfil != null) {
                    string uri = await UploadImage(CaminhoFotoPerfil);
                    usuario.CaminhoFotoPerfil = uri;
                }
                _service.CriarUsuario(usuario);
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        private static async Task<string> UploadImage(IFormFile imageFile) {

            var reader = imageFile.OpenReadStream();
            var cloundStorageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = cloundStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();
            CloudBlockBlob blob = container.GetBlockBlobReference(imageFile.FileName);
            await blob.UploadFromStreamAsync(reader);
            return blob.Uri.ToString();
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
        public async Task<IActionResult> Edit(Guid id, Usuario usuario, IFormFile CaminhoFotoPerfil)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                usuario.CaminhoFotoPerfil = await UploadImage(CaminhoFotoPerfil); //erro
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
                DeleteFoto(usuario.CaminhoFotoPerfil);
                _service.ExcluirUsuario(usuario);
            }
            return RedirectToAction(nameof(Index));
        }

        private static void DeleteFoto(string foto) {

            if (foto != null) {
                try {
                    string nomeArquivo = foto.Split("/" + containerName + "/")[1];
                    var blobClient = new BlobClient(connectionString, containerName, nomeArquivo);
                    blobClient.Delete();
                }
                catch { }
            }
        }

        private Guid GetUserId() {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}