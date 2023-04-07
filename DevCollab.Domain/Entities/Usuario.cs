using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevCollab.Domain.Entities
{
	public class Usuario
	{
		public Guid Id { get; set; }
		public string Nome { get; set; }
		public string CaminhoFotoPerfil { get; set; }
		public string Descricao { get; set; }
		public ICollection<Publicacao> Publicacoes { get; set; }
		public ICollection<SeguidorSeguido> Seguidores { get; set; }
		public ICollection<SeguidorSeguido> Seguindo { get; set; }
		public ICollection<Comentario> Comentarios { get; set; }
	}
}
