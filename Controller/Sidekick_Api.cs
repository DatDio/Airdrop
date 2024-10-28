using Airdrop.Helper;
using Airdrop.Model;
using Airdrop.Model.SideKick;
using Jint.Native;
using Leaf.xNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using WebSocketSharp;

namespace Airdrop.Controller
{
	public class Sidekick_Api
	{
		Leaf.xNet.HttpRequest rq;
		AccountModel model;
		string header_temp, body, cookie = "", hash = "", auth_date = "", chat_instance = "", start_param = "",
			userID = "", first_name = "", last_name = "", username = "", language_code = "";
		public Sidekick_Api(AccountModel accountModel)
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
			first_name = RegexHelper.GetValueFromRegex("\"first_name\":\"(.*?)\"", urlJson);
			last_name = RegexHelper.GetValueFromRegex("\"last_name\":\"(.*?)\"", urlJson);
			username = RegexHelper.GetValueFromRegex("\"username\":\"(.*?)\"", urlJson);
			chat_instance = RegexHelper.GetValueFromRegex("chat_instance=(.*?)&", urlJson);
			start_param = RegexHelper.GetValueFromRegex("start_param=(.*?)&", urlJson);
			language_code = RegexHelper.GetValueFromRegex("\"language_code\":\"(.*?)\",", urlJson);
			hash = RegexHelper.GetValueFromRegex("hash=(.*?)&", urlJson);
			#endregion




