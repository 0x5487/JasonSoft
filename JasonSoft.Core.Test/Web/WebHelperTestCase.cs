using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using JasonSoft.Web;
using JasonSoft.Web.Security.AntiXss;
using Xunit;

namespace JasonSoft.Tests.Web
{

    public class WebHelperTestCase
    {
        [Fact]
        public void HtmlEncodePerformanceTest()
        {
            String source = @"abcdefghijklmnopqrstuvwxyz1234567890!#$%^&*()_-=+[]{};':,<>?
            (中央社記者馮昭台北十二日電)中央氣象局預報中心預報課
                            長蔡甫甸表示，強烈颱風辛樂克結構好，只要颱風中心接近東北角就足以對台灣造成災害，目前不排除颱風中心登陸。由於颱風移動緩慢，過去一至二小時幾乎呈
                            打轉現象，不僅中秋節賞月確定泡湯，下週一 (十五日)仍要注意颱風的影響。蔡甫甸說，颱風暴風圈已開始接觸台灣陸地，下午各地風雨逐漸增強，位於山區的
                            桃園縣復興鄉累積雨量已達一百五十毫米。入夜後風會更強，明天清晨在宜蘭、基隆和台北將會出現十三級強陣風，花蓮最大陣風將達十二級，台東、桃園、新竹
                            、苗栗、台中和南投也可能出現十一級強陣風。The .NET Framework uses the Char structure to represent a Unicode character. The Unicode Standard identifies each Unicode character with a unique 21-bit scalar number called a code point, and defines the UTF-16 encoding form that specifies how a code point is encoded into a sequence of one or more 16-bit values. Each 16-bit value ranges from hexadecimal 0x0000 through 0xFFFF and is stored in a Char structure. The value of a Char object is its 16-bit numeric (ordinal) value.";

            //String source = "(桃園縣復興鄉累積雨量已)";

            Stopwatch sw = new Stopwatch();
            sw.Start();
            Int32 length = 0;
            for (int i = 0; i < 10000; i++)
            {
                //Console.WriteLine(WebHelper.EncodeHtml1("陣"));
                //length = WebHelper.EncodeHtml1(source).Length;
                //SampleStringBuilder(source);
            }

            sw.Stop();
            Console.WriteLine(String.Format("Microsoft AntiXss: {0}, Length: {1}", sw.Elapsed, length));

            sw.Reset();
            sw.Start();

            for (int i = 0; i < 10000; i++)
            {
                length = AntiXss.HtmlEncode(source).Length;
                //SampleCharArray(source);
            }

            sw.Stop();
            Console.WriteLine(String.Format("JasonSoft AntiXss: {0}, length: {1}", sw.Elapsed, length));

        }


        public static string SampleStringBuilder(string text)
        {

            // Declares new string builder and filters the input to allow alphanumeric string

            StringBuilder builder = new StringBuilder("", text.Length);



            foreach (char c in text)
            {

                if ((((uint)c > 96) && ((uint)c < 123)) ||    // "a-z"

                    (((uint)c > 64) && ((uint)c < 91)) ||    // "A-Z"

                    (((uint)c > 47) && ((uint)c < 58))      // "0-9"

                   )
                {

                    // add alphanumeric character to builder

                    builder.Append(c);

                }

            }

            return builder.ToString();

        }

        public static string SampleCharArray(string text)
        {

            // Use a new char array.

            char[] buffer = new char[text.Length];

            int lenIndex = 0;



            foreach (char c in text)
            {

                // Check for alphanumeric character.

                if ((((uint)c > 96) && ((uint)c < 123)) ||    // "a-z"

                   (((uint)c > 64) && ((uint)c < 91)) ||    // "A-Z"

                   (((uint)c > 47) && ((uint)c < 58))      // "0-9"

                    )
                {

                    // add alphanumeric character to char array

                    buffer[lenIndex++] = c;

                }



            }

            return new string(buffer);

        }

        [Fact]
        public void CrossSite()
        {
            String script = "<span onclick=\"javascript:alert('Hello');\">click me</span><span onclick=\"javascript:alert('Hello');\">click me</span>";
            String script1 = "<a href=\"p.aspx\">Jason</a><iframe></iframe>";
            String result = AntiXss.GetSafeHtmlFragment(script1);
            Console.WriteLine(result);
        }

        [Fact]
        public void HtmlEncodeTest() 
        {
            string content = "資安之眼";
            string result = AntiXss.GetSafeHtmlFragment(content);
        }
    }


}
