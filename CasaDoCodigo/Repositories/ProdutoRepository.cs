using CasaDoCodigo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public class ProdutoRepository : BaseRepository<Produto>, IProdutoRepository
    {
        private readonly ICategoriaRepository categoriaRepository;

        public ProdutoRepository(ApplicationContext contexto,
                                ICategoriaRepository categoriaRepository) : base(contexto)
        {
            this.categoriaRepository = categoriaRepository;
        }

        public IList<Produto> GetProdutos()
        {
            return dbSet.Include(p=>p.Categoria).ToList();
        }

        public IList<Produto> GetProdutos(string pesquisa)
        {
            //Se pesquisa vazia retorna todos os produtos
            if (string.IsNullOrEmpty(pesquisa))
            {
                return GetProdutos();
            }
            var consulta = dbSet.Include(p => p.Categoria).Where(p => p.Nome.Contains(pesquisa) || p.Categoria.Nome.Contains(pesquisa)).ToList();
            //Se pesquisa não retornou nada, retorna todos os produtos
            if (consulta.Count==0)
            {
                return GetProdutos();
            }
            return consulta;
        }

        public async Task SaveProdutos(List<Livro> livros)
        {
            foreach (var livro in livros)
            {
                if (!dbSet.Where(p => p.Codigo == livro.Codigo).Any())
                {
                    dbSet.Add(new Produto(livro.Codigo, livro.Nome, livro.Preco, categoriaRepository.AddCategoria(livro.Categoria)));
                }
            }
            await contexto.SaveChangesAsync();
        }
    }

    public class Livro
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Categoria { get; set; }
        public string Subcategoria { get; set; }
        public decimal Preco { get; set; }
    }
}
