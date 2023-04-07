using DevCollab.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevCollab.Infra.Context
{
	public class DevCollabDbContext : DbContext
	{
		private const string connectionString =
		"Data Source=leonardolage.database.windows.net;Initial Catalog=atdevcollab;User ID=leonardo;Password=Falafel148;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
		//public DevCollabDbContext()
		//{
		//	Database.EnsureCreated();
		//}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(connectionString);
		}
		public DbSet<Usuario> Usuarios { get; set; }
		public DbSet<Publicacao> Publicacoes { get; set; }
		public DbSet<Comentario> Comentarios { get; set; }
		public DbSet<SeguidorSeguido> SeguidoresSeguidos { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<SeguidorSeguido>()
				.HasKey(ss => new { ss.SeguidorId, ss.SeguidoId });

			//usuário pode seguir vários usuários (serve para contabilizar lista de pessoas seguidas)
			//num início, o feed de postagens só terá suas postagens
			//(Funcionalidade: O feed só incrementará com publicações de outros usuários se o usuário seguir a pessoa)
	
			modelBuilder.Entity<SeguidorSeguido>()
				.HasOne(ss => ss.Seguidor)
				.WithMany(u => u.Seguindo)
				.HasForeignKey(ss => ss.SeguidorId)
				.OnDelete(DeleteBehavior.Restrict);

			//usuário pode ser seguido por vários usuários 
			modelBuilder.Entity<SeguidorSeguido>()
				.HasOne(ss => ss.Seguido)
				.WithMany(u => u.Seguidores)
				.HasForeignKey(ss => ss.SeguidoId)
				.OnDelete(DeleteBehavior.Restrict);

			//Excluindo autor exclui todas as publicações
			modelBuilder.Entity<Publicacao>()
				.HasOne(p => p.Autor)
				.WithMany(u => u.Publicacoes)
				.HasForeignKey(p => p.AutorId)
				.OnDelete(DeleteBehavior.Cascade);
			//Excluindo autor exclui todos os comentários
			//modelBuilder.Entity<Comentario>()
			//	.HasOne(c => c.Autor)
			//	.WithMany(u => u.Comentarios)
			//	.HasForeignKey(c => c.AutorId)
			//	.OnDelete(DeleteBehavior.Cascade);
			//Excluindo publicação exclui todos os comentários
			modelBuilder.Entity<Comentario>()
				.HasOne(c => c.Publicacao)
				.WithMany(p => p.Comentarios)
				.HasForeignKey(c => c.PublicacaoId)
				.OnDelete(DeleteBehavior.Cascade);
		}


		//public DbSet<Aluno> Alunos { get; set; }
		//public DbSet<Amizade> Amizades { get; set; }

		//protected override void OnModelCreating(ModelBuilder modelBuilder)
		//{

		//	modelBuilder.Entity<Amizade>()
		//		.HasKey(am => new { am.IdAlunoA, am.IdAlunoB });
		//	modelBuilder.Entity<Amizade>()
		//		.HasOne(am => am.AlunoA)
		//		.WithMany(a => a.AmizadesB)
		//		.HasForeignKey(ad => ad.IdAlunoA)
		//		.OnDelete(DeleteBehavior.Restrict);
		//	modelBuilder.Entity<Amizade>()
		//		.HasOne(ad => ad.AlunoB)
		//		.WithMany(d => d.AmizadesA)
		//		.HasForeignKey(ad => ad.IdAlunoB)
		//		.OnDelete(DeleteBehavior.Restrict);
		//}
	}
}
