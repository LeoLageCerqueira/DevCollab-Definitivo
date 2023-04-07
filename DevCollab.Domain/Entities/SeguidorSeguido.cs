using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevCollab.Domain.Entities
{
	public class SeguidorSeguido
	{
		[Key]
		public Guid SeguidorId { get; set; }

		public Usuario Seguidor { get; set; }

		[Key]
		public Guid SeguidoId { get; set; }

		public Usuario Seguido { get; set; }

		public SeguidorSeguido()
		{

		}
		public SeguidorSeguido(Usuario seguidor, Usuario seguido)
		{
			Seguidor = seguidor;
			Seguido = seguido;
		}
	}
}
