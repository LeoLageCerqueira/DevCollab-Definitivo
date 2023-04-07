using DevCollab.Domain.Entities;
using DevCollab.Domain.Interfaces;

namespace DevCollab.Domain.Services {
    public class UsuarioService {

        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository) {
            _usuarioRepository = usuarioRepository;
        }

        public bool UsuariosVazio() {
            if (_usuarioRepository.Vazio() == 0) {
                return true;
            }
            return false;
        }

        public ICollection<Usuario> ConsultarTodos() {
            return _usuarioRepository.ConsultarTodos();
        }

        public Usuario ConsultarUsuario(Guid id) {
            return _usuarioRepository.Consultar(id);
        }

        public bool CriarUsuario(Usuario usuario) {
            int cont = _usuarioRepository.Criar(usuario);
            if (cont == 0) {
                return false;
            }
            return true;
        }

        public bool AlterarUsuario(Usuario usuario) {
            int cont = _usuarioRepository.Alterar(usuario);
            if (cont == 0) {
                return false;
            }
            return true;
        }

        public bool ExcluirUsuario(Usuario usuario) {
            int cont = _usuarioRepository.Excluir(usuario);
            if (cont == 0) {
                return false;
            }
            return true;
        }
		public List<Usuario> ObterSeguidosPorSeguidorId(Guid seguidorId)
		{
			return _usuarioRepository.ObterSeguidosPorSeguidorId(seguidorId);
		}
	}
}