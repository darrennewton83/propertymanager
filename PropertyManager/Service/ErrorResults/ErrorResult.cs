using AutoMapper;

namespace Service.ErrorResults
{
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// A custom action result for sending error information to the client
    /// </summary>
    public class ErrorResult : ActionResult, IActionResult
    {
        private string _errorMessage;
        private IMapper _mapper;

        /// <summary>
        /// Initialises a new instance of the <see cref="ErrorResult"/> class.
        /// </summary>
        /// <param name="ErrorMessage">The error message to send to the client</param>
        /// <param name="mapper">An automapper instance</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ErrorResult (string errorMessage, IMapper mapper)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new ArgumentNullException(errorMessage);
            }

            _errorMessage = errorMessage;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(Mapper));
        }

        /// <inheritdoc />
        public override void ExecuteResult(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var error = new ErrorMessage(_errorMessage);
            var dto = _mapper.Map<ErrorMessageDto>(error);
            context.HttpContext.Response.WriteAsJsonAsync(dto);
        }
    }
}
