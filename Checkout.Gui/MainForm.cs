using System;
using System.Windows.Forms;
using Autofac;
using Checkout.Presentation;
using IContainer = Autofac.IContainer;

namespace Checkout.Gui
{
    public partial class MainForm : Form, IPresenter
    {
        private readonly IContainer _container;
        private readonly Invoker _invoker;

        public MainForm()
        {
            InitializeComponent();
            _invoker = new Invoker(RefreshControls);
            _container = TypeRegistry.Build(RegisterThis);
        }

        public void WarnLimitExceeded(decimal limit, decimal currentPrice)
        { }

        private void RegisterThis(ContainerBuilder builder) => builder.RegisterInstance(this).As<IPresenter>();

        private void MainForm_Load(object sender, EventArgs e) => RefreshControls();

        private void RefreshControls()
        {
            using var scope = _container.BeginLifetimeScope();
            var startCommand = scope.Resolve<StartCommand>();
            StartButton.Enabled = startCommand.CanExecute;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            using var scope = _container.BeginLifetimeScope();
            var startCommand = scope.Resolve<StartCommand>();
            _invoker.Invoke(startCommand);
        }
    }
}
