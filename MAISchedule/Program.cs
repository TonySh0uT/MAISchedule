using System;
using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace MAI_Schedule
{
    internal class Program
    {

        static void Main(string[] args)
        {
            void Start()
            {
                var firefoxOptions = new OpenQA.Selenium.Firefox.FirefoxOptions();
                firefoxOptions.AddArguments("--incognito");
                firefoxOptions.AddArguments("--headless");

                firefoxOptions.AcceptInsecureCertificates = true;
                FirefoxProfile myprofile = new FirefoxProfile();
                var firefoxDriverService = FirefoxDriverService.CreateDefaultService();
                IWebDriver browser = new FirefoxDriver(firefoxDriverService, firefoxOptions);
                browser.Navigate()
                    .GoToUrl($"https://mai.ru/education/studies/schedule/index.php?group=М8О-205Б-20");

                var chooseWeekBtn = browser.FindElement(By.XPath("/html[1]/body[1]/main[1]/div[1]/div[1]/div[1]/article[1]/div[2]/div[1]/a[2]"));
                chooseWeekBtn.Click();
                Thread.Sleep(1000);
                string code = browser.FindElement(By.XPath("/html/body/main/div/div/div[1]/article/div[3]")).Text;
                int numberOfWeeks = int.Parse(code[code.Length - 27].ToString() + code[code.Length - 26].ToString()); ;

                

                



                //Обработчик недель

                DateTime today = DateTime.Now.Date.AddDays(1);
                DateTime beginDate, endDate;
                int week = 0;

                for (int i = 0; i < numberOfWeeks; i++)
                {
                    week = int.Parse(code.Substring(0, code.IndexOf("\r")));
                    code = code.Remove(0, code.IndexOf("\r") + 2);
                    beginDate = Convert.ToDateTime(code.Substring(0, 10));
                    code = code.Remove(0, 13);
                    endDate = Convert.ToDateTime(code.Substring(0, 10));
                    if (i != numberOfWeeks - 1) code = code.Remove(0, 12);

                    
                    if (today >= beginDate && today < endDate) break;
                }
                if (week == 0) Console.WriteLine("Ты долбоеб, у тебя каникулы (либо расписания еще нет)");
                else
                    

                browser.Navigate()
                    .GoToUrl($"https://mai.ru/education/studies/schedule/index.php?group=М8О-205Б-20&week=" + week);
                
                code = browser.FindElement(By.XPath("/html/body/main/div/div/div[1]/article/ul")).Text;
                Console.WriteLine(code);
                code = code.Replace(" января", ".01");
                code = code.Replace(" февраля", ".02");
                code = code.Replace(" марта", ".03");
                code = code.Replace(" апреля", ".04");
                code = code.Replace(" мая", ".05");
                code = code.Replace(" июня", ".06");
                code = code.Replace(" июля", ".07");
                code = code.Replace(" августа", ".08");
                code = code.Replace(" сентября", ".09");
                code = code.Replace(" октября", ".10");
                code = code.Replace(" ноября", ".11");
                code = code.Replace(" декабря", ".12");

                
                  


                string finalMsg = "Расписание на завтра:\n\n";
                string buf;

                if (code.IndexOf(today.ToString("dd/MM")) != -1)
                {
                    code = code.Remove(0, code.IndexOf(today.ToString("dd/MM")) + 7);
                    if (code.IndexOf(", ") != -1)
                        code = code.Remove(code.IndexOf(", ") - 3, code.Length - code.IndexOf(", ") + 3);
                    bool chk = false;
                    while (code.IndexOf('\n') != -1)
                    {
                        buf = code.Substring(0, code.IndexOf("\r"));
                        int i = 0;
                        int spaces = 0;
                        while (i < buf.Length - 1 && buf[i] != '\r')
                        {
                            if (buf[i] == ' ' && spaces < 2)
                                spaces++;
                            else if (buf[i] == ' ' && chk) { buf = buf.Remove(i, buf.Length - i); break; }
                            i++;
                        }
                        finalMsg += buf;
                        if (chk) { finalMsg += "\n"; chk = false; }
                        else chk = true;
                        if (code.IndexOf('\r') != -1) code = code.Remove(0, code.IndexOf('\r') + 1);
                    }
                }
                Console.WriteLine(finalMsg);
                browser.Close();
            }

            Start();
            while (true)
            {
                if (DateTime.Now.ToString("HH:mm:ss") == "23:33:40" || DateTime.Now.ToString("HH:mm:ss") == "21:07:40") Start();
            }
            
        }
    }
}
