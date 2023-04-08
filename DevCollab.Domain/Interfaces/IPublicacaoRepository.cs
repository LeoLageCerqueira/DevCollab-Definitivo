using DevCollab.Domain.Entities;
using System;
using System.Collections.Generic;

namespace DevCollab.Domain.Interfaces {
    public interface IPublicacaoRepository {
        
        List<Publicacao> ObterTodasPublicacoes();
        Publicacao ObterAutorPorId(Guid autorId);
        public int Criar(Publicacao publicacao);
        public int Vazio();
        public Publicacao Consultar(int IdPublicacao);
        public int Alterar(Publicacao publicacao);
        public int Excluir(Publicacao publicacao);
    }
}