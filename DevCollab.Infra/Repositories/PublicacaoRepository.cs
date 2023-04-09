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
        //public List<Publicacao> FiltrarPublicacoesParaFeed()
        //{
        //    List<Publicacao> publicacoesAll = ObterTodasPublicacoes();
        //    List<Publicacao> publicacoesFeed = publicacoesAll.Select(p => ) 
        //}

        public int Criar(Publicacao publicacao) {
            _dbContext.Add(publicacao);
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
