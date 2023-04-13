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
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Azure.Storage.Blobs;

namespace DevCollab.WebApp.Controllers
{
    [Authorize]
    public class PublicacaoController : Controller
    {
        private readonly PublicacaoService _publicacaoService;
        private readonly SeguidorSeguidoService _seguidorSeguidoService;
        private const string connectionString = @"DefaultEndpointsProtocol=https;AccountName=storage4pb;AccountKey=yl2aPSDz28eDjMoNH1YBVZQ05jaZTz6Dx/GLkaUFyWvHxODfTnn+3V8zBIe77gHm3qf0cYmAGZ0h+AStjQwhlA==;EndpointSuffix=core.windows.net";
        private const string containerName = "pbteste";

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

            Publicacao publicacao = new()
            { AutorId = autorId };

            return View(publicacao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Publicacao publicacao, IFormFile Texto) {
            if (ModelState.IsValid) {
                Guid autorId = GetUserId();

                publicacao.AutorId = autorId;
                if (Texto != null) {
                    string uri = await UploadImage(Texto);
                    publicacao.Texto = uri;
                }

                _publicacaoService.CriarPublicacao(publicacao);
                return RedirectToAction(nameof(Index));
            }
            return View(publicacao);
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
        public async Task<IActionResult> Edit(int Id, Publicacao publicacao, IFormFile Texto)
        {
            if (Id != publicacao.IdPublicacao) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                Guid autorId = GetUserId();
                publicacao.AutorId = autorId;
                publicacao.Texto = await UploadImage(Texto);
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
                DeleteFoto(publicacao.Texto);
                _publicacaoService.ExcluirPublicacao(publicacao);
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
		public Usuario ObterPorId(Guid id)
		{
			return _seguidorSeguidoService.ObterClicadoPorId(id);
		}
	}
}
