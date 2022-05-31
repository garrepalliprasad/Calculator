using Calculator.Common.Models;
using Calculator.Common.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Research.SEAL;
namespace Calculator.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluatorController : ControllerBase
    {
        private readonly ICalculatorService _calculatorService;
        private readonly ILogger _logger;

        public EvaluatorController(ICalculatorService calculatorService , ILogger<EvaluatorController> logger)
        {
            _calculatorService=calculatorService;
            _logger=logger;
        }
        [HttpGet]
        [Route("")]
        public string Index()
        {
            return "Hello From API";
        }
        [HttpPost]
        [Route("")]
        public IActionResult Add([FromBody] EncodedData data)
        {
            _logger.LogInformation("In WebAPI");
            Ciphertext xyEncryptedResult=new Ciphertext();
            Utilities utilities = new Utilities();
            SEALContext context = utilities.SEALContext;
            Ciphertext xEncryptedCiphertext = utilities.BuildCiphertextFromBase64String(data.Value1, context);
            Ciphertext yEncryptedCiphertext = utilities.BuildCiphertextFromBase64String(data.Value2, context);
            utilities.Evaluator.Add(xEncryptedCiphertext, yEncryptedCiphertext, xyEncryptedResult);
            string xyEncryptedResultString=utilities.CiphertextToBase64String(xyEncryptedResult);
            data.Result = xyEncryptedResultString;
            _calculatorService.PostEncodedData(data);
            Result result = new Result() { EncodedStringResult=xyEncryptedResultString};
            _logger.LogInformation("xyEncryptedResult" + xyEncryptedResult.ToString());
            _logger.LogInformation("xyEncryptedResultString" + xyEncryptedResultString);
            return Ok(result);
        }
        /*[HttpGet]
        [Route("{id:int}")]
        public IActionResult Add(int id)
        {
            EncodedData encodedData=_calculatorService.GetInput(id);
            Ciphertext xyEncryptedResult = new Ciphertext();
            Utilities utilities = new Utilities();
            SEALContext context = utilities.SEALContext;
            Ciphertext xEncryptedCiphertext = utilities.BuildCiphertextFromBase64String(encodedData.Value1, context);
            Ciphertext yEncryptedCiphertext = utilities.BuildCiphertextFromBase64String(encodedData.Value2, context);
            utilities.Evaluator.Add(xEncryptedCiphertext, yEncryptedCiphertext, xyEncryptedResult);
            string xyEncryptedResultString = utilities.CiphertextToBase64String(xyEncryptedResult);
            _calculatorService.UpdateResult(id,xyEncryptedResultString);
            //Result result = new Result() { EncodedStringResult = xyEncryptedResultString };
            return Ok();
        }*/
    }
}
