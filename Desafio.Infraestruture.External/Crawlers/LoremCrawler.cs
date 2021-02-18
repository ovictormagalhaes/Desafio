using Desafio.Core.Entities;
using Desafio.Core.Interface.Crawlers;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Infraestruture.External.Crawlers
{
    public class LoremCrawler : ILoremCrawler
    {
        private const string URL = "https://www.loremipzum.com/pt/gerador-de-texto";
        private readonly ILogger _logger;
        private readonly IWebDriver _driver;
        private readonly IJavaScriptExecutor _js;

        public LoremCrawler(ILogger<LoremCrawler> logger)
        {
            _logger = logger;
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            _driver = new ChromeDriver(Environment.CurrentDirectory, options);
            _driver.Navigate().GoToUrl(URL);
            _js = (IJavaScriptExecutor)_driver;
        }

        public async Task<LoremEntity> GetData()
        {
            try
            {
                var generate = new WebDriverWait(_driver, TimeSpan.FromSeconds(2000)).Until(d => d.FindElement(By.Id("generate")));
                
                generate.Click();

                var div = new WebDriverWait(_driver, TimeSpan.FromSeconds(2000)).Until(d => d.FindElement(By.Id("outputtext")));
                var paragraphs = new WebDriverWait(_driver, TimeSpan.FromSeconds(2000)).Until(d => div.FindElements(By.TagName("p")));

                string text = paragraphs.Select(x => x.Text).Aggregate((i, j) => i + j);
                return new LoremEntity() { Text = text };
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
