using FoodOrder.Pedidos.Application.DTOs.Pedidos;
using FoodOrder.Pedidos.Application.DTOs.PedidoStatus;
using FoodOrder.Pedidos.Application.DTOs.Produto;
using FoodOrder.Pedidos.Application.Interfaces;
using FoodOrder.Pedidos.Application.Mapper;
using FoodOrder.Pedidos.Application.UseCase.Pedidos.Interface;
using FoodOrder.Pedidos.Domain.Entities;
using FoodOrder.Pedidos.Domain.Enums;
using FoodOrder.Pedidos.Domain.Repository;

namespace FoodOrder.Pedidos.Application.UseCase.Pedidos
{
    public class PedidoUseCase : IPedidoUseCase
    {
        private readonly IPedidoRepository _pedidosRepository;
        private readonly ISacolaRepository _sacolaRepository;
        private readonly ISacolaProdutoRepository _sacolaProdutoRepository;
        private readonly IProdutoHttpService _produtoService;

        public PedidoUseCase(IPedidoRepository pedidosRepository, 
                            ISacolaProdutoRepository sacolaProdutoRepository,
                            IProdutoHttpService produtoService,
                            ISacolaRepository sacolaRepository)
        {
            _pedidosRepository = pedidosRepository;
            _sacolaProdutoRepository = sacolaProdutoRepository;
            _produtoService = produtoService;
            _sacolaRepository = sacolaRepository;
        }

        public async Task<PedidosOutput> ListarPedidos()
        {
            List<PedidoOutput> pedidosOutput = new List<PedidoOutput>();
            var pedidos = await _pedidosRepository.ListarPedidos();

            foreach (var item in pedidos)
            {
                PedidoOutput pedidoOutput = await BuildPedidoOutput(item);

                pedidosOutput.Add(pedidoOutput);
            }
            
            return OrdenarPedidos(pedidosOutput);
        }

        public async Task<PedidoOutput> Consultar(int numeroPedido)
        {
            var pedido = await _pedidosRepository.ConsultarPedidoPorNumero(numeroPedido);

            return pedido == null
                ? throw new ArgumentNullException(nameof(numeroPedido), "Pedido não encontrado!")
                : await BuildPedidoOutput(pedido);
        }

        public async Task Atualizar(PedidoOutput pedidoAtualizado)
        {
            var pedidoRepositorio = await _pedidosRepository.ConsultarPedidoPorNumero(pedidoAtualizado.NumeroPedido) 
                ?? throw new ArgumentNullException(nameof(pedidoAtualizado.NumeroPedido), "Pedido não encontrado!");
            
            var pedido = PedidoMapper.Map(pedidoAtualizado, pedidoRepositorio.NumeroPedido);
            await _pedidosRepository.Atualizar(pedido);
        }

        public async Task AtualizarStatusPedido(PedidoOutput pedidoAtualizado)
        {
            if (pedidoAtualizado == null || pedidoAtualizado.Id <= 0) return;

            await _pedidosRepository.AtualizarStatusPedido(pedidoAtualizado.Id, pedidoAtualizado.PedidoStatus);
        }

        public async Task AtualizarStatusPagamento(PedidoOutput pedidoAtualizado)
        {
            if (pedidoAtualizado == null || pedidoAtualizado.Id <= 0) return;

            await _pedidosRepository.AtualizarStatusPagamento(pedidoAtualizado.Id, pedidoAtualizado.PagamentoStatus);
        }

        public async Task<PedidoDto> CriarNovoPedido(List<int> produtos, Guid ClienteId)
        {
            var cadastrarSacola = await _sacolaRepository.Cadastrar(new Sacola());

            await CadastraProdutoNaSacola(produtos, cadastrarSacola);

            var produto = await BuscarProdutos(produtos);

            var tempoPreparo = CalcularTempoPreparoParalelo(produto);

            var pedido = new Pedido(tempoPreparo, ClienteId, PagamentoStatusEnum.AguardandoPagamento, PedidoStatusEnum.Recebido, cadastrarSacola.Id);

            var pedidoCriado = await _pedidosRepository.Cadastrar(pedido);

            var precoTotal = produto.Sum(x => x.Preco);

            return new PedidoDto(numeroPedido: pedidoCriado, preco: precoTotal);
        }

        #region Private Methods
        private async Task<PedidoOutput> BuildPedidoOutput(Pedido pedido)
        {
            var pedidoOutput = PedidoMapper.Map(pedido);

            var sacolasProdutos = await _sacolaProdutoRepository.ConsultarPorSacola(pedido.SacolaId);

            foreach (var SacolaProduto in sacolasProdutos)
            {
                var produtoBase = await _produtoService.ObterProdutoPorIdAsync(SacolaProduto.ProdutoId);

                if (produtoBase != null)
                {
                    var mapperProduto = ProdutoMapper.Map(produtoBase);
                    pedidoOutput.Produtos.Add(mapperProduto);
                }
            }

            return pedidoOutput;
        }

        private static PedidosOutput OrdenarPedidos(List<PedidoOutput> pedidos)
        {
            PedidosOutput pedidosOutput = new PedidosOutput();
            
            pedidos = pedidos.OrderBy(x => x.DataCriacao).ToList();

            foreach (var item in pedidos)
            {
                if (item.PedidoStatus == PedidoStatusEnum.Pronto)
                {
                    pedidosOutput.Pronto.Add(item);
                }
                else if (item.PedidoStatus == PedidoStatusEnum.EmPreparacao)
                {
                    pedidosOutput.EmPreparacao.Add(item);
                }
                else if (item.PedidoStatus == PedidoStatusEnum.Recebido)
                {
                    pedidosOutput.Recebido.Add(item);
                }
                else if (item.PedidoStatus == PedidoStatusEnum.Finalizado)
                {
                    pedidosOutput.Finalizado.Add(item);
                }
            }

            return pedidosOutput;
        }

        private async Task<List<ProdutoDto>> BuscarProdutos(List<int> produtos)
        {
            var listaProdutos = new List<ProdutoDto>();

            foreach (var produtoId in produtos)
            {
                var produtoBase = await _produtoService.ObterProdutoPorIdAsync(produtoId)
                    ?? throw new ArgumentNullException(nameof(produtos), "Produto não encontrado!");

                listaProdutos.Add(produtoBase);
            }

            return listaProdutos;
        }

        private TimeSpan CalcularTempoPreparoParalelo(List<ProdutoDto> produtos)
        {
            var grupos = produtos.GroupBy(p => p.Tipo);
            var tempoTotal = grupos.Sum(g => g.Max(p => p.TempoPreparo));

            return TimeSpan.FromMinutes(tempoTotal);
        }

        private async Task CadastraProdutoNaSacola(List<int> produtos, Sacola cadastrarSacola)
        {
            foreach (var produtoId in produtos)
            {
                var sacolaProduto = new SacolaProduto(cadastrarSacola.Id, produtoId);
                await _sacolaProdutoRepository.Cadastrar(sacolaProduto);
            }
        }
        #endregion
    }
}
