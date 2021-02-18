using Desafio.Core.Entities;
using Desafio.Core.Interface.Crawlers;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Desafio.Infraestruture.External.Crawlers
{
    public class MotherEffCrawler : IMotherEffCrawler
    {
        private const string URL = "https://mothereff.in/byte-counter";
        private readonly ILogger<MotherEffCrawler> _logger;
        private readonly IWebDriver _driver;
        private readonly IJavaScriptExecutor _js;

        public MotherEffCrawler(ILogger<MotherEffCrawler> logger)
        {
            _logger = logger;
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            _driver = new ChromeDriver(Environment.CurrentDirectory, options);
            _driver.Navigate().GoToUrl(URL);
            _js = (IJavaScriptExecutor)_driver;
        }

        public async Task<MotherEffEntity> GetData(string text)
        {
            try
            {
                var textArea = new WebDriverWait(_driver, TimeSpan.FromSeconds(2000)).Until(d => d.FindElement(By.TagName("textarea")));
                string scriptClear = "document.getElementsByTagName('textarea')[0].value = '';";
                _js.ExecuteScript(scriptClear);

                string scriptSend = "document.getElementsByTagName('textarea')[0].value = '" + text + "';";
                _js.ExecuteScript(scriptSend);
                textArea.SendKeys(" ");
                textArea.SendKeys("\b");

                var textBytes = new WebDriverWait(_driver, TimeSpan.FromSeconds(3000)).Until(d => d.FindElement(By.Id("bytes")).Text);
                string value = Regex.Replace(textBytes, @"[^\d]", "");
                var bytes = long.Parse(value);

                if (bytes <= 10 && !string.IsNullOrEmpty(text))
                    throw new Exception("Invalid Count: " + textBytes + "\nBytes:" + bytes);

                return new MotherEffEntity() { Bytes = bytes };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
