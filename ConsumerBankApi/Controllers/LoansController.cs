using ConsumerBank.Services;
using ConsumerBank.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ConsumerBankApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly ILogger<LoansController> _logger;
        private readonly ILoanerService _loanerService;

        public LoansController(ILogger<LoansController> logger, ILoanerService loanerService)
        {
            _logger = logger;
            _loanerService = loanerService;
        }

        [HttpPost]
        [Route("Apply")]
        public async Task<IActionResult> ApplyForLoan([FromBody]LoanRequest request)
        {
            _logger.Log(LogLevel.Information, "Got application");
            var approved = await _loanerService.Apply(request);
            return Ok(approved);
        }

        [HttpGet]
        public IActionResult Ping()
        {
            _logger.Log(LogLevel.Information, "Ping");
            return Ok("Hello world");
        }
    }
}