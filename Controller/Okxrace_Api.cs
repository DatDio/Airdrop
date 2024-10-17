using Airdrop.Helper;
using Airdrop.Model;
using Leaf.xNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Airdrop.Controller
{
	public class Okxrace_Api
	{
		Leaf.xNet.HttpRequest rq;
		AccountModel model;
		//List<TaskDto> Alltask, CompletedTask;
		string header_temp, body, username, id;
		public Okxrace_Api(AccountModel accountModel)
		{
			this.model = accountModel;
			rq = new Leaf.xNet.HttpRequest();
			rq.Cookies = new CookieStorage();
			rq.AllowAutoRedirect = true;
			rq.KeepAlive = true;
			rq.Proxy = FunctionHelper.ConvertToProxyClient(model.Proxy);
			rq.UserAgent = Form1.useragent;

			var urlDecode = HttpUtility.UrlDecode(accountModel.InitParam);
			var urlJson = HttpUtility.UrlDecode(urlDecode);
			username = RegexHelper.GetValueFromRegex("\"username\":\"(.*?)\"", urlJson);

			header_temp = $@"Accept-Language: vi-VN,vi;q=0.9,fr-FR;q=0.8,fr;q=0.7,en-US;q=0.6,en;q=0.5
                      accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7
					  Origin:https://telegram.nomis.cc/
					Referer: https://telegram.nomis.cc/
					X-App-Init-Data: {model.InitData}
                      Priority:u=1, i
                      Sec-Ch-Ua-Mobile:?1
                      Sec-Ch-Ua-Platform:""Windows""
                      Sec-Fetch-Dest:empty
                      Sec-Fetch-Mode:cors
					  Authorization: Bearer 8e25d2673c5025dfcd36556e7ae1b330095f681165fe964181b13430ddeb385a0844815b104eff05f44920b07c073c41ff19b7d95e487a34aa9f109cab754303cd994286af4bd9f6fbb945204d2509d4420e3486a363f61685c279ae5b77562856d3eb947e5da44459089b403eb5c80ea6d544c5aa99d4221b7ae61b5b4cbb55
                      Sec-Fetch-Site:same-site";
		}
		public string checkDailyRewards()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Get($"https://www.okx.com/priapi/v1/affiliate/game/racer/tasks?t=${DateTime.Now}").ToString();

				//Lấy ra id để làm payload khi post claimTask
				id = RegexHelper.GetValueFromRegex("{\"id\":(.*?),", body);
			}
			catch
			{
				body = "";
			}
			return id;
		}
	}
}
