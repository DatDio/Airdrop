using Airdrop.Helper;
using Airdrop.Model;
using Leaf.xNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Airdrop.Controller
{
	public class XKucoin_Api
	{
		Leaf.xNet.HttpRequest rq;
		AccountModel model;
		string header_temp, body, cookie = "", hash="", auth_date="", chat_instance="", start_param="",
			userID="", first_name="", last_name="", username="", language_code="";
		public XKucoin_Api(AccountModel accountModel)
		{
			this.model = accountModel;
			rq = new Leaf.xNet.HttpRequest();
			rq.Cookies = new CookieStorage();
			rq.AllowAutoRedirect = true;
			rq.KeepAlive = true;
			rq.Proxy = FunctionHelper.ConvertToProxyClient(model.Proxy);
			rq.UserAgent = Form1.useragent;

			#region getAllRequestPayload
			var urlDecode = HttpUtility.UrlDecode(accountModel.InitParam);
			var urlJson = HttpUtility.UrlDecode(urlDecode);
			//long authDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			//auth_date = authDate.ToString();
			auth_date = RegexHelper.GetValueFromRegex("auth_date=(.*?)&", urlJson);
			first_name = RegexHelper.GetValueFromRegex("\"first_name\":\"(.*?)\"",urlJson);
			last_name = RegexHelper.GetValueFromRegex("\"last_name\":\"(.*?)\"", urlJson);
			username = RegexHelper.GetValueFromRegex("\"username\":\"(.*?)\"", urlJson);
			chat_instance = RegexHelper.GetValueFromRegex("chat_instance=(.*?)&", urlJson);
			start_param = RegexHelper.GetValueFromRegex("start_param=(.*?)&", urlJson);
			language_code = RegexHelper.GetValueFromRegex("\"language_code\":\"(.*?)\",", urlJson);
			hash = RegexHelper.GetValueFromRegex("hash=(.*?)&", urlJson);
			#endregion




			header_temp = @"Accept-Language: vi-VN,vi;q=0.9,fr-FR;q=0.8,fr;q=0.7,en-US;q=0.6,en;q=0.5
                      accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7
					  Origin:https://www.kucoin.com
					Referer: https://www.kucoin.com/miniapp/tap-game?bot_click=openminiapp
                      Priority:u=1, i
                      Sec-Ch-Ua-Mobile:?1
                      Sec-Ch-Ua-Platform:""Windows""
                      Sec-Fetch-Dest:empty
                      Sec-Fetch-Mode:cors
                      Sec-Fetch-Site:same-site";
		}
		public string GetCookie()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp);
				body = rq.Get(model.InitParam).ToString();
			}
			catch
			{
				body = "";
			}

			var payload = new
			{
				extInfo = new
				{
					hash = hash,
					auth_date = auth_date,
					via = "miniApp",

					// Sử dụng string interpolation để thay thế giá trị
					user = $"{{\"id\":{model.UserId},\"first_name\":\"{first_name}\",\"last_name\":\"{last_name}\",\"username\":\"{username}\",\"language_code\":\"{language_code}\",\"allows_write_to_pm\":true}}",

					chat_type = "sender",
					chat_instance = chat_instance,
					start_param = start_param
				}
			};

			try
			{
				// Chuyển đổi payload sang chuỗi JSON
				string jsonPayload = JsonConvert.SerializeObject(payload);
				rq.AddHeader(HttpHeader.ContentType, "application/json");

				// Gửi POST request kèm theo query string và payload
				body = rq.Post("https://www.kucoin.com/_api/xkucoin/platform-telebot/game/login?lang=en_US", jsonPayload, "application/json").ToString();
			}
			catch
			{
				body = "";
			}
			cookie = rq.Cookies.GetCookieHeader("https://www.kucoin.com");
			var cookies = cookie.Split(';');
			foreach (var ck in cookies)
			{
				try
				{
					var arr = ck.Split("=".ToCharArray(), 2);
					rq.Cookies.Add(new System.Net.Cookie(arr[0].Trim(), arr[1].Trim(), "/", "www.kucoin.com"));

				}
				catch { }
			}
			
			return cookie;
		}

		public bool increaseGold()
		{
			
			var param = new RequestParams()
			{
				["increment"] = $"{Form1.random.Next(50, 100)}",
				["molecule"] = "3000",
			};
			try
			{
				body = rq.Post("https://www.kucoin.com/_api/xkucoin/platform-telebot/game/gold/increase?lang=en_US", param).ToString();
			}
			catch
			{
				return false;
			}
			var success = RegexHelper.GetValueFromRegex("\"success\":(.*?),", body);
			if (success == "true") return true;
			return false;
		}
	}
}
