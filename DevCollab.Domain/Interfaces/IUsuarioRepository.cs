using DevCollab.Domain.Entities;

namespace DevCollab.Domain.Interfaces {
    public interface IUsuarioRepository {

        public int Vazio();

        public ICollection<Usuario> ConsultarTodos();

        public Usuario Consultar(Guid id);

        public int Criar(Usuario usuario); 

        public int Alterar(Usuario usuario);

        public int Excluir(Usuario usuario);
		List<Usuario> ObterSeguidosPorSeguidorId(Guid seguidorId);
	}
}