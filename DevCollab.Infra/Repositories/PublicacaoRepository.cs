using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevCollab.Domain.Entities;
using DevCollab.Domain.Interfaces;
using DevCollab.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace DevCollab.Infra.Repositories {
    public class PublicacaoRepository : IPublicacaoRepository {

        private readonly DevCollabDbContext _dbContext;

        public PublicacaoRepository(DevCollabDbContext dbContext) {
            _dbContext = dbContext;
        }

        public List<Publicacao> ObterTodasPublicacoes() {
            return _dbContext.Publicacoes.Include(p => p.Autor).ToList();
        }

        public Publicacao ObterAutorPorId(Guid autorId) {
            // Use o contexto de banco de dados (_dbContext) para buscar o autor pelo id
            return _dbContext.Publicacoes.FirstOrDefault(a => a.AutorId == autorId);
        }

        public int Criar(Publicacao publicacao) {
            // Adicione a publicação ao contexto do banco de dados
            _dbContext.Add(publicacao);

            // Salve as mudanças no banco de dados e retorne o número de registros afetados
            return _dbContext.SaveChanges();
        }

        public int Vazio() {
            int cont = _dbContext.Publicacoes.Count();
            return cont;
        }

        public Publicacao Consultar(int IdPublicacao)
		{
            try {
                return _dbContext.Publicacoes.Find(IdPublicacao);
            }
            catch (Exception) {
                return null;
            }
        }

        public int Alterar(Publicacao publicacao) {
            _dbContext.Publicacoes.Update(publicacao);
            return _dbContext.SaveChanges();
        }

        public int Excluir(Publicacao publicacao) {
            _dbContext.Publicacoes.Remove(publicacao);
            return _dbContext.SaveChanges();
        }
    }
}
