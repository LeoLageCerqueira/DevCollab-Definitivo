using DevCollab.Domain.Entities;
using DevCollab.Domain.Interfaces;
using DevCollab.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace DevCollab.Infra.Repositories
{
    public class SeguidorSeguidoRepository : ISeguidorSeguidoRepository {
        private readonly DevCollabDbContext _dbContext;

        public SeguidorSeguidoRepository(DevCollabDbContext dbContext) {
            _dbContext = dbContext;
        }

        public int CriarRelacaoUsuarios(Usuario seguidor, Usuario seguido) {
            SeguidorSeguido seguidorSeguido = new(seguidor, seguido);
            _dbContext.SeguidoresSeguidos.Add(seguidorSeguido);
            return _dbContext.SaveChanges();
        }

        public Usuario ObterClicadoPorId(Guid id) {
            return _dbContext.Usuarios.FirstOrDefault(u => u.Id == id);
        }

        public List<Usuario> ObterSeguidosPorSeguidorId(Guid seguidorId) {
            return _dbContext.SeguidoresSeguidos
                .Where(a => a.Seguidor.Id == seguidorId)
                .Select(a => a.Seguido)
                .ToList();
        }
		public int DeletarRelacaoUsuarios(Usuario seguidor, Usuario seguido)
		{
			SeguidorSeguido seguidorSeguido = new(seguidor, seguido);
			_dbContext.SeguidoresSeguidos.Remove(seguidorSeguido);
			return _dbContext.SaveChanges();
		}
	}
}