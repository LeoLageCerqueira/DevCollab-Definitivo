using DevCollab.Domain.Entities;
using DevCollab.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevCollab.Domain.Services {
    public class PublicacaoService {

        private readonly IPublicacaoRepository _publicacaoRepository;

        public PublicacaoService(IPublicacaoRepository publicacaoRepository) {
            _publicacaoRepository = publicacaoRepository;
        }

        public List<Publicacao> ObterTodasPublicacoes() {
            return _publicacaoRepository.ObterTodasPublicacoes();
        }

        public bool CriarPublicacao(Publicacao publicacao) {
            int cont = _publicacaoRepository.Criar(publicacao);
            if (cont > 0) {
                return true;
            }

            return false;
        }

        public bool PublicacoesVazio() {
            if (_publicacaoRepository.Vazio() == 0) {
                return true;
            }
            return false;
        }

        public Publicacao ConsultarPublicacao(int IdPublicacao)
		{
            return _publicacaoRepository.Consultar(IdPublicacao);
        }

        public bool AlterarPublicacao(Publicacao publicacao) {
            int cont = _publicacaoRepository.Alterar(publicacao);
            if (cont == 0) {
                return false;
            }
            return true;
        }

        public bool ExcluirPublicacao(Publicacao publicacao) {
            int cont = _publicacaoRepository.Excluir(publicacao);
            if (cont == 0) {
                return false;
            }
            return true;
        }
    }
}
