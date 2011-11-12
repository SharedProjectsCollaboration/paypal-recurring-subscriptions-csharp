using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace PayPalRecurring
{
	public class PPRecurring
	{
		public bool test { get; set; } // By default we will contact the sandbox server. developer.paypal.com
        public string user { get; set; } // API account from PayPal
        public string password { get; set; } // API password
        public string signature { get; set; } // API Signature
		public string countryCode { get; set; }
		public string billingPeriod { get; set; }
		public int billingFrequency { get; set; }
		public double amount { get; set; }
		public double initAmount { get; set; }
		public double taxAmount { get; set; }
		public string description { get; set; }
		public string currencyCode { get; set; }

		public string creditCardType { get; set; }
		public string creditCardNumber { get; set; }
		public int expirationYear { get; set; }
		public int expirationMonth { get; set; }
		public string firstName { get; set; }
		public string lastName { get; set; }
		public DateTime profileStartDate { get; set; }
		public int totalBillingCycles { get; set; }
		public string email { get; set; }
		public string payerStatus { get; set; }

		public string lastResponse { get; private set; }
		public string lastErrorResponse { get; private set; }
		public double taxPercent { get; set; }

		private string livePPServer = "https://api-3t.paypal.com/nvp";
		private string testPPServer = "https://api-3t.sandbox.paypal.com/nvp";
		private string methodName = "CreateRecurringPaymentsProfile";
		private UriBuilder baseUri;

		public PPRecurring()
		{
			this.test = true;
			this.user = "";
			this.password = "";
			this.signature = "";
			this.countryCode = "US";
			this.billingPeriod = "Month";
			this.billingFrequency = 1;
			this.currencyCode = "USD";
			this.amount = 9.95;
			this.initAmount = 0.00;
			this.taxPercent = 0.07;
			this.taxAmount = this.amount * this.taxPercent;
			this.description = "Default Package Name";
			this.creditCardType = "Visa";
			this.creditCardNumber = "";
			this.expirationMonth = 1;
			this.expirationYear = DateTime.Now.Year + 1;
			this.firstName = "";
			this.lastName = "";
			this.profileStartDate = DateTime.Now;
			this.totalBillingCycles = 12;
			this.email = "";
			this.payerStatus = "verified";
		}

		public PPRecurring(bool test, string user, string password, string signature, 
			double amount, string creditCardType,  string creditCardNumber,
			int cardExpirationMonth, int cardExpirationYear, string firstName, string lastName,
			string email)
			: this()
		{
			this.test = test;
			this.user = user;
			this.password = password;
			this.signature = signature;
			this.amount = amount;
			this.creditCardType = creditCardType;
			this.creditCardNumber = creditCardNumber;
			this.expirationMonth = cardExpirationMonth;
			this.expirationYear = cardExpirationYear;
			this.firstName = firstName;
			this.lastName = lastName;
			this.email = email;
		}

		private void addQuery(string parameter, string value)
		{
			string queryToAppend = parameter+"="+value;

			if(this.baseUri.Query != null && this.baseUri.Query.Length > 1)
				this.baseUri.Query = this.baseUri.Query.Substring(1) + "&" + queryToAppend; 
			else
				this.baseUri.Query = queryToAppend;
		}

		public void sendSubscriptionRequest()
		{
			string baseUri_ = (this.test) ? this.testPPServer : this.livePPServer;
			this.baseUri = new UriBuilder(baseUri_);

			this.addQuery("METHOD", this.methodName);
			this.addQuery("VERSION", "56.0");
			this.addQuery("user", this.user);
			this.addQuery("pwd", this.password);
			this.addQuery("signature", this.signature);
			this.addQuery("countrycode", this.countryCode);
			this.addQuery("billingperiod", this.billingPeriod);
			this.addQuery("billingfrequency", this.billingFrequency.ToString());
			this.addQuery("currencycode", this.currencyCode);
			this.addQuery("amt", this.amount.ToString());
			this.addQuery("initamt", this.initAmount.ToString());
			this.addQuery("taxamt", this.taxAmount.ToString());
			this.addQuery("desc", this.description);
			this.addQuery("creditcardtype", this.creditCardType);
			this.addQuery("acct", this.creditCardNumber);
			this.addQuery("expdate", this.expirationMonth.ToString().PadLeft(2, '0')+this.expirationYear.ToString());
			this.addQuery("firstname", this.firstName);
			this.addQuery("lastname", this.lastName);
			this.addQuery("profilestartdate", this.profileStartDate.ToString());
			this.addQuery("totalbillingcycles", this.totalBillingCycles.ToString());
			this.addQuery("email", this.email);
			this.addQuery("payerstatus", this.payerStatus);

			HttpWebRequest request = (HttpWebRequest) WebRequest.Create(baseUri.Uri);
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();

			using (var sr = new StreamReader(response.GetResponseStream()))
			{
				this.lastResponse = Uri.UnescapeDataString(sr.ReadToEnd());
			}
		}
	}
}
