using Calculator.Common.Models;
using Calculator.Common.Utilities;
using Calculator.WebAPP.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Research.SEAL;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;


namespace Calculator.WebAPP.Controllers
{
    public class HomeController : Controller
    {
        
        CalculatorAPI _api=new CalculatorAPI();
       
        Utilities utilities=new Utilities();
        
        private static HttpClient _httpClient=new HttpClient();
        private readonly ILogger _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            Data data = new Data();
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Index(Data data)
        {


            ulong x = data.Value1;
            Plaintext xPlain = new Plaintext(utilities.ULongToString(x));
            

            ulong y = data.Value2;
            Plaintext yPlain = new Plaintext(utilities.ULongToString(y));
            

            Ciphertext xEncrypted = new Ciphertext();
            utilities.Encryptor.Encrypt(xPlain, xEncrypted);
            
            Ciphertext yEncrypted = new Ciphertext();
            utilities.Encryptor.Encrypt(yPlain, yEncrypted);
            _logger.LogInformation("IN WEB APP");
            _logger.LogInformation("xEncrypted=" + xEncrypted.ToString());
            _logger.LogInformation("yEncrypted=" + yEncrypted.ToString());

            Ciphertext encryptedResult = new Ciphertext();
            
            string xEncryptedString=utilities.CiphertextToBase64String(xEncrypted);
            string yEncryptedString=utilities.CiphertextToBase64String(yEncrypted);
            _logger.LogInformation("xEncryptedString=" + xEncryptedString);
            _logger.LogInformation("yEncryptedString=" + yEncryptedString);
            //EncodedData encodedData= _calculatorService.PostInput(xEncryptedString, yEncryptedString);
            EncodedData encodedData = new EncodedData() { Value1 = xEncryptedString, Value2 = yEncryptedString };
                        
            HttpClient client = _api.Initial();


            StringContent content = new StringContent(JsonConvert.SerializeObject(encodedData), Encoding.UTF8, "application/json");

            HttpResponseMessage res =  await client.PostAsync("api/evaluator", content);

            //HttpResponseMessage res = await client.GetAsync("api/evaluator/" + encodedData.Id);

            Result encodedResultString;
            //string resultString;
            if (res.IsSuccessStatusCode)
            {
                var result = await res.Content.ReadAsStringAsync();
                encodedResultString=JsonConvert.DeserializeObject<Result>(result);
                //resultString= _calculatorService.GetResult(encodedData.Id);
                //encryptedResult = utilities.BuildCiphertextFromBase64String(resultString, utilities.SEALContext);
                encryptedResult = utilities.BuildCiphertextFromBase64String(encodedResultString.EncodedStringResult, utilities.SEALContext);
            }
            Plaintext decryptedResult = new Plaintext();
            
            utilities.Decryptor.Decrypt(encryptedResult, decryptedResult);
            

            int reslt = int.Parse(decryptedResult.ToString(), System.Globalization.NumberStyles.HexNumber);
            return RedirectToAction("Result", new { result = reslt });
        }
        [HttpGet]
        public IActionResult Result(int result)
        {


            ViewBag.Result = result;
            return View();
        }
        [HttpGet]
        public IActionResult Weekly()
        {
            Week week = new Week();
            return View(week);
        }
    }
}
