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

        public string ObterAutorNome(Guid autorId) {
            // Chame o método no repositório para obter o autor pelo id
            var autor = _publicacaoRepository.ObterAutorPorId(autorId);
            // Verifique se o autor foi encontrado
            if (autor != null && autor.Autor != null) {
                // Retorne o nome do autor
                return autor.Autor.Nome;
            }
            // Retorne uma string vazia caso o autor não tenha sido encontrado ou a propriedade Nome seja nula
            return string.Empty;
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
