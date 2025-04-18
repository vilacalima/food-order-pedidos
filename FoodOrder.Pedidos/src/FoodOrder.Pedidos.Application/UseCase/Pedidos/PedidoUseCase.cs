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
        private readonly ISacolaProdutoRepository _sacolaProdutoRepository;
        private readonly IProdutoHttpService _produtoService;

        public PedidoUseCase(IPedidoRepository pedidosRepository, 
                            ISacolaProdutoRepository sacolaProdutoRepository,
                            IProdutoHttpService produtoService)
        {
            _pedidosRepository = pedidosRepository;
            _sacolaProdutoRepository = sacolaProdutoRepository;
            _produtoService = produtoService;
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

        #region Private Methods
        private async Task<PedidoOutput> BuildPedidoOutput(Pedido pedido)
        {
            var pedidoOutput = PedidoMapper.Map(pedido);

            var sacolasProdutos = await _sacolaProdutoRepository.ConsultarPorSacola(pedido.SacolaId);

            foreach (var SacolaProduto in sacolasProdutos)
            {
                var produtoBase = await _produtoService.ObterProdutoPorIdAsync(SacolaProduto.ProdutoId);

                if (produtoBase != null) // Ensure produtoBase is not null before adding
                {
                    pedidoOutput.Produtos.Add(produtoBase);
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

        #endregion
    }
}
