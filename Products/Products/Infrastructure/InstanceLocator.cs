namespace Products.Infrastructure
{
	using System;
	using Products.ViewModels;

    public class InstanceLocator
    {
		public MainViewModel Main
		{
			get;
			set;
		}

        public InstanceLocator()
        {
            Main = new MainViewModel();
        }
    }
}
