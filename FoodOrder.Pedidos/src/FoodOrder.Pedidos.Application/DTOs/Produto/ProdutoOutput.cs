namespace FoodOrder.Pedidos.Application.DTOs.Produto
{
    public class ProdutoOutput(int id, string nome, string descricao)
    {
        public int Id { get; set; } = id;
        public string Nome { get; set; } = nome;
        public string Descricao { get; set; } = descricao;
    }
}