			header_temp = @"Accept-Language: vi-VN,vi;q=0.9,fr-FR;q=0.8,fr;q=0.7,en-US;q=0.6,en;q=0.5
                      accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7
					  Origin:https://game.sidekick.fans
					Referer: https://game.sidekick.fans/
                      Priority:u=1, i
                      Sec-Ch-Ua-Mobile:?1
                      Sec-Ch-Ua-Platform:""Windows""
                      Sec-Fetch-Dest:empty
                      Sec-Fetch-Mode:cors
                      Sec-Fetch-Site:same-site";
		}
		public string Login()
		{
			try
			{
				var payload = new
				{
					telegramId = model.UserId,
					firstName = first_name,
					lastName = last_name,
					languageCode = language_code,
					isVip = false,
					init = model.InitData,
				};
				string jsonPayload = JsonConvert.SerializeObject(payload);

				//FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"authorization:Bearer {model.Token}");
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Post($"https://gameapi.sidekick.fans/api/user/login", jsonPayload, "application/json").ToString();
			}
			catch
			{
				body = "";
			}
			model.Token = RegexHelper.GetValueFromRegex("\"accessToken\":\"(.*?)\"", body);
			return model.Token;
		}
		public string GetUserDate()
		{
			try
			{

				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"authorization:Bearer {model.Token}");

				body = rq.Get($"https://gameapi.sidekick.fans/api/user/{model.UserId}/date").ToString();
			}
			catch
			{
				body = "";
				var respone = rq.Response.ToString();
			}
			return body;
		}

		public async Task ClaimTask()
		{
			bool reTryConnect = true;
			List<TaskDto> unfinishedTasks = null;  // Định nghĩa ngoài vòng lặp để lưu trữ nhiệm vụ chưa hoàn thành

			while (reTryConnect)
			{
				try
				{
					using (var ws = new WebSocket("wss://gameapi.sidekick.fans/socket.io/?EIO=4&transport=websocket"))
					{
						ws.OnOpen += (sender, e) =>
						{
							Console.WriteLine("Kết nối thành công");
							var authMessage = $"40{{\"token\":\"Bearer {model.Token}\"}}";
							ws.Send(authMessage);

							Task.Delay(3000).ContinueWith(t =>
							{
								var signinListMessage = "425[\"getSigninList\"]";
								ws.Send(signinListMessage);
							});
						};

						ws.OnClose += (sender, e) =>
						{
							Debug.WriteLine("Ngắt kết nối!");
							if (unfinishedTasks != null && unfinishedTasks.Count > 0)
							{
								Debug.WriteLine("Còn nhiệm vụ chưa hoàn thành. Kết nối lại...");
								Task.Delay(5000).ContinueWith(t =>
								{
									Debug.WriteLine("Thử kết nối lại sau 5 giây...");
									reTryConnect = true;
								});
							}
							else
							{
								reTryConnect = false; 
							}
						};

						ws.OnError += (sender, e) =>
						{
							Debug.WriteLine($"Lỗi rồi: {e.Message}");
							ws.Close();
						};

						ws.OnMessage += async (sender, e) =>
						{
							string message = e.Data;

							// Dùng Regex để tìm số ở đầu chuỗi
							Regex regex = new Regex(@"^\d+");
							Match match = regex.Match(message);
							string numberCodeRespone = match.Value;

							if (numberCodeRespone == "435" && message.Contains("list"))
							{
								string jsonStr = message.Substring(message.IndexOf("["));

								try
								{
									List<ReponeLoginDto> ReponeLoginDto = JsonConvert.DeserializeObject<List<ReponeLoginDto>>(jsonStr);
									if (ReponeLoginDto != null && ReponeLoginDto.Count > 0)
									{
										var signinList = ReponeLoginDto[0].list;

										if (signinList != null)
										{
											var todaySignin = signinList.FirstOrDefault(item => item.isToday == true);

											if (todaySignin != null && !todaySignin.isSignin)
											{
												Debug.WriteLine("Gửi yêu cầu signin");
												var signinMessage = "426[\"signin\"]";
												ws.Send(signinMessage);
											}
											else
											{
												Debug.WriteLine("Hôm nay bạn đã điểm danh rồi");
												GetTaskList(ws);
											}
										}
									}
								}
								catch (Exception ex)
								{
									Debug.WriteLine($"Lỗi xử lý tin nhắn: {ex.Message}");
									Debug.WriteLine($"Nội dung tin nhắn gốc: {message}");
								}
							}
							else if (numberCodeRespone == "437")
							{
								string jsonStr = message.Substring(message.IndexOf("["));

								jsonStr = jsonStr.Substring(1, jsonStr.Length - 2);
								unfinishedTasks = JsonConvert.DeserializeObject<List<TaskDto>>(jsonStr)
									.Where(t => t.isFinish == false && t.title != "Invite 5 Friends")
									.ToList();

								Debug.WriteLine($"Tìm thấy {unfinishedTasks.Count} nhiệm vụ chưa làm");
								foreach (var task in unfinishedTasks.ToList())
								{
									await Task.Delay(1000);
									var changeTaskMessage = $"42{new Random().Next(1000)}[\"changeTask\",{{\"taskId\":\"{task._id}\"}}]";
									ws.Send(changeTaskMessage);
									Debug.WriteLine($"Làm nhiệm vụ: {task.title}");

									unfinishedTasks.Remove(task);  // Xoá nhiệm vụ đã hoàn thành
								}

								await Task.Delay(3000);
								reTryConnect = false;
								ws.Close();
							}
							//else if (message.Contains("exception"))
							//{
							//	Debug.WriteLine($"Thao tác không thành công");
							//}
						};


						ws.Connect();

						while (unfinishedTasks == null || unfinishedTasks.Count > 0)
						{
							Debug.WriteLine("Chờ xử lý nhiệm vụ...");
							await Task.Delay(1000); // Kiểm tra lại trạng thái nhiệm vụ mỗi 1 giây
						}
						break;
					}
				}
				catch (Exception ex)
				{
					//Debug.WriteLine($"Lỗi: {ex.Message}");
					//reTryConnect = true; 
					//await Task.Delay(5000); 
					break;
				}
			}
		}



		private void GetTaskList(WebSocket ws)
		{
			Debug.WriteLine("Gửi yêu cầu getTaskList");
			var taskListMessage = "427[\"getTaskList\"]";
			ws.Send(taskListMessage);
		}
	}
}
