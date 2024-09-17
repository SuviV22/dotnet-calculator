using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using dotnet_calculator.Models;
using System.IO.Pipelines;
using System.Xml.Linq;

namespace dotnet_calculator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost("/test/Calculate")]
        public IActionResult Calculate([FromBody] string operation)
        {
            List<string> operationArr = new();
            char[] characters = operation.ToCharArray();
            var startIndex = 0;
            var result = "";

            foreach (var item in characters.Select((value, i) => new { i, value }))
            {
                if(item.value == '+' || item.value == '-' || item.value == '*' || item.value == '/')
                {
                    operationArr.Add(operation.Substring(startIndex, item.i - startIndex));
                    operationArr.Add(item.value.ToString());
                    startIndex = item.i + 1;
                }
            }
            operationArr.Add(operation.Substring(startIndex, operation.Length - startIndex));

            if (operationArr.Count > 1)
            {
                var resultCalculations = Calculations(operationArr);
                result = resultCalculations[0];
            }

            return Json(new { ans = result });
        }

        private static List<string> Calculations(List<string> operationArr)
        {
            if (operationArr.Count > 1)
            {
                if (operationArr.Contains("/") || operationArr.Contains("*"))
                {
                    var indexDivide = operationArr.IndexOf("/");
                    var indexMultiply = operationArr.IndexOf("*");

                    if (indexDivide >=0 && (indexDivide < indexMultiply || indexMultiply < 0))
                    {
                        try
                        {
                            var indexOfSymbol = indexDivide;
                            int left = Int32.Parse(operationArr[indexOfSymbol - 1]);
                            int right = Int32.Parse(operationArr[indexOfSymbol + 1]);

                            var ans = left / right;
                            operationArr.RemoveRange(indexOfSymbol - 1, 3);
                            operationArr.Insert(indexOfSymbol - 1, ans.ToString());

                            return Calculations(operationArr);
                        } 
                        catch 
                        {
                            operationArr = new List<string> { "error" };
                            return operationArr;
                        }
                    }

                    if (indexMultiply >= 0 && (indexMultiply < indexDivide || indexDivide < 0))
                    {
                        var indexOfSymbol = indexMultiply;
                        int left = Int32.Parse(operationArr[indexOfSymbol - 1]);
                        int right = Int32.Parse(operationArr[indexOfSymbol + 1]);

                        var ans = left * right;
                        operationArr.RemoveRange(indexOfSymbol - 1, 3);
                        operationArr.Insert(indexOfSymbol - 1, ans.ToString());

                        return Calculations(operationArr);
                    }
                }

                if (operationArr.Contains("+") || operationArr.Contains("-"))
                {
                    var indexPlus = operationArr.IndexOf("+");
                    var indexSub = operationArr.IndexOf("-");

                    if (indexPlus >= 0 && (indexPlus < indexSub || indexSub < 0))
                    {
                        var indexOfSymbol = operationArr.IndexOf("+");
                        int left = Int32.Parse(operationArr[indexOfSymbol - 1]);
                        int right = Int32.Parse(operationArr[indexOfSymbol + 1]);

                        var ans = left + right;
                        operationArr.RemoveRange(indexOfSymbol - 1, 3);
                        operationArr.Insert(indexOfSymbol - 1, ans.ToString());

                        return Calculations(operationArr);
                    }

                    if (indexSub >= 0 && (indexSub < indexPlus || indexPlus < 0))
                    {
                        var indexOfSymbol = operationArr.IndexOf("-");
                        int left = Int32.Parse(operationArr[indexOfSymbol - 1]);
                        int right = Int32.Parse(operationArr[indexOfSymbol + 1]);

                        var ans = left - right;
                        operationArr.RemoveRange(indexOfSymbol - 1, 3);
                        operationArr.Insert(indexOfSymbol - 1, ans.ToString());

                        return Calculations(operationArr);
                    }
                }
            }

            return operationArr;
        }
    }
}