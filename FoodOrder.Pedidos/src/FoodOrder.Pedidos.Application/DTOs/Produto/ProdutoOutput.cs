namespace FoodOrder.Pedidos.Application.DTOs.Produto
{
    public class ProdutoOutput
    {
        public ProdutoOutput()
        {
            Nome = string.Empty;
            Descricao = string.Empty;
        }

        public ProdutoOutput(int id, string nome, string descricao)
        {
            Id = id;
            Nome = nome;
            Descricao = descricao;
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }
}
