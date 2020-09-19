using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace caMon.pagemod.LCD4Passenger
{
	public class caMonIF : caMon.IPages
	{
		public Page FrontPage { get; } = new FrontPage();

		public event EventHandler BackToHome;
		public event EventHandler CloseApp;

		public void Dispose()
		{
			//throw new NotImplementedException();
		}
	}
}
