using FoodOrder.Pedidos.Application.DTOs.Pedidos;
using FoodOrder.Pedidos.Application.DTOs.PedidoStatus;
using FoodOrder.Pedidos.Application.UseCase.Pedidos.Interface;
using FoodOrder.Pedidos.Domain.Enums;
using FoodOrder.Pedidos.Domain.Messaging;
using MediatR;

namespace FoodOrder.Pedidos.Application.Feature.Pedidos
{
    public class UpdateStatusPagamentoCommand(int numeroPedido, PagamentoStatusEnum status) : IRequest<Unit>
    {
        public int NumeroPedido { get; set; } = numeroPedido;
        public PagamentoStatusEnum Status { get; set; } = status;
    }

    public class UpdateStatusPagamentoCommandHandler : IRequestHandler<UpdateStatusPagamentoCommand, Unit>
    {
        private readonly IPedidoUseCase _pedidoUseCase;
        private readonly ISqsMessageSender _sqs;

        public UpdateStatusPagamentoCommandHandler(IPedidoUseCase pedidoUseCase, ISqsMessageSender sqs)
        {
            _pedidoUseCase = pedidoUseCase;
            _sqs = sqs;
        }

        public async Task<Unit> Handle(UpdateStatusPagamentoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                PedidoOutput pedido = await ConsultarPedido(request);

                pedido.SetPagamentoStatus(request.Status);

                await _pedidoUseCase.AtualizarStatusPagamento(pedido);

                await EnviarPedidoParaProdução(pedido);

                return Unit.Value;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task EnviarPedidoParaProdução(PedidoOutput pedido)
        {
            if (pedido.PagamentoStatus == PagamentoStatusEnum.PagamentoRealizado)
            {
                await _sqs.EnviarMensagemAsync(pedido, "producao");
            }
        }

        private async Task<PedidoOutput> ConsultarPedido(UpdateStatusPagamentoCommand request)
        {
            var pedido = await _pedidoUseCase.Consultar(request.NumeroPedido);

            return pedido ?? throw new KeyNotFoundException("Pedido não encontrado!");
        }
    }
}
