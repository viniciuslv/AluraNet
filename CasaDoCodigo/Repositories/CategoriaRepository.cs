using CasaDoCodigo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public interface ICategoriaRepository
    {
        Categoria GetCategoria(string nome);
        Categoria AddCategoria(string nome);
    }

    public class CategoriaRepository : BaseRepository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(ApplicationContext contexto) : base(contexto)
        {
        }

        public Categoria AddCategoria(string nome)
        {
            var categoria = GetCategoria(nome);
            if (categoria == null) //Verifica se categoria existe, evita duplicidade lógica
            {
                categoria = new Categoria(nome);
                dbSet.Add(categoria);
            }
            contexto.SaveChanges();
            return categoria;
        }

        public Categoria GetCategoria(string nome)
        {
            return dbSet.Where(c => c.Nome.ToUpper() == nome.ToUpper()).SingleOrDefault();
        }

    }
}
