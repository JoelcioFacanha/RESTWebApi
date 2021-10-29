using DevIO.Business.Interfaces;
using DevIO.Business.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace DevIO.Api.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        private readonly INotificador _notificador;

        protected MainController(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }

        protected ActionResult CustomResponse(object result = null)
        {
            if (OperacaoValida())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = _notificador.ObterNotificacoes().Select(n => n.Mensagem)
            });
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelStateDictonary)
        {
            if (!modelStateDictonary.IsValid) NotificarErroModelInvalida(modelStateDictonary);
            return CustomResponse();
        }

        protected void NotificarErroModelInvalida(ModelStateDictionary modelStateDictonary)
        {
            var errors = modelStateDictonary.SelectMany(e => e.Value.Errors);

            foreach (var error in errors)
            {
                var mensagem = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                NotificarError(mensagem);
            }
        }

        protected void NotificarError(string mensagem)
        {
            _notificador.Handle(new Notificacao(mensagem));
        }
    }
}
