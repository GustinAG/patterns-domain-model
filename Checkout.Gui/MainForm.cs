using System;
using System.Windows.Forms;
using Autofac;
using Checkout.Domain.Checkout;
using Checkout.Presentation;
using IContainer = Autofac.IContainer;

namespace Checkout.Gui
{
    public partial class MainForm : Form, IPresenter
    {
        private readonly IContainer _container;

        public MainForm()
        {
            InitializeComponent();
            _container = TypeRegistry.Build(RegisterThis);
        }

        public void RefreshDisplay(BillAppearance appearance)
        {
            EnableControlBasedOnCommand<StartCommand>(StartButton);
            EnableControlBasedOnCommand<StopCommand>(StopButton);
            EnableControlBasedOnCommand<ScanCommand>(ScanButton);
            EnableControlBasedOnCommand<CancelCommand>(CancelItemButton);
            EnableControlBasedOnCommand<ScanCommand>(BarCodeTextBox);
            RefreshTexts(appearance);
        }

        public void ShowWarning(string message)
        {
        }

        public void ShowError(string message)
        {
        }

        private void RegisterThis(ContainerBuilder builder) => builder.RegisterInstance(this).As<IPresenter>();

        private void MainForm_Load(object sender, EventArgs e) => RefreshDisplay(new BillAppearance(Bill.NoBill));

        private void StartButton_Click(object sender, EventArgs e) => InvokeCommand<StartCommand>();

        private void StopButton_Click(object sender, EventArgs e) => InvokeCommand<StopCommand>();

        private void ScanButton_Click(object sender, EventArgs e) => InvokeCommand<ScanCommand>(c => c.Code = BarCodeTextBox.Text);

        private void CancelItemButton_Click(object sender, EventArgs e) => InvokeCommand<CancelCommand>(c => c.Code = BarCodeTextBox.Text);

        private void EnableControlBasedOnCommand<T>(Control control) where T : ICommand
        {
            using var scope = _container.BeginLifetimeScope();
            var command = scope.Resolve<T>();
            control.Enabled = command.CanExecute;
        }

        private void InvokeCommand<T>(Action<T> customAction = null) where T : ICommand
        {
            using var scope = _container.BeginLifetimeScope();
            var invoker = scope.Resolve<Invoker>();
            var command = scope.Resolve<T>();
            customAction?.Invoke(command);
            invoker.Invoke(command);
        }

        private void RefreshTexts(BillAppearance appearance)
        {
            BillTextBox.Text = appearance.AsText;
            BillTextBox.SelectionStart = BillTextBox.SelectionLength = 0;
            LastScannedLabel.Text = appearance.LastAddedProductAsText;
            BarCodeTextBox.Text = string.Empty;
            BarCodeTextBox.Focus();
        }
    }
}
