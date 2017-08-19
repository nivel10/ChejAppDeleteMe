namespace ChejAppDeleteMe.Infrastructure
{
    using ChejAppDeleteMe.ViewModels;

    public class InstanceLocator
    {
        #region Properties

        private MainViewModel Main
        {
            get;
            set;
        }

        #endregion Properties

        #region Constructor

        public InstanceLocator()
        {
            Main = new MainViewModel();
        }

        #endregion Constructor
    }
}
