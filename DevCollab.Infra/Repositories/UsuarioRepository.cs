
using DevCollab.Domain.Entities;
using DevCollab.Domain.Interfaces;
using DevCollab.Infra.Context;

namespace DevCollab.Infra.Repositories {
    public class UsuarioRepository : IUsuarioRepository {

        private readonly DevCollabDbContext _context;

        public UsuarioRepository(DevCollabDbContext context) {
            _context = context;
        }

        public int Vazio() {
            int cont = _context.Usuarios.Count();
            return cont;
        }

        public ICollection<Usuario> ConsultarTodos() {
            return _context.Usuarios.ToList();
        }

        public Usuario Consultar(Guid id) {
            try
            {
                return _context.Usuarios.Find(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int Criar(Usuario usuario) {
            try {
                _context.Usuarios.Add(usuario);
                return _context.SaveChanges();
            }
            catch (Exception) {
                return -1;
            }    
        }

        public int Alterar(Usuario usuario) {
            _context.Usuarios.Update(usuario);
            return _context.SaveChanges();
        }

        public int Excluir(Usuario usuario) {
            _context.Usuarios.Remove(usuario);
            return _context.SaveChanges();
        }

		public List<Usuario> ObterSeguidosPorSeguidorId(Guid seguidorId)
		{
            return _context.SeguidoresSeguidos
				.Where(a => a.Seguidor.Id == seguidorId)
				.Select(a => a.Seguido)
				.ToList();
		}
	}
}