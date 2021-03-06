using System;
using System.Text;
using System.Web;
using System.Data;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Utility
{
    /// <summary>
    /// StringHelper 用于字符串的公用方法
    /// </summary>
    public class StringCore
    {

        public static string IsStrFormat(string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                return str;
            }
            return Config.GetStrFormat;
        }

        /// <summary>
        /// 将int 的集合转化成 类似“1,2,3,4”这样的格式
        /// </summary>
        /// <returns></returns>
        public static string ConvertListToStr(List<int> ids)
        {
            var result = string.Empty;
            ids.All(id =>
            {
                result += id.ToString() + ",";
                return true;
            });

            result = RegexCore.Replace(result, ",$", "");
            return result;
        }
        /// <summary>
        /// 获得规范的strArray 1,2,3,4 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetStrArray(string str)
        {

            var result = string.Empty;
            if (string.IsNullOrEmpty(str))
            {
                result = string.Empty;
            }
            else
            {
                foreach (Match m in RegexCore.Matches(str, @"(?<content>(\d+,?)+)"))
                {
                    var tempStr = m.Groups["content"].Value;
                    if (!string.IsNullOrEmpty(tempStr))
                    {
                        tempStr.Split(',').All(id =>
                        {
                            if (id.Length > 0)
                            {
                                result += id + ",";
                            }
                            return true;
                        });
                    }
                }
                result = RegexCore.Replace(result, ",$", "");
            }
            return result;
        }


        /// <summary>
        /// 银行卡验证算法。
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool Luhn(long n)
        {
            long nextdigit, sum = 0;
            bool alt = false;
            while (n != 0)
            {
                nextdigit = n % 10;
                if (alt)
                {
                    nextdigit *= 2;
                    nextdigit -= (nextdigit > 9) ? 9 : 0;
                }
                sum += nextdigit;
                alt = !alt;
                n /= 10;
            }
            return (sum % 10 == 0);
        }

        public static string GetStringWithOutHtml(string str)
        {
            str = Regex.Replace(str, @"<\/?[^>]*>", "", RegexOptions.Multiline);
            str = Regex.Replace(str, @"&nbsp;", "", RegexOptions.Multiline);

            return str;
        }

        /// <summary>
        /// 替换空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetStringWithOutSpace(string str)
        {
            str = Regex.Replace(str, @"\s", "");
            return str;
        }

        /// <summary>
        /// 获取分页的字符串 ， getPagerStr("/ask/daikuan", "/ask/daikuan-{1}.html", 12, 1000, 10);
        /// </summary>
        /// <param name="urlPageFirst">第一页的完整链接</param>
        /// <param name="urlPattern">其它页的链接</param>
        /// <param name="PageIndex"></param>
        /// <param name="count">总记录条数</param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public static string getPagerStr(string urlPageFirst, string urlPattern, int PageIndex, int count, int PageSize)
        {
            string reStr = "";

            if (PageSize >= count)
                return reStr;

            string TempletStr = TempletStr = "  <a href='" + urlPattern + "' class='pageindex'>{0}</a>  ";
            int ForCount = 9;
            int AllCount = ((count % PageSize) == 0) ? (count / PageSize) : ((count / PageSize) + 1);
            int start = ((PageIndex - 4) < 1) ? 1 : (PageIndex - 4);
            if (AllCount < 10)
            {
                ForCount = AllCount - start + 1;
            }
            start = ((PageIndex + 9) > AllCount && (AllCount - 8) > 0) ? (AllCount - 8) : start;
            start = (PageIndex - 4) > 0 ? (PageIndex - 4) : start;
            if (AllCount < start + 10)
            {
                ForCount = AllCount - start + 1;
            }
            int end = start + ForCount;

            if (start > 1)
            {
                if (urlPageFirst != "")
                    reStr += "  <a href='" + urlPageFirst + "' class='pageindex'>第一页</a>  ";
                else
                    reStr += string.Format(TempletStr, "第一页", "1");
            }
            for (int i = 0; i < ForCount; i++)
            {
                if (start + i == PageIndex)
                {
                    reStr += string.Format("<a href='" + urlPattern + "' class='pageindex seld'>{0}</a>", PageIndex, PageIndex);
                }
                else
                {
                    if (start + i == 1)
                    {
                        if (urlPageFirst != "")
                            reStr += "  <a href='" + urlPageFirst + "' class='pageindex'>" + ((start + i).ToString()) + "</a>  ";
                        else
                            reStr += string.Format(TempletStr, ((start + i).ToString()), (start + i).ToString());
                    }
                    else
                    {
                        reStr += string.Format(TempletStr, ((start + i).ToString()), (start + i).ToString());
                    }
                }
            }
            if (end < AllCount)
            {
                reStr += string.Format(TempletStr, "最后一页", AllCount.ToString());
            }
            return reStr;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public StringCore() { }

        public static bool CompareStr(string str1, string str2)
        {
            return CompareStr(str1, str2, true);
        }

        /// <summary>
        ///  字符串比较方法
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <param name="withCase">是否包含大小写</param>
        /// <returns></returns>
        public static bool CompareStr(string str1, string str2, bool withCase)
        {
            if (!withCase)
                return string.Compare(str1, str2, true) == 0;
            return string.Compare(str1, str2) == 0;
        }


        /// <summary>
        /// 验证字符串是否为纯数字!
        /// </summary>
        /// <param name="s">需要验证的字符串</param>
        /// <returns></returns>
        public static bool IsNum(string s)
        {
            string strReg = "^\\d+$";
            Regex r = new Regex(strReg, RegexOptions.IgnoreCase);
            Match m = r.Match(s);
            return m.Success;
        }
        /// <summary>
        /// 验证字符串是否是浮点类型！
        /// </summary>
        /// <param name="s">需要验证的字符串</param>
        /// <returns></returns>
        public static bool IsFloat(string s)
        {
            string pattern = "^\\d+\\.\\d+$";
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
            Match m = r.Match(s);
            return m.Success;
        }
        /// <summary>
        /// 获取字符串中的字母和数字！
        /// </summary>
        /// <param name="s">需要获取的字符串</param>
        /// <returns></returns>
        public static string GetNLString(string s)
        {
            if (s == null || s == String.Empty)
                return string.Empty;
            return new Regex("[^0-9a-zA-Z]").Replace(s, String.Empty);
        }

        #region 加密算法
        /// <summary>
        /// MD5函数2
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        public static string MD5(string str)
        {
            byte[] b = Encoding.Default.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = String.Empty;
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        }
        /// <summary>
        /// DEC 加密过程 
        /// </summary>
        /// <param name="pToEncrypt"></param>
        /// <param name="sKey"></param>
        /// <param name="sIV"></param>
        /// <returns></returns>
        public static string EncryptHttpString(string pToEncrypt, string sKey, string sIV)
        {
            //把字符串放到byte数组中
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            //建立加密对象的密钥和偏移量
            des.Key = Encoding.ASCII.GetBytes(sKey);
            //原文使用ASCIIEncoding.ASCII方法的GetBytes方法 
            des.IV = Encoding.ASCII.GetBytes(sIV);
            //使得输入密码必须输入英文文本 
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
                ret.AppendFormat("{0:X2}", b);
            ret.ToString();
            return ret.ToString();
        }
        /// <summary>
        /// DEC 解密过程 
        /// </summary>
        /// <param name="pToDecrypt"></param>
        /// <param name="sKey"></param>
        /// <param name="sIV"></param>
        /// <returns></returns>
        public static string DecryptHttpString(string pToDecrypt, string sKey, string sIV)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            des.Key = Encoding.ASCII.GetBytes(sKey); //建立加密对象的密钥和偏移量，此值重要，不能修改 
            des.IV = Encoding.ASCII.GetBytes(sIV);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder(); //建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象 
            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
        #endregion

        /// <summary>
        /// 返回字串的字节长度.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static int GetStringByteLength(string src)
        {
            byte[] sarr = System.Text.Encoding.Default.GetBytes(src);
            return sarr.Length;
        }
        /// <summary>
        /// 将全角字符转换为半角字符
        /// </summary>
        /// <param name="QJstr">源字符串</param>
        /// <returns>源字符串所对应的半角字串</returns>
        public static string TransToDBC(string QJstr)
        {
            if (QJstr.Trim() == "") return "";

            char[] c = QJstr.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                byte[] b = System.Text.Encoding.Unicode.GetBytes(c, i, 1);
                if (b.Length == 2)
                {
                    if (b[1] == 255)
                    {
                        b[0] = (byte)(b[0] + 32);
                        b[1] = 0;
                        c[i] = System.Text.Encoding.Unicode.GetChars(b)[0];
                    }
                }
            }
            string strNew = new string(c);
            return strNew;
        }

        public static string Abbreviate(string s, int length)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            if (s.Length > length)
            {
                return s.Substring(0, length - 1) + "..";
            }

            return s;
        }
        public static string Abbreviate(string s, int length, string ReplaceStr)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            if (s.Length > length)
            {
                return s.Substring(0, length - 1) + ReplaceStr;
            }

            return s;
        }
        public static string AbbreviateMid(string s, int length)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            if (s.Length > length)
            {
                int excess = s.Length - length;

                try
                {
                    string head = s.Substring(0, s.Length / 2 - excess / 2 - 2);
                    string foot = s.Substring(s.Length / 2 + excess / 2 + 2);
                    return head + "..." + foot;
                }
                catch { }
            }
            return s;
        }
        /// <summary>
        /// 获取安全字符串，并限制长度
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string InputText(string text, int maxLength)
        {
            text = text.Trim();
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            if (text.Length > maxLength)
                text = text.Substring(0, maxLength);
            return InputText(text);
        }




        /// <summary>
        /// 显示字符串
        /// </summary>
        /// <param name="text">要截取的字符串</param>
        /// <param name="length">长度</param>
        /// <param name="replace">替换为</param>
        /// <returns></returns>
        public static string substr(string text, int length, string replace)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            text = text.Trim();
            if (text.Length < length)
                return text;
            return text.Substring(0, length) + replace;
        }


        private static string cutSubstring(string s, int length)
        {
            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(s);
            int n = 0;  //  表示当前的字节数
            int i = 0;  //  要截取的字节数
            for (; i < bytes.GetLength(0) && n < length; i++)
            {
                //  偶数位置，如0、2、4等，为UCS2编码中两个字节的第一个字节
                if (i % 2 == 0)
                {
                    n++;      //  在UCS2第一个字节时n加1
                }
                else
                {
                    //  当UCS2编码的第二个字节大于0时，该UCS2字符为汉字，一个汉字算两个字节
                    if (bytes[i] > 0)
                    {
                        n++;
                    }
                }
            }
            //  如果i为奇数时，处理成偶数
            if (i % 2 == 1)
            {
                //  该UCS2字符是汉字时，去掉这个截一半的汉字
                if (bytes[i] > 0)
                    i = i - 1;
                //  该UCS2字符是字母或数字，则保留该字符
                else
                    i = i + 1;
            }
            return System.Text.Encoding.Unicode.GetString(bytes, 0, i);
        }

        /// <summary>
        /// 限制显示长度
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string substr(string text, int maxLength)
        {
            if (text == null) return string.Empty;

            text = text.Trim();
            var bytes = System.Text.Encoding.UTF8.GetBytes(text);
            if (bytes.Length <= maxLength)
                return text;

            return cutSubstring(text, maxLength);
        }
        /// <summary>
        /// 限制显示长度
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static bool checksStringLength(string text, int minLength, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return false;

            text = text.Trim();
            var bytes = System.Text.Encoding.UTF8.GetBytes(text);
            if (bytes.Length <= maxLength && bytes.Length >= minLength)
                return true;

            return false;
        }



        /// <summary>
        /// 区分中、英文以及数字的 字符截取函数
        /// </summary>
        /// <param name="text">需要获取的字符</param>
        /// <param name="maxLength">最大长度</param>
        /// <param name="isState">随意给一个true或false都可以。无任何实际意义，主要用于实现之前已存在的 Substr 函数的重载</param>
        /// <example></example>
        /// <remarks>邢智， 2011年1月18日 新增</remarks>
        /// <returns></returns>
        public static string substr(string text, int maxLength, bool isState)
        {
            text = text.Trim();
            if (text == null || text == String.Empty)
                return String.Empty;

            int i = 0;  //计数器
            int j = 0;  //计数器
            foreach (char chr in text)
            {
                //判断ASCII码值
                if ((int)chr > 127)
                    i += 2;
                else
                    i++;
                //验证是否超出指定长度
                if (i > maxLength)
                {
                    text = text.Substring(0, j);
                    break;
                }
                j++;
            }
            return text;
        }

        public static int GetStringLength(string str, bool isDouble)
        {
            if (str == String.Empty)
                return 0;
            else
            {
                int i = 0;
                if (isDouble)
                {
                    foreach (char c in str)
                    {
                        if ((int)c > 127)
                            i += 2;
                        else
                            i++;
                    }
                    return i;
                }
                else
                    return str.Length;
            }
        }
        /// <summary>
        /// 过滤安全字符串
        /// </summary>
        /// <param name="text">需要过滤的源字符串</param>
        /// <returns></returns>
        public static string InputText(string text)
        {
            if (text == null)
                return String.Empty;
            text = text.Trim();
            if (String.IsNullOrEmpty(text))
                return String.Empty;
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");	//<br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");	//&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);	//any other tags
            text = Regex.Replace(text, "◇|◢|▼|▽|⊿|♡|■|▓|╝|╚|╔|╗|╬|╓|╩|┠|┨|┯|┷|┏|┓|┳|⊥|﹃|﹄|┌|┐|└|┘|∟|「|」|↑|↓|→|←|↘|↙|┇|┅|﹉|﹊", string.Empty);
            text = Regex.Replace(text, "﹍|﹎|╭|╮|╰|╯|∵|︴|﹏|﹋|﹌|︵|︶|︹|︺|【|】|〖|〗|﹕|﹗|·|√|∪|∩|∈|の|℡|ぁ|§|ミ|灬|№|＊|||≮|≯|÷|±|∫|∝|∧|∨|∏", string.Empty);
            text = Regex.Replace(text, "∥|∠|≌|∽|≒|じ|⊙|●|★|☆|『|』|◆|◣|▲|Ψ|◤|◥|㊣|∑|⌒|ξ|ζ|ω|□|∮|〓|※|∴|ぷ|卐|△|¤|々|♀|♂|∞|①|ㄨ|≡|▃|▄|▅|▆|▇|█|┗|┛", "");
            text = Regex.Replace(text, "var|truncate|delete|insert|select|drop|alter|update|exec|char|fetch|declare|cast|convert|sysobjects|syscolumns|create|%20|exists|trace|xp_cmdshell", "", RegexOptions.IgnoreCase);
            text = text.Replace("'", "＇");
            text = text.Replace(";", "；");
            text = text.Replace("--", "－－");
            return text;
        }

        private int rep = 0;
        /// <summary>
        /// 生成随机数字字符串
        /// </summary>
        /// <param name="codeCount">待生成的位数</param>
        /// <returns>生成的数字字符串</returns>
        public string GenerateCheckCodeNum(int codeCount)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + this.rep;
            this.rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> this.rep)));
            for (int i = 0; i < codeCount; i++)
            {
                int num = random.Next();
                str = str + ((char)(0x30 + ((ushort)(num % 10)))).ToString();
            }
            return str;
        }
        /// <summary>
        /// 生成随机字母字符串(数字字母混和)
        /// 待生成的位数
        /// 生成的字母字符串
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public string GenerateCheckCode(int codeCount)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + this.rep;
            this.rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> this.rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }

        /// <summary>
        /// 剔除 Html 页面上的javascript、style标签以及其中内容！
        /// </summary>
        /// <param name="input">需要剔除的字符串</param>
        /// <returns></returns>
        public static string NoTagText(string input)
        {
            string ret = input;
            ret = System.Text.RegularExpressions.Regex.Replace(ret, "<script(.*?)</script>", "");
            ret = System.Text.RegularExpressions.Regex.Replace(ret, "<style(.*?)</style>", "");
            ret = System.Text.RegularExpressions.Regex.Replace(ret, "<(.*?)>", "");
            return ret;
        }


        public static string InputTitle(string text)
        {
            if (text == null)
                return String.Empty;
            text = Regex.Replace(text, @"[^\u4e00-\u9fa5\da-zA-Z\.\-]", "");
            text = Regex.Replace(text, "var|truncate|delete|insert|select|drop|alter|update|exec|char|fetch|declare|cast|convert|sysobjects|syscolumns|create|%20|exists|trace|xp_cmdshell", "", RegexOptions.IgnoreCase);
            return text;
        }
        /// <summary>
        /// 转换一,二,三,四等为阿拉伯数字
        /// </summary>
        /// <param name="s">一,二,三,四等</param>
        /// <returns>num</returns>
        public static int TransNum(string s)
        {
            string[] arr = new string[] { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };
            int num = -1;
            for (int i = 0; i < arr.Length; i++)
                if (arr[i] == s.Trim())
                    num = i + 1;
            return num;
        }

        /// <summary>
        /// 将字符串编码为Base64字符串
        /// </summary>
        /// <param name="str">要编码的字符串</param>
        public static string Base64Encode(string str)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(str));
        }

        /// <summary>
        /// 将Base64字符串解码为普通字符串
        /// </summary>
        /// <param name="str">要解码的字符串</param>
        public static string Base64Decode(string str)
        {
            byte[] barray;
            barray = Convert.FromBase64String(HttpUtility.UrlDecode(str, Encoding.UTF8));
            return Encoding.UTF8.GetString(barray);
        }

        /// <summary>
        /// 判断一个字符串是否在字符串数组里面。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="strarry"></param>
        /// <returns></returns>
        public static bool StrInArray(string str, string[] strarry)
        {
            if (str == null)
                return false;
            if (strarry == null || strarry.Length == 0)
                return false;
            for (int i = 0; i < strarry.Length; i++)
            {
                if (strarry[i] == null)
                    continue;
                if (str == strarry[i])
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 替换特殊字符
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string InputSMS(string text)
        {
            if (text == null)
                return "";

            string s = System.Text.RegularExpressions.Regex.Replace(text.Trim(), "(^86)|(^8#)", "");

            s = System.Text.RegularExpressions.Regex.Replace(s, "(^qz)|(^q)", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            s = System.Text.RegularExpressions.Regex.Replace(s, "^\\,|^\\s+|^\\，", "");

            return s;
        }

        public static string SqlPrefix(string sql)
        {
            System.Web.HttpContext web = System.Web.HttpContext.Current;

            StringBuilder sb = new StringBuilder();

            sb.Append("/* #" + web.Request.ServerVariables["LOCAL_ADDR"]);
            sb.Append("#" + web.Request.ServerVariables["REMOTE_ADDR"]);
            sb.Append("#" + web.Request.ServerVariables["URL"]);
            sb.Append("# */");
            sb.Append(sql);


            web = null;
            return sb.ToString();
        }

        #region 自定义 URLEncode 与 UrlDecode

        public static string UrlEncode(string s)
        {
            if (s == null || s == string.Empty) return string.Empty;



            return HttpUtility.UrlEncode(s, Encoding.UTF8);
        }

        public static string UrlDecode(string s)
        {
            if (s == null || s == string.Empty) return string.Empty;

            return HttpUtility.UrlDecode(s);
        }

        public static string UrlDecode(string s, bool filterHtml)
        {
            if (filterHtml)
                return RegexCore.ReplaceHtml("", UrlDecode(s), "");
            else
                return UrlDecode(s);
        }
        //针对只有cookie名，没有key的情况。
        public static string getCookies(string cookieName)
        {
            HttpCookie cookie;
            cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
            {
                return HttpContext.Current.Server.HtmlEncode(cookie.Value);
            }
            else
            {
                return "";
            }
        }
        #endregion

        public static string GetConfigApp(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings.Get(key);
        }
        public static string GetLongStringByTime()
        {
            string day = DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00");
            string time = DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");
            string ms = DateTime.Now.Millisecond.ToString("00");
            return day + time + ms;
        }
        /// <summary>
        /// 验证是否是有效手机号
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool ValidationPhone(string phone)
        {
            string tmoblie = @"^(13[0-9]|14[5|7]|15[0|1|2|3|5|6|7|8|9]|18[0|1|2|3|5|6|7|8|9]|17[0|1|2|3|5|6|7|8|9])\d{8}$";
            return Regex.IsMatch(phone, tmoblie) ? true : false;

        }
        /// <summary>
        /// 验证是否是有效邮箱
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool ValidationEmail(string email)
        {
            string temail = @"^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$";
            return Regex.IsMatch(email, temail) ? true : false;
        }
        /// <summary>
        /// 验证是否是有效用户名
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool ValidationUserName(string name)
        {
            string temail = @"^[a-zA-Z]\w{5,15}$";
            if (!string.IsNullOrEmpty(name))
            {
                if (Regex.IsMatch(name, temail))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 验证是否是有效汉字名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool ValidationRealName(string name)
        {
            string temail = @"^[\u4E00-\u9FFF]+$";
            if (!string.IsNullOrEmpty(name))
            {
                if (Regex.IsMatch(name, temail))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 替换字符中指定位置内容  
        /// 第二、三个参数类似substring 方法
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        /// <param name="repCount"></param>
        /// <param name="rep"></param>
        /// <returns></returns>
        public static string ReplaceContent(string str, int startIndex, int repCount, char rep)
        {
            var result = string.Empty;
            if (startIndex > -1 && repCount > 0 && str.Length > startIndex && str.Length + 1 > startIndex + repCount)
            {
                result = string.Format("{0}{1}{2}", str.Substring(0, startIndex), new string(rep, repCount), str.Substring(startIndex + repCount));
            }
            return result;
        }


    }
}