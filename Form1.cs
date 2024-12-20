﻿using Airdrop.Controller;
using Airdrop.Helper;
using Airdrop.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Airdrop
{
	public partial class Form1 : Form
	{
		public static string useragent, game, url;
		public static Random random = new Random();
		bool stop = false;
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			useragent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36";
		}

		private void button1_Click(object sender, EventArgs e)
		{
			game = comboBox1.Text;
			url = textBox1.Text;

			if (game == "Major") Major();
			else if (game == "XKucoin") XKucoin();
			else if (game == "Nomis") Nomis();
			else if (game == "TonStation") TonStation();
			else if (game == "Vooi") Vooi();
			else if (game == "Sidekick") Sidekick();
			else if (game == "Bird") Bird();
			else if (game == "CryptoRank") CryptoRank();
			else if (game == "Coub") Coub();
			else if (game == "Clayton") Clayton();
			else if (game == "PipWorld") PipWorld();
			else if (game == "PinEye") PinEye();
			else if (game == "DuckChain") DuckChain();
			else if (game == "KiloEx") KiloEx();
			else if (game == "Frog Farm") FrogFarm();
			else if (game == "Vertus") Vertus();
			else if (game == "Dropee") Dropee();
			else if (game == "Ducks") Ducks();
		}

		private void Major()
		{
			Major_Api _apiController = null;
			AccountModel accountModel = new AccountModel { InitParam = url };

			try
			{
				_apiController = new Major_Api(accountModel);
				accountModel.InitData = HttpUtility.UrlDecode(RegexHelper.GetValueFromRegex("tgWebAppData=(.*?)&", accountModel.InitParam));
				accountModel.UserId = RegexHelper.GetValueFromRegex("id%2522%253A(.*?)%", accountModel.InitParam);

				//Get token ...
				accountModel.Token = _apiController.GetToken();
				if (accountModel.Token == "")
				{
					//Invoke((MethodInvoker)delegate { accountModel.Row.Cells["Status"].Value = "Get token null"; });
					goto END;
				}

				//Invoke((MethodInvoker)delegate
				//{
				//	accountModel.Row.Cells["Token"].Value = accountModel.Token;
				//	accountModel.Row.Cells["Status"].Value = "Get info lần 1 ...";
				//});
				accountModel.Amount = _apiController.GetBalance();

				if (accountModel.Amount == "")
				{
					//Invoke((MethodInvoker)delegate
					//{
					//	accountModel.Row.Cells["Status"].Value = "Get info lần 1 null";
					//	//accountModel.Row.Cells["Check"].Value = false;
					//	//accountModel.Row.DefaultCellStyle.ForeColor = Color.Tomato;
					//});
					goto END;

					//continue;
				}

				//Invoke((MethodInvoker)delegate
				//{
				//	accountModel.Row.Cells["Amount"].Value = double.Parse(accountModel.Amount).ToString("N0");
				//});

				//Invoke((MethodInvoker)delegate { accountModel.Row.Cells["Status"].Value = "Điểm danh ..."; });
				_apiController.Visit();

				//Invoke((MethodInvoker)delegate { accountModel.Row.Cells["Status"].Value = "Tasks ..."; });
				_apiController.Tasks();

				//Invoke((MethodInvoker)delegate { accountModel.Row.Cells["Status"].Value = "Play game ..."; });
				_apiController.PlayGame_Hold();
				_apiController.PlayGame_Roulette();
				_apiController.PlayGame_Swipe();

				//Invoke((MethodInvoker)delegate { accountModel.Row.Cells["Status"].Value = "Get info lần 2 ..."; });
				accountModel.Amount = _apiController.GetBalance();

				if (accountModel.Amount != "")
				{
					accountModel.Time = DateTime.Now.AddHours(7).ToString();
					//Invoke((MethodInvoker)delegate
					//{
					//	accountModel.Row.Cells["Time"].Value = accountModel.Time;
					//	accountModel.Row.Cells["Amount"].Value = double.Parse(accountModel.Amount).ToString("N0");
					//	accountModel.Row.Cells["Status"].Value = "OK";
					//});
				}
			}
			catch
			{
				//Invoke((MethodInvoker)delegate { accountModel.Row.Cells["Status"].Value = "Lỗi catch"; });
				goto END;
			}

		END:
			Console.WriteLine("end");
		}

		private void XKucoin()
		{
			XKucoin_Api _apiController = null;
			AccountModel accountModel = new AccountModel { InitParam = url };

			try
			{
				_apiController = new XKucoin_Api(accountModel);
				accountModel.InitData = HttpUtility.UrlDecode(RegexHelper.GetValueFromRegex("tgWebAppData=(.*?)&", accountModel.InitParam));
				accountModel.UserId = RegexHelper.GetValueFromRegex("id%2522%253A(.*?)%", accountModel.InitParam);

				//Get token ...
				accountModel.Token = _apiController.GetCookie();
				_apiController.increaseGold();
			}
			catch
			{
				//Invoke((MethodInvoker)delegate { accountModel.Row.Cells["Status"].Value = "Lỗi catch"; });
				goto END;
			}

		END:
			Console.WriteLine("end");
		}

		private void Nomis()
		{
			Nomis_Api _apiController = null;
			AccountModel accountModel = new AccountModel { InitParam = url };

			try
			{
				accountModel.InitData = HttpUtility.UrlDecode(RegexHelper.GetValueFromRegex("tgWebAppData=(.*?)&", accountModel.InitParam));
				accountModel.UserId = RegexHelper.GetValueFromRegex("id%2522%253A(.*?)%", accountModel.InitParam);
				_apiController = new Nomis_Api(accountModel);


				//Get token ...
				accountModel.Token = _apiController.Auth();

				_apiController.GetProfile();

				_apiController.claimTask();
				_apiController.StartFarm();
				_apiController.ClaimFarm();

			}
			catch
			{
				//Invoke((MethodInvoker)delegate { accountModel.Row.Cells["Status"].Value = "Lỗi catch"; });
				goto END;
			}

		END:
			Console.WriteLine("end");
		}
		private void TonStation()
		{
			TonStation_Api _apiController = null;
			AccountModel accountModel = new AccountModel { InitParam = url };

			try
			{
				accountModel.InitData = HttpUtility.UrlDecode(RegexHelper.GetValueFromRegex("tgWebAppData=(.*?)&", accountModel.InitParam));
				accountModel.UserId = RegexHelper.GetValueFromRegex("id%2522%253A(.*?)%", accountModel.InitParam);
				_apiController = new TonStation_Api(accountModel);


				//Get token ...
				accountModel.Token = _apiController.GetToken();

				_apiController.HandleFarming();
				_apiController.HandleTask();



			}
			catch
			{
				//Invoke((MethodInvoker)delegate { accountModel.Row.Cells["Status"].Value = "Lỗi catch"; });
				goto END;
			}

		END:
			Console.WriteLine("end");
		}
		private void Vooi()
		{
			Vooi_Api _apiController = null;
			AccountModel accountModel = new AccountModel { InitParam = url };

			try
			{
				accountModel.InitData = HttpUtility.UrlDecode(RegexHelper.GetValueFromRegex("tgWebAppData=(.*?)&", accountModel.InitParam));
				accountModel.UserId = RegexHelper.GetValueFromRegex("id%2522%253A(.*?)%", accountModel.InitParam);
				_apiController = new Vooi_Api(accountModel);


				//Get token ...
				accountModel.Token = _apiController.GetToken("jnoZKzY");

				_apiController.HandleAutoTrade();
				//_apiController.HandleTask();



			}
			catch
			{
				//Invoke((MethodInvoker)delegate { accountModel.Row.Cells["Status"].Value = "Lỗi catch"; });
				goto END;
			}

		END:
			Console.WriteLine("end");
		}

		private async void Sidekick()
		{
			Sidekick_Api _apiController = null;
			AccountModel accountModel = new AccountModel { InitParam = url };

			try
			{
				accountModel.InitData = HttpUtility.UrlDecode(RegexHelper.GetValueFromRegex("tgWebAppData=(.*?)&", accountModel.InitParam));
				accountModel.UserId = RegexHelper.GetValueFromRegex("id%2522%253A(.*?)%", accountModel.InitParam);
				_apiController = new Sidekick_Api(accountModel);


				//Get token ...
				accountModel.Token = _apiController.Login();

				if (accountModel.Token == "")
				{
					goto END;
				}
				_apiController.GetUserDate();

				await _apiController.ClaimTask();

				Debug.WriteLine("Done");

			}
			catch
			{
				//Invoke((MethodInvoker)delegate { accountModel.Row.Cells["Status"].Value = "Lỗi catch"; });
				goto END;
			}

		END:
			Console.WriteLine("end");
		}
		private async void Bird()
		{
			Bird_Api _apiController = null;
			AccountModel accountModel = new AccountModel { InitParam = url };

			try
			{
				accountModel.InitData = HttpUtility.UrlDecode(RegexHelper.GetValueFromRegex("tgWebAppData=(.*?)&", accountModel.InitParam));
				accountModel.UserId = RegexHelper.GetValueFromRegex("id%2522%253A(.*?)%", accountModel.InitParam);
				_apiController = new Bird_Api(accountModel);


				//Get token ...
				accountModel.Level = _apiController.GetInfo();


				_apiController.CallWormMintAPI();

				_apiController.PlayEggMinigame();
				_apiController.HandelTask();

				Debug.WriteLine("Done");

			}
			catch
			{
				//Invoke((MethodInvoker)delegate { accountModel.Row.Cells["Status"].Value = "Lỗi catch"; });
				goto END;
			}

		END:
			Console.WriteLine("end");
		}

		private async void Coub()
		{
			Coub_Api _apiController = null;
			AccountModel accountModel = new AccountModel { InitParam = url };

			try
			{
				accountModel.InitData = HttpUtility.UrlDecode(RegexHelper.GetValueFromRegex("tgWebAppData=(.*?)&", accountModel.InitParam));
				accountModel.UserId = RegexHelper.GetValueFromRegex("id%2522%253A(.*?)%", accountModel.InitParam);
				_apiController = new Coub_Api(accountModel);


				//Get token ...
				accountModel.Token = _apiController.GetToken();
				if (accountModel.Token == "") goto END;
				//Làm nhiệm vụ
				_apiController.HandleTask();

				Debug.WriteLine("Done");

			}
			catch
			{
				//Invoke((MethodInvoker)delegate { accountModel.Row.Cells["Status"].Value = "Lỗi catch"; });
				goto END;
			}

		END:
			Console.WriteLine("end");
		}
		private async void CryptoRank()
		{
			Cryptorank_Api _apiController = null;
			AccountModel accountModel = new AccountModel { InitParam = url };

			try
			{
				accountModel.InitData = HttpUtility.UrlDecode(RegexHelper.GetValueFromRegex("tgWebAppData=(.*?)&", accountModel.InitParam));
				accountModel.UserId = RegexHelper.GetValueFromRegex("id%2522%253A(.*?)%", accountModel.InitParam);
				_apiController = new Cryptorank_Api(accountModel);


				//Get token ...
				accountModel.Token = "eyJxdWVyeV9pZCI6IkFBRlJLN1JXQUFBQUFGRXJ0Rlk4VFhWVSIsInVzZXIiOnsiaWQiOjE0NTQ2NDgxNDUsImZpcnN0X25hbWUiOiLEkOG6oXQiLCJsYXN0X25hbWUiOiJEaW8iLCJ1c2VybmFtZSI6IkRhdERpbyIsImxhbmd1YWdlX2NvZGUiOiJlbiIsImFsbG93c193cml0ZV90b19wbSI6dHJ1ZX0sImF1dGhfZGF0ZSI6IjE3Mjk4Mjk3MzQiLCJoYXNoIjoiM2U4YzBiMGI0YWY0NWI3NzFiNjIxODQ5ZWI5ODgwYTRmMDk4NmNjNDM1MGZhNTllNWEzNjQ5ZWJkNGNiY2EyZSJ9";
				_apiController.GetInfo();
				if (accountModel.Token == "") goto END;

				_apiController.HandleFarm();

				//Làm nhiệm vụ
				_apiController.HandleTask();

				//Claim Point Từ referral
				_apiController.HandleBuddies();


				Debug.WriteLine("Done");

			}
			catch
			{
				//Invoke((MethodInvoker)delegate { accountModel.Row.Cells["Status"].Value = "Lỗi catch"; });
				goto END;
			}

		END:
			Console.WriteLine("end");
		}
		private async void Clayton()
		{
			Clayton_Api _apiController = null;
			AccountModel accountModel = new AccountModel { InitParam = url };

			try
			{
				accountModel.InitData = HttpUtility.UrlDecode(RegexHelper.GetValueFromRegex("tgWebAppData=(.*?)&", accountModel.InitParam));
				accountModel.UserId = RegexHelper.GetValueFromRegex("id%2522%253A(.*?)%", accountModel.InitParam);
				_apiController = new Clayton_Api(accountModel);


				//Get token ...
				_apiController.ProccessAccount();



				if (accountModel.Token == "") goto END;

				//_apiController.HandleFarm();

				//Làm nhiệm vụ
				//_apiController.HandleTask();

				//Claim Point Từ referral
				//_apiController.HandleBuddies();


				Debug.WriteLine("Done");

			}
			catch
			{
				goto END;
			}

		END:
			MessageBox.Show("end");
		}

		private async void PipWorld()
		{
			PipWorld_Api _apiController = null;
			AccountModel accountModel = new AccountModel { InitParam = url };

			try
			{
				accountModel.InitData = HttpUtility.UrlDecode(RegexHelper.GetValueFromRegex("tgWebAppData=(.*?)&", accountModel.InitParam));
				accountModel.UserId = RegexHelper.GetValueFromRegex("id%2522%253A(.*?)%", accountModel.InitParam);
				_apiController = new PipWorld_Api(accountModel);


				//Get token ...
				_apiController.ProcessAccount();
				_apiController.GetTask();
				//_apiController.ProcessAccount();



				if (accountModel.Token == "") goto END;

				//_apiController.HandleFarm();

				//Làm nhiệm vụ
				//_apiController.HandleTask();

				//Claim Point Từ referral
				//_apiController.HandleBuddies();


				Debug.WriteLine("Done");

			}
			catch
			{
				goto END;
			}

		END:
			MessageBox.Show("end");
		}
		private async void PinEye()
		{
			PinEye_Api _apiController = null;
			AccountModel accountModel = new AccountModel { InitParam = url };

			try
			{
				accountModel.InitData = HttpUtility.UrlDecode(RegexHelper.GetValueFromRegex("tgWebAppData=(.*?)&", accountModel.InitParam));
				accountModel.UserId = RegexHelper.GetValueFromRegex("id%2522%253A(.*?)%", accountModel.InitParam);
				_apiController = new PinEye_Api(accountModel);


				//Get token ...
				accountModel.Token = _apiController.Login();

				_apiController.GetProfile();

				_apiController.TapEnergy(accountModel.CurrentEnergy);

				_apiController.HandleTask();

				//Mua vé số
				_apiController.BuyLottery();

				//Booter tăng energy tap
				_apiController.HandleBooters();

				//mua card tăng profit, mỗi lần chạy mua tăng 1 level
				_apiController.HandlePranaGameCard();


				if (accountModel.Token == "") goto END;

				Debug.WriteLine("Done");

			}
			catch
			{
				goto END;
			}

		END:
			MessageBox.Show("end");
		}

		private async void DuckChain()
		{
			DuckChain_Api _apiController = null;
			AccountModel accountModel = new AccountModel { InitParam = url };

			try
			{
				accountModel.InitData = HttpUtility.UrlDecode(RegexHelper.GetValueFromRegex("tgWebAppData=(.*?)&", accountModel.InitParam));
				accountModel.UserId = RegexHelper.GetValueFromRegex("id%2522%253A(.*?)%", accountModel.InitParam);
				_apiController = new DuckChain_Api(accountModel);


				//Get token ...
				_apiController.GetProfile();

				_apiController.HandleTask();




				Debug.WriteLine("Done");

			}
			catch
			{
				goto END;
			}

		END:
			MessageBox.Show("end");
		}

		private async void KiloEx()
		{
			KiloEx_Api _apiController = null;
			AccountModel accountModel = new AccountModel { InitParam = url };

			try
			{
				accountModel.InitData = HttpUtility.UrlDecode(RegexHelper.GetValueFromRegex("tgWebAppData=(.*?)&", accountModel.InitParam));
				accountModel.UserId = RegexHelper.GetValueFromRegex("id%2522%253A(.*?)%", accountModel.InitParam);
				_apiController = new KiloEx_Api(accountModel);


				//Get token ...
				_apiController.GetProfile();

				//Nhập referral
				_apiController.checkAndBindReferral();

				_apiController.ClaimOfflineCoins();

				_apiController.UpdateMining();



				_apiController.HandelLongShort();




				Debug.WriteLine("Done");

			}
			catch
			{
				goto END;
			}

		END:
			MessageBox.Show("end");
		}

		private async void FrogFarm()
		{
			FrogFarm_Api _apiController = null;
			AccountModel accountModel = new AccountModel { InitParam = url };

			try
			{
				accountModel.InitData = HttpUtility.UrlDecode(RegexHelper.GetValueFromRegex("tgWebAppData=(.*?)&", accountModel.InitParam));
				accountModel.UserId = RegexHelper.GetValueFromRegex("id%2522%253A(.*?)%", accountModel.InitParam);
				_apiController = new FrogFarm_Api(accountModel);


				//Get token ...
				_apiController.Login();

				_apiController.DailyRewards();

				//Nhập referral
				_apiController.HandelTask();

				_apiController.StartFarming();

				_apiController.ClaimReferral();

				_apiController.PlayGameFrog();


				Debug.WriteLine("Done");

			}
			catch
			{
				goto END;
			}

		END:
			MessageBox.Show("end");
		}

		private async void Vertus()
		{
			Vertus_Api _apiController = null;
			AccountModel accountModel = new AccountModel { InitParam = url };

			try
			{
				accountModel.InitData = HttpUtility.UrlDecode(RegexHelper.GetValueFromRegex("tgWebAppData=(.*?)&", accountModel.InitParam));
				accountModel.UserId = RegexHelper.GetValueFromRegex("id%2522%253A(.*?)%", accountModel.InitParam);
				_apiController = new Vertus_Api(accountModel);


				//Get token ...
				_apiController.GetProfile();


				_apiController.HandelTask();

				_apiController.ClaimDaily();

				_apiController.ClaimFarming();

				_apiController.HandleCard();

				_apiController.HandleUpgradeFarming();




				Debug.WriteLine("Done");

			}
			catch
			{
				goto END;
			}

		END:
			MessageBox.Show("end");
		}

		private async void Dropee()
		{
			Dropee_Api _apiController = null;
			AccountModel accountModel = new AccountModel { InitParam = url };

			try
			{
				accountModel.InitData = HttpUtility.UrlDecode(RegexHelper.GetValueFromRegex("tgWebAppData=(.*?)&", accountModel.InitParam));
				accountModel.UserId = RegexHelper.GetValueFromRegex("id%2522%253A(.*?)%", accountModel.InitParam);
				_apiController = new Dropee_Api(accountModel);


				//Get token ...
				accountModel.Token = _apiController.Login();

				_apiController.HandleDailyCheckin();

				//Làm các nv mạng xh
				_apiController.HandleTask();

				_apiController.HandleFortuneWheelSpins();

				//upgrade card để kiếm điểm khi offline, mỗi lần chạy là nâng cấp card lên 1 level
				_apiController.HandleUpgradeCard();

				_apiController.HandelTap();




				Debug.WriteLine("Done");

			}
			catch
			{
				goto END;
			}

		END:
			MessageBox.Show("end");
		}

		private async void Ducks()
		{
			Duck_Api _apiController = null;
			AccountModel accountModel = new AccountModel { InitParam = url };

			try
			{
				accountModel.InitData = HttpUtility.UrlDecode(RegexHelper.GetValueFromRegex("tgWebAppData=(.*?)&", accountModel.InitParam));
				accountModel.UserId = RegexHelper.GetValueFromRegex("id%2522%253A(.*?)%", accountModel.InitParam);
				_apiController = new Duck_Api(accountModel);


				//Get token ...
				//login lúc được lúc ko
				accountModel.Token = _apiController.Login();

				if(accountModel.Token=="") goto END;
				_apiController.DailyCheckin();

				 _apiController.HandleTask();




				

			}
			catch
			{
				goto END;
			}

		END:
			MessageBox.Show("end");
		}
	}
}
