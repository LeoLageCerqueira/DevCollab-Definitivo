using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevCollab.Domain.Entities
{
	public class Publicacao
	{
		[Key]
		public int IdPublicacao { get; set; }
		public string Texto { get; set; }

		public Guid AutorId { get; set; }
		public Usuario Autor { get; set; }

		public ICollection<Comentario> Comentarios { get; set; }
	}
}
