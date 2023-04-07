using DevCollab.Domain.Entities;
using DevCollab.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevCollab.Domain.Services {
    public class SeguidorSeguidoService {
        private readonly ISeguidorSeguidoRepository _seguidorSeguidoRepository;

        public SeguidorSeguidoService(ISeguidorSeguidoRepository seguidorSeguidoRepository) {
            _seguidorSeguidoRepository = seguidorSeguidoRepository;
        }

        public int CriarRelacaoUsuarios(Usuario seguidor, Usuario seguido) {
            return _seguidorSeguidoRepository.CriarRelacaoUsuarios(seguidor, seguido);
        }
		public int DeletarRelacaoUsuarios(Usuario seguidor, Usuario seguido)
		{
			return _seguidorSeguidoRepository.DeletarRelacaoUsuarios(seguidor, seguido);
		}
		public Usuario ObterClicadoPorId(Guid id) {
            return _seguidorSeguidoRepository.ObterClicadoPorId(id);
        }

        public List<Usuario> ObterSeguidosPorSeguidorId(Guid seguidorId) {
            return _seguidorSeguidoRepository.ObterSeguidosPorSeguidorId(seguidorId);
        }



        // validações que não estavam funcionando no meu código

        //public ICollection<SeguidorSeguido> ConsultarAmigos(Guid seguidorId) {
        //    return _seguidorSeguidoRepository.ConsultarAmigos();
        //}

        //public int Vazio() {
        //    return _seguidorSeguidoRepository.Vazio();
        //}

        //public bool UsuarioExiste(Guid usuarioId) {
        //    return _seguidorSeguidoRepository.UsuarioExiste(usuarioId);
        //}

        //public bool JaSegueUsuario(Guid seguidorId, Guid seguidoId) {
        //    return _seguidorSeguidoRepository.JaSegueUsuario(seguidorId, seguidoId);
        //}

        //public ICollection<Usuario> ConsultarUsuarios() {
        //    return null;
        //}

        //public bool SeguirUsuario(Guid seguidorId, Guid seguidoId) {
        //    bool usuarioSeguido = _seguidorSeguidoRepository.UsuarioExiste(seguidoId) &&
        //                          !_seguidorSeguidoRepository.JaSegueUsuario(seguidorId, seguidoId);

        //    if (usuarioSeguido) {
        //        _seguidorSeguidoRepository.CriarRelacaoUsuarios(new Usuario { Id = seguidorId },
        //                                                         new Usuario { Id = seguidoId });
        //        return true;
        //    }
        //    else {
        //        return false;
        //    }
        //}
    }
}