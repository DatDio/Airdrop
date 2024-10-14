using Airdrop.Model;
using Jint;
using Leaf.xNet;
using Microsoft.ClearScript.V8;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Airdrop.Helper
{
	public class FunctionHelper
	{
		public static void AddHeader(Leaf.xNet.HttpRequest rq, string headers)
		{
			var Listheader = headers.Replace("\r", "").Split('\n');
			foreach (var header in Listheader)
			{
				if (header.Trim() == "")
				{
					continue;
				}

				try
				{
					var hd = header.Split(":".ToCharArray(), 2);
					rq.AddHeader(hd[0].Trim(), hd[1].Trim());
				}
				catch { }
			}
		}


	

		

		public static string GenerateRandomString(int length)
		{
			Random random = new Random();
			const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
			char[] randomString = new char[length];

			for (int i = 0; i < length; i++)
			{
				randomString[i] = chars[random.Next(chars.Length)];
			}

			return new string(randomString);
		}

		public static string GenerateRandomString_1(int length)
		{
			string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			var random = new Random();
			var result = new StringBuilder(length);
			var charArray = chars.ToCharArray();
			int charArrayLength = charArray.Length;

			for (int i = 0; i < length; i++)
			{
				result.Append(charArray[random.Next(charArrayLength)]);
			}

			return result.ToString();
		}

		public static ProxyClient ConvertToProxyClient(string proxy)
		{
			if (string.IsNullOrEmpty(proxy))
				return null;

			if (proxy.Contains("socks5://"))
			{
				proxy = proxy.Replace("socks5://", "");
				var proxies = proxy.Split(':');
				if (proxies.Length > 2)
				{
					return new Socks5ProxyClient(proxies[0], int.Parse(proxies[1]), proxies[2], proxies[3]);
				}
				else
				{
					return new Socks5ProxyClient(proxies[0], int.Parse(proxies[1]));
				}
			}
			else
			{
				proxy = proxy.Replace("http://", "");
				var proxies = proxy.Split(':');
				if (proxies.Length > 2)
				{
					return new HttpProxyClient(proxies[0], int.Parse(proxies[1]), proxies[2], proxies[3]);
				}
				else
				{
					return new HttpProxyClient(proxies[0], int.Parse(proxies[1]));
				}
			}
		}

		public static void SetCookieToRequestXnet(Leaf.xNet.HttpRequest rq, string cookie)
		{
			var cookies = cookie.Split(';');
			foreach (var ck in cookies)
			{
				try
				{
					var arr = ck.Split("=".ToCharArray(), 2);
					if (arr[0].Trim() == "ci_session")
						rq.Cookies.Add(new System.Net.Cookie(arr[0].Trim(), arr[1].Trim(), "/", "tgames.bcsocial.net"));
					else
						rq.Cookies.Add(new System.Net.Cookie(arr[0].Trim(), arr[1].Trim(), "/", ".bcsocial.net"));
				}
				catch { }
			}
		}



		public static DateTime TimeStampToDateTime(double seconde)
		{
			// Unix timestamp is seconds past epoch
			System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddSeconds(seconde).ToLocalTime();
			return dtDateTime;
		}

		public static DateTime JavaTimeStampToDateTime(double miliseconde)
		{
			// Java timestamp is milliseconds past epoch
			System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddMilliseconds(miliseconde).ToLocalTime();
			return dtDateTime;
		}

		public static string PassRecaptcha(string apikey, string sitekey, string siteurl)
		{
			try
			{
				using (var client = new WebClient())
				{
					var body = client.DownloadString($"https://api.rockcaptcha.com/recaptchav2?apikey={apikey}&sitekey={sitekey}&siteurl={siteurl}");
					var taskId = RegexHelper.GetValueFromRegex("\"TaskId\":(.*?),", body);
					if (taskId != "")
					{
						for (int i = 0; i < 50; i++)
						{
							Thread.Sleep(2000);
							body = client.DownloadString($"https://api.rockcaptcha.com/getresult?apikey={apikey}&taskId={taskId}");
							var gRecaptchaResponse = RegexHelper.GetValueFromRegex("\"Token\":\"(.*?)\"", body);
							if (gRecaptchaResponse != "") return gRecaptchaResponse;
						}
					}
				}
			}
			catch { }

			return "";
		}

		public static string GetValueChr_1(string chq)
		{
			var decode = Execute(chq);
			var match = Regex.Match(decode, "\\(function\\(\\){(.*?);return");
			if (match.Success)
			{
				try
				{
					Dictionary<string, string> dict = new Dictionary<string, string>();
					var string_1 = match.Groups[1].Value.ToString().Replace("\\x20", " ").Replace("\\x22", "\"").Replace("'+'", "");
					var matches = Regex.Matches(string_1, "id=\"(.*?)\" _v=\"(.*?)\"");
					foreach (Match match1 in matches)
					{
						dict[match1.Groups[1].Value] = match1.Groups[2].Value;
					}
					matches = Regex.Matches(string_1, "\\['(.*?)'\\]='(.*?)'");
					foreach (Match match1 in matches)
					{
						if (dict.ContainsKey(match1.Groups[2].Value))
						{
							dict[match1.Groups[1].Value] = dict[match1.Groups[2].Value];
						}
					}

					var value_1 = "";
					var matches_1 = Regex.Matches(string_1, "\\['(.*?)'\\]");
					foreach (Match match1 in matches_1)
					{
						if (match1.Groups[1].Value != "_v")
							value_1 = match1.Groups[1].Value;
					}

					var value_2 = "";
					var matches_2 = Regex.Matches(string_1, "\\('(.*?)'\\)");
					foreach (Match match1 in matches_2)
					{
						if (match1.Groups[1].Value != "_v")
							value_2 = match1.Groups[1].Value;
					}

					var matches_3 = Regex.Matches(string_1, "\\w{1}=");
					var key_1 = matches_3[matches_3.Count - 3].Value.Replace("=", "");
					var key_2 = matches_3[matches_3.Count - 2].Value.Replace("=", "");
					var key_3 = matches_3[matches_3.Count - 1].Value.Replace("=", "");
					var matches_4 = Regex.Matches(string_1, "\\+\\w{1}");
					var key_4 = matches_4[0].Value.Replace("+", "");

					var stt_1 = string_1.LastIndexOf(value_1);
					var stt_2 = string_1.LastIndexOf(value_2);

					var string_2 = "";
					if (stt_1 < stt_2)
						string_2 += $"var {key_1}={dict[value_1]},{key_2}={dict[value_2]},{key_3}=+{key_4}";
					else
						string_2 += $"var {key_1}={dict[value_2]},{key_2}={dict[value_1]},{key_3}=+{key_4}";

					decode = decode.Replace(match.Groups[1].Value, string_2).Replace("(function(c", "var result=(function(c");
				}
				catch { return ""; }
			}
			else
				return "";

			try
			{
				var engine = new Engine();
				var result = engine.Execute(decode).GetValue("result").ToObject();

				return result.ToString();
			}
			catch (Exception ex)
			{
				// Xử lý exception nếu có lỗi khi thực hiện đoạn mã JavaScript
				Console.WriteLine("Error executing JavaScript: " + ex.Message);
			}

			return "";
		}

		public static string GetValueChr_2(string chq)
		{
			var decode = Execute(chq);
			var match = Regex.Match(decode, "\\(function\\(\\){(.*?);return");
			if (match.Success)
			{
				try
				{
					Dictionary<string, string> dict = new Dictionary<string, string>();
					var string_1 = match.Groups[1].Value.ToString().Replace("\\x20", " ").Replace("\\x22", "\"").Replace("'+'", "");
					var matches = Regex.Matches(string_1, "id=\"(.*?)\" _v=\"(.*?)\"");
					foreach (Match match1 in matches)
					{
						dict[match1.Groups[1].Value] = match1.Groups[2].Value;
					}
					matches = Regex.Matches(string_1, "\\['(.*?)'\\]='(.*?)'");
					foreach (Match match1 in matches)
					{
						if (dict.ContainsKey(match1.Groups[2].Value))
						{
							dict[match1.Groups[1].Value] = dict[match1.Groups[2].Value];
						}
					}

					string value_1 = "", value_2 = "";
					var matches_1 = Regex.Matches(string_1, "\\['(.{5})'\\]\\)");
					var matches_2 = Regex.Matches(string_1, "\\('(.{5})'\\)");

					if (matches_2.Count == 0 && matches_1.Count > 1)
					{
						value_1 = matches_1[matches_1.Count - 2].Groups[1].Value;
						value_2 = matches_1[matches_1.Count - 1].Groups[1].Value;
					}
					else if (matches_1.Count == 0 && matches_2.Count > 1)
					{
						value_1 = matches_2[matches_2.Count - 2].Groups[1].Value;
						value_2 = matches_2[matches_2.Count - 1].Groups[1].Value;
					}

					var matches_3 = Regex.Matches(string_1, "\\w{1}=");
					var key_1 = matches_3[matches_3.Count - 3].Value.Replace("=", "");
					var key_2 = matches_3[matches_3.Count - 2].Value.Replace("=", "");
					var key_3 = matches_3[matches_3.Count - 1].Value.Replace("=", "");
					var matches_4 = Regex.Matches(string_1, "\\+\\w{1}");
					var key_4 = matches_4[0].Value.Replace("+", "");

					var stt_1 = string_1.LastIndexOf(value_1);
					var stt_2 = string_1.LastIndexOf(value_2);

					var string_2 = "";
					if (stt_1 < stt_2)
						string_2 += $"var {key_1}={dict[value_1]},{key_2}={dict[value_2]},{key_3}=+{key_4}";
					else
						string_2 += $"var {key_1}={dict[value_2]},{key_2}={dict[value_1]},{key_3}=+{key_4}";

					decode = decode.Replace(match.Groups[1].Value, string_2).Replace("(function(c", "var result=(function(c");
				}
				catch { return ""; }
			}
			else
				return "";

			try
			{
				var engine = new Engine();
				var result = engine.Execute(decode).GetValue("result").ToObject();

				return result.ToString();
			}
			catch (Exception ex)
			{
				// Xử lý exception nếu có lỗi khi thực hiện đoạn mã JavaScript
				Console.WriteLine("Error executing JavaScript: " + ex.Message);
			}

			return "";
		}

		public static string GetValueChr_3(string chq, string userId, string Xcv)
		{
			using (var engine = new V8ScriptEngine())
			{
				string jsCode = $@"var document = {{}};
let myMap = new Map();
let headers = {{}};
document.getElementById = (id) => {{
    let o = myMap.get(id);
    if (o === undefined) {{
        o = {{
            innerHTML: '',
            getAttribute: (att) => {{
                var result = '';
                myMap.forEach(value => {{
                    var t = getValueByIdAndAttribute(JSON.stringify(value), id, att);
                    if (t != null) {{
                        result = t;
                    }}
                }})
                return result;
            }}
        }};
        myMap.set(id, o);
    }}
    return o;
}}

function getValueByIdAndAttribute(htmlString, id, attribute) {{
    const parsedObject = JSON.parse(htmlString);
    const innerHTML = parsedObject.innerHTML;
    const regex = new RegExp(`<div\\s+id=""${{id}}""[^>]*\\s+${{attribute}}=""([^""]+)""`);
    const match = innerHTML.match(regex);

    if (match && match[1]) {{
        return match[1];
    }} else {{
        return null;
    }}
}}

var window = {{}};
window.document = document;
window.ctx = {{}};
window.ctx.api = {{}};
window.ctx.api.setHeaders = (a) => {{
    headers = a;
}}
window.ctx.api.headers = {{}};
window.ctx.api.headers.get = (key) => {{
    if (key !== 'x-cv') {{
        console.log('Change x-cv: ' + key);
    }}
    return {Xcv};
}}
window.Telegram = {{}};
window.Telegram.WebApp = {{}};
window.Telegram.WebApp.initDataUnsafe = {{}};
window.Telegram.WebApp.initDataUnsafe.user = {{}};
window.Telegram.WebApp.initDataUnsafe.user.id = {userId};

function getChr(value){{
    return eval(value);
}}

function getCache(){{
    return headers['Cache-Id']
}}
";

				// Thực thi mã JavaScript
				engine.Execute(jsCode);
				var decode = Execute(chq);
				return $"{engine.Invoke("getChr", decode)}-{engine.Invoke("getCache")}";
			}
		}

		public static string GetApiHash_empirebot_1(string data)
		{
			using (var engine = new V8ScriptEngine())
			{
				string jsCode = $@"function Dx(e) {{
    function t(g) {{
        var S = n(a(r(o(g), 8 * g.length)));
        return S.toLowerCase()
    }}
    function n(g) {{
        for (var S, p = ""0123456789ABCDEF"", b = """", v = 0; v < g.length; v++)
            S = g.charCodeAt(v),
                b += p.charAt(S >>> 4 & 15) + p.charAt(15 & S);
        return b
    }}
    function o(g) {{
        for (var S = Array(g.length >> 2), p = 0; p < S.length; p++)
            S[p] = 0;
        for (p = 0; p < 8 * g.length; p += 8)
            S[p >> 5] |= (255 & g.charCodeAt(p / 8)) << p % 32;
        return S
    }}
    function a(g) {{
        for (var S = """", p = 0; p < 32 * g.length; p += 8)
            S += String.fromCharCode(g[p >> 5] >>> p % 32 & 255);
        return S
    }}
    function r(g, S) {{
        g[S >> 5] |= 128 << S % 32,
            g[14 + (S + 64 >>> 9 << 4)] = S;
        for (var p = 1732584193, b = -271733879, v = -1732584194, m = 271733878, y = 0; y < g.length; y += 16) {{
            var w = p
                , C = b
                , k = v
                , O = m;
            b = u(b = u(b = u(b = u(b = c(b = c(b = c(b = c(b = s(b = s(b = s(b = s(b = l(b = l(b = l(b = l(b, v = l(v, m = l(m, p = l(p, b, v, m, g[y + 0], 7, -680876936), b, v, g[y + 1], 12, -389564586), p, b, g[y + 2], 17, 606105819), m, p, g[y + 3], 22, -1044525330), v = l(v, m = l(m, p = l(p, b, v, m, g[y + 4], 7, -176418897), b, v, g[y + 5], 12, 1200080426), p, b, g[y + 6], 17, -1473231341), m, p, g[y + 7], 22, -45705983), v = l(v, m = l(m, p = l(p, b, v, m, g[y + 8], 7, 1770035416), b, v, g[y + 9], 12, -1958414417), p, b, g[y + 10], 17, -42063), m, p, g[y + 11], 22, -1990404162), v = l(v, m = l(m, p = l(p, b, v, m, g[y + 12], 7, 1804603682), b, v, g[y + 13], 12, -40341101), p, b, g[y + 14], 17, -1502002290), m, p, g[y + 15], 22, 1236535329), v = s(v, m = s(m, p = s(p, b, v, m, g[y + 1], 5, -165796510), b, v, g[y + 6], 9, -1069501632), p, b, g[y + 11], 14, 643717713), m, p, g[y + 0], 20, -373897302), v = s(v, m = s(m, p = s(p, b, v, m, g[y + 5], 5, -701558691), b, v, g[y + 10], 9, 38016083), p, b, g[y + 15], 14, -660478335), m, p, g[y + 4], 20, -405537848), v = s(v, m = s(m, p = s(p, b, v, m, g[y + 9], 5, 568446438), b, v, g[y + 14], 9, -1019803690), p, b, g[y + 3], 14, -187363961), m, p, g[y + 8], 20, 1163531501), v = s(v, m = s(m, p = s(p, b, v, m, g[y + 13], 5, -1444681467), b, v, g[y + 2], 9, -51403784), p, b, g[y + 7], 14, 1735328473), m, p, g[y + 12], 20, -1926607734), v = c(v, m = c(m, p = c(p, b, v, m, g[y + 5], 4, -378558), b, v, g[y + 8], 11, -2022574463), p, b, g[y + 11], 16, 1839030562), m, p, g[y + 14], 23, -35309556), v = c(v, m = c(m, p = c(p, b, v, m, g[y + 1], 4, -1530992060), b, v, g[y + 4], 11, 1272893353), p, b, g[y + 7], 16, -155497632), m, p, g[y + 10], 23, -1094730640), v = c(v, m = c(m, p = c(p, b, v, m, g[y + 13], 4, 681279174), b, v, g[y + 0], 11, -358537222), p, b, g[y + 3], 16, -722521979), m, p, g[y + 6], 23, 76029189), v = c(v, m = c(m, p = c(p, b, v, m, g[y + 9], 4, -640364487), b, v, g[y + 12], 11, -421815835), p, b, g[y + 15], 16, 530742520), m, p, g[y + 2], 23, -995338651), v = u(v, m = u(m, p = u(p, b, v, m, g[y + 0], 6, -198630844), b, v, g[y + 7], 10, 1126891415), p, b, g[y + 14], 15, -1416354905), m, p, g[y + 5], 21, -57434055), v = u(v, m = u(m, p = u(p, b, v, m, g[y + 12], 6, 1700485571), b, v, g[y + 3], 10, -1894986606), p, b, g[y + 10], 15, -1051523), m, p, g[y + 1], 21, -2054922799), v = u(v, m = u(m, p = u(p, b, v, m, g[y + 8], 6, 1873313359), b, v, g[y + 15], 10, -30611744), p, b, g[y + 6], 15, -1560198380), m, p, g[y + 13], 21, 1309151649), v = u(v, m = u(m, p = u(p, b, v, m, g[y + 4], 6, -145523070), b, v, g[y + 11], 10, -1120210379), p, b, g[y + 2], 15, 718787259), m, p, g[y + 9], 21, -343485551),
                p = f(p, w),
                b = f(b, C),
                v = f(v, k),
                m = f(m, O)
        }}
        return Array(p, b, v, m)
    }}
    function i(g, S, p, b, v, m) {{
        return f(h(f(f(S, g), f(b, m)), v), p)
    }}
    function l(g, S, p, b, v, m, y) {{
        return i(S & p | ~S & b, g, S, v, m, y)
    }}
    function s(g, S, p, b, v, m, y) {{
        return i(S & b | p & ~b, g, S, v, m, y)
    }}
    function c(g, S, p, b, v, m, y) {{
        return i(S ^ p ^ b, g, S, v, m, y)
    }}
    function u(g, S, p, b, v, m, y) {{
        return i(p ^ (S | ~b), g, S, v, m, y)
    }}
    function f(g, S) {{
        var p = (65535 & g) + (65535 & S);
        return (g >> 16) + (S >> 16) + (p >> 16) << 16 | 65535 & p
    }}
    function h(g, S) {{
        return g << S | g >>> 32 - S
    }}
    return t(e)
}}

function Fx(e, t) {{
    return Dx(`${{e}}_${{t}}`)
}}

function Rx() {{
    return Date.now()
}}
function Lx() {{
    return 1e3
}}
function Mx() {{
    return Math.floor(Rx() / Lx())
}}

function Vx(e) {{
    const o = !!e;
    e instanceof Object && (e = JSON.stringify(e));
    const a = Mx()
        , r = Fx(a, o ? e : """");
    return `Api-Time:${{a}}
    Api-Hash:${{r}}`
}}

function GetApiHash(query_id) {{
    const myData = {{
        initData: query_id,
        platform: ""android"",
        chatId: """"
    }}
    return Vx({{ data: myData }});
}}

function Vx2(e) {{
    const o = !!e;
    e instanceof Object && (e = JSON.stringify(e));
    const a = Mx()
        , r = Fx(a, o ? e : """");
    return `Api-Time:${{a}}
    Api-Hash:${{r}}`
}}

function GetHashByTime(_data) {{
    return Vx2(_data);
}}";

				// Thực thi mã JavaScript
				engine.Execute(jsCode);
				return $"{engine.Invoke("GetApiHash", data)}";
			}
		}

		public static string GetApiHash_empirebot_2(string data)
		{
			using (var engine = new V8ScriptEngine())
			{
				string jsCode = $@"function Dx(e) {{
    function t(g) {{
        var S = n(a(r(o(g), 8 * g.length)));
        return S.toLowerCase()
    }}
    function n(g) {{
        for (var S, p = ""0123456789ABCDEF"", b = """", v = 0; v < g.length; v++)
            S = g.charCodeAt(v),
                b += p.charAt(S >>> 4 & 15) + p.charAt(15 & S);
        return b
    }}
    function o(g) {{
        for (var S = Array(g.length >> 2), p = 0; p < S.length; p++)
            S[p] = 0;
        for (p = 0; p < 8 * g.length; p += 8)
            S[p >> 5] |= (255 & g.charCodeAt(p / 8)) << p % 32;
        return S
    }}
    function a(g) {{
        for (var S = """", p = 0; p < 32 * g.length; p += 8)
            S += String.fromCharCode(g[p >> 5] >>> p % 32 & 255);
        return S
    }}
    function r(g, S) {{
        g[S >> 5] |= 128 << S % 32,
            g[14 + (S + 64 >>> 9 << 4)] = S;
        for (var p = 1732584193, b = -271733879, v = -1732584194, m = 271733878, y = 0; y < g.length; y += 16) {{
            var w = p
                , C = b
                , k = v
                , O = m;
            b = u(b = u(b = u(b = u(b = c(b = c(b = c(b = c(b = s(b = s(b = s(b = s(b = l(b = l(b = l(b = l(b, v = l(v, m = l(m, p = l(p, b, v, m, g[y + 0], 7, -680876936), b, v, g[y + 1], 12, -389564586), p, b, g[y + 2], 17, 606105819), m, p, g[y + 3], 22, -1044525330), v = l(v, m = l(m, p = l(p, b, v, m, g[y + 4], 7, -176418897), b, v, g[y + 5], 12, 1200080426), p, b, g[y + 6], 17, -1473231341), m, p, g[y + 7], 22, -45705983), v = l(v, m = l(m, p = l(p, b, v, m, g[y + 8], 7, 1770035416), b, v, g[y + 9], 12, -1958414417), p, b, g[y + 10], 17, -42063), m, p, g[y + 11], 22, -1990404162), v = l(v, m = l(m, p = l(p, b, v, m, g[y + 12], 7, 1804603682), b, v, g[y + 13], 12, -40341101), p, b, g[y + 14], 17, -1502002290), m, p, g[y + 15], 22, 1236535329), v = s(v, m = s(m, p = s(p, b, v, m, g[y + 1], 5, -165796510), b, v, g[y + 6], 9, -1069501632), p, b, g[y + 11], 14, 643717713), m, p, g[y + 0], 20, -373897302), v = s(v, m = s(m, p = s(p, b, v, m, g[y + 5], 5, -701558691), b, v, g[y + 10], 9, 38016083), p, b, g[y + 15], 14, -660478335), m, p, g[y + 4], 20, -405537848), v = s(v, m = s(m, p = s(p, b, v, m, g[y + 9], 5, 568446438), b, v, g[y + 14], 9, -1019803690), p, b, g[y + 3], 14, -187363961), m, p, g[y + 8], 20, 1163531501), v = s(v, m = s(m, p = s(p, b, v, m, g[y + 13], 5, -1444681467), b, v, g[y + 2], 9, -51403784), p, b, g[y + 7], 14, 1735328473), m, p, g[y + 12], 20, -1926607734), v = c(v, m = c(m, p = c(p, b, v, m, g[y + 5], 4, -378558), b, v, g[y + 8], 11, -2022574463), p, b, g[y + 11], 16, 1839030562), m, p, g[y + 14], 23, -35309556), v = c(v, m = c(m, p = c(p, b, v, m, g[y + 1], 4, -1530992060), b, v, g[y + 4], 11, 1272893353), p, b, g[y + 7], 16, -155497632), m, p, g[y + 10], 23, -1094730640), v = c(v, m = c(m, p = c(p, b, v, m, g[y + 13], 4, 681279174), b, v, g[y + 0], 11, -358537222), p, b, g[y + 3], 16, -722521979), m, p, g[y + 6], 23, 76029189), v = c(v, m = c(m, p = c(p, b, v, m, g[y + 9], 4, -640364487), b, v, g[y + 12], 11, -421815835), p, b, g[y + 15], 16, 530742520), m, p, g[y + 2], 23, -995338651), v = u(v, m = u(m, p = u(p, b, v, m, g[y + 0], 6, -198630844), b, v, g[y + 7], 10, 1126891415), p, b, g[y + 14], 15, -1416354905), m, p, g[y + 5], 21, -57434055), v = u(v, m = u(m, p = u(p, b, v, m, g[y + 12], 6, 1700485571), b, v, g[y + 3], 10, -1894986606), p, b, g[y + 10], 15, -1051523), m, p, g[y + 1], 21, -2054922799), v = u(v, m = u(m, p = u(p, b, v, m, g[y + 8], 6, 1873313359), b, v, g[y + 15], 10, -30611744), p, b, g[y + 6], 15, -1560198380), m, p, g[y + 13], 21, 1309151649), v = u(v, m = u(m, p = u(p, b, v, m, g[y + 4], 6, -145523070), b, v, g[y + 11], 10, -1120210379), p, b, g[y + 2], 15, 718787259), m, p, g[y + 9], 21, -343485551),
                p = f(p, w),
                b = f(b, C),
                v = f(v, k),
                m = f(m, O)
        }}
        return Array(p, b, v, m)
    }}
    function i(g, S, p, b, v, m) {{
        return f(h(f(f(S, g), f(b, m)), v), p)
    }}
    function l(g, S, p, b, v, m, y) {{
        return i(S & p | ~S & b, g, S, v, m, y)
    }}
    function s(g, S, p, b, v, m, y) {{
        return i(S & b | p & ~b, g, S, v, m, y)
    }}
    function c(g, S, p, b, v, m, y) {{
        return i(S ^ p ^ b, g, S, v, m, y)
    }}
    function u(g, S, p, b, v, m, y) {{
        return i(p ^ (S | ~b), g, S, v, m, y)
    }}
    function f(g, S) {{
        var p = (65535 & g) + (65535 & S);
        return (g >> 16) + (S >> 16) + (p >> 16) << 16 | 65535 & p
    }}
    function h(g, S) {{
        return g << S | g >>> 32 - S
    }}
    return t(e)
}}

function Fx(e, t) {{
    return Dx(`${{e}}_${{t}}`)
}}

function Rx() {{
    return Date.now()
}}
function Lx() {{
    return 1e3
}}
function Mx() {{
    return Math.floor(Rx() / Lx())
}}

function Vx(e) {{
    const o = !!e;
    e instanceof Object && (e = JSON.stringify(e));
    const a = Mx()
        , r = Fx(a, o ? e : """");
    return `Api-Time:${{a}}
    Api-Hash:${{r}}`
}}

function GetApiHash(query_id) {{
    const myData = {{
        initData: query_id,
        platform: ""android"",
        chatId: """"
    }}
    return Vx({{ data: myData }});
}}

function Vx2(e) {{
    const o = !!e;
    e instanceof Object && (e = JSON.stringify(e));
    const a = Mx()
        , r = Fx(a, o ? e : """");
    return `Api-Time:${{a}}
    Api-Hash:${{r}}`
}}

function GetHashByTime(_data) {{
    return Vx2(_data);
}}";

				// Thực thi mã JavaScript
				engine.Execute(jsCode);
				return $"{engine.Invoke("GetHashByTime", data)}";
			}
		}

		public static string GetValueSecret(string telegramId)
		{
			try
			{
				return CalculateHMAC(telegramId, "adwawdasfajfklasjglrejnoierjboivrevioreboidwa").Result;
			}
			catch { }

			return "";
		}

		private static async Task<string> CalculateHMAC(string t, string e)
		{
			return await Task.Run(() =>
			{
				var n = new UTF8Encoding();
				byte[] r = n.GetBytes(e);
				byte[] s = n.GetBytes(t.ToString());

				using (var hmac = new HMACSHA256(r))
				{
					byte[] o = hmac.ComputeHash(s);
					return BitConverter.ToString(o).Replace("-", "").ToLower();
				}
			});
		}

		public static string GetValueContentId(string id, string time)
		{
			var code = $"var result = {id} * {time} % {id}";

			try
			{
				var engine = new Engine();
				var result = engine.Execute(code).GetValue("result").ToObject();

				return result.ToString();
			}
			catch (Exception ex)
			{
				// Xử lý exception nếu có lỗi khi thực hiện đoạn mã JavaScript
				Console.WriteLine("Error executing JavaScript: " + ex.Message);
			}

			return "";
		}

		private static string Execute(string value)
		{
			int len = value.Length;
			byte[] bytes = new byte[len / 2];
			byte x = 157;

			for (int R = 0; R < len; R += 2)
			{
				// Ensure that the substring length is 2
				string hexPair = value.Substring(R, 2);

				// Convert the hex pair to byte safely
				if (byte.TryParse(hexPair, System.Globalization.NumberStyles.HexNumber, null, out byte byteValue))
				{
					bytes[R / 2] = byteValue;
				}
				else
				{
					throw new ArgumentException($"Invalid hex pair: {hexPair}");
				}
			}

			byte[] xored = bytes.Select(b => (byte)(b ^ x)).ToArray();
			string decoded = Encoding.UTF8.GetString(xored);

			return decoded;
		}

		public static string CalculateMD5Hash(string input)
		{
			using (MD5 md5 = MD5.Create())
			{
				byte[] inputBytes = Encoding.UTF8.GetBytes(input);
				byte[] hashBytes = md5.ComputeHash(inputBytes);

				// Chuyển đổi byte array sang chuỗi hex
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < hashBytes.Length; i++)
				{
					sb.Append(hashBytes[i].ToString("x2"));
				}
				return sb.ToString();
			}
		}

		public static string GetAppSign_Sphynx(string token, long time, string json_param, string useragent)
		{
			string F = useragent;
			string W = token.Substring(token.Length - 50);
			long B = time;

			string cH_F_W_B = cH(F + cH(W + B));
			string cH_W_B = cH(W + B);
			string U = $"{cH_F_W_B.Substring(cH_F_W_B.Length - 20)}{cH_W_B}{json_param}{B}";
			string r = cH(U);
			return r;
		}

		private static string cH(string input)
		{
			using (MD5 md5 = MD5.Create())
			{
				byte[] inputBytes = Encoding.ASCII.GetBytes(input);
				byte[] hashBytes = md5.ComputeHash(inputBytes);

				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < hashBytes.Length; i++)
				{
					sb.Append(hashBytes[i].ToString("x2"));
				}
				return sb.ToString();
			}
		}

		public static string RunNodeScript_old(string nodePath, string scriptPath)
		{
			var startInfo = new ProcessStartInfo
			{
				FileName = nodePath,
				Arguments = scriptPath,
				RedirectStandardOutput = true,
				UseShellExecute = false,
				CreateNoWindow = true
			};

			using (var process = Process.Start(startInfo))
			{
				using (var reader = process.StandardOutput)
				{
					string result = reader.ReadToEnd();
					process.WaitForExit();
					return result;
				}
			}
		}

	

		private static string FormatResult(byte[] salt, byte[] ciphertext)
		{
			byte[] prefix = Encoding.ASCII.GetBytes("Salted__");
			byte[] prefixSaltCiphertext = new byte[prefix.Length + salt.Length + ciphertext.Length];
			Buffer.BlockCopy(prefix, 0, prefixSaltCiphertext, 0, prefix.Length);
			Buffer.BlockCopy(salt, 0, prefixSaltCiphertext, prefix.Length, salt.Length);
			Buffer.BlockCopy(ciphertext, 0, prefixSaltCiphertext, prefix.Length + salt.Length, ciphertext.Length);
			return Convert.ToBase64String(prefixSaltCiphertext);
		}

	

		public static void CaiThuVien(string directoryPath, string command)
		{
			// Cấu hình và khởi chạy tiến trình
			ProcessStartInfo processStartInfo = new ProcessStartInfo
			{
				FileName = "cmd.exe",
				Arguments = $"/C {command}",
				WorkingDirectory = directoryPath,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true
			};

			using (Process process = new Process())
			{
				process.StartInfo = processStartInfo;
				process.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
				process.ErrorDataReceived += (sender, args) => Console.WriteLine("ERROR: " + args.Data);

				process.Start();
				process.BeginOutputReadLine();
				process.BeginErrorReadLine();
				//process.WaitForExit();
			}
		}

		public static string GetToken_CryptoRank(string init)
		{
			var data = HttpUtility.UrlDecode(HttpUtility.UrlDecode(RegexHelper.GetValueFromRegex("tgWebAppData=(.*?)&", init)));
			var initDataUnsafe = $"{{\"{data.Replace("={", "\":{").Replace("}&", "},\"").Replace("=", "\":\"").Replace("&", "\",\"")}\"}}";
			var code = $@"function btoa(utf8Str) {{
                var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=';
                var result = '';
                var i = 0;
                while (i < utf8Str.length) {{
                    var a = utf8Str.charCodeAt(i++);
                    var b = utf8Str.charCodeAt(i++);
                    var c = utf8Str.charCodeAt(i++);
                    var b1 = (a >> 2) & 0x3F;
                    var b2 = ((a & 0x3) << 4) | ((b >> 4) & 0xF);
                    var b3 = ((b & 0xF) << 2) | ((c >> 6) & 0x3);
                    var b4 = c & 0x3F;
                    if (isNaN(b)) {{
                        b3 = b4 = 64;
                    }} else if (isNaN(c)) {{
                        b4 = 64;
                    }}
                    result += characters.charAt(b1) + characters.charAt(b2) + characters.charAt(b3) + characters.charAt(b4);
                }}
                return result;
            }}

            var result = btoa(unescape(encodeURIComponent(`{initDataUnsafe}`)));";
			try
			{
				var engine = new Engine();
				var result = engine.Execute(code).GetValue("result").ToObject();

				return result.ToString();
			}
			catch (Exception ex)
			{
				// Xử lý exception nếu có lỗi khi thực hiện đoạn mã JavaScript
				Console.WriteLine("Error executing JavaScript: " + ex.Message);
			}

			return "";
		}
	}
}
