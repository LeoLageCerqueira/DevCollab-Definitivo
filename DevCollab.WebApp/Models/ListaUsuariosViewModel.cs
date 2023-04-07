using DevCollab.Domain.Entities;

namespace DevCollab.WebApp.Models {
    public class ListaUsuariosViewModel {
        public Guid SeguidorId {
            get; set;
        }
        public IEnumerable<Usuario> Usuarios {
            get; set;
        }
    }
}