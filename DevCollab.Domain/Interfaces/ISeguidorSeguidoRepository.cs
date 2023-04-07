using DevCollab.Domain.Entities;

namespace DevCollab.Domain.Interfaces
{
    public interface ISeguidorSeguidoRepository {
        public int CriarRelacaoUsuarios(Usuario seguidor, Usuario seguido);
        public int DeletarRelacaoUsuarios(Usuario seguidor, Usuario seguido);
        public Usuario ObterClicadoPorId(Guid id);
        public List<Usuario> ObterSeguidosPorSeguidorId(Guid seguidorId);

    }
}