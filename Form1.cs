using Airdrop.Controller;
using Airdrop.Helper;
using Airdrop.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
	}
}
