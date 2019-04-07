using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using TimeXv2.Model;
using UniversalKLibrary.Classic.Helpers;

namespace TimeXv2.Controls
{
    /// <summary>
    /// Логика взаимодействия для EditCheckpoint.xaml
    /// </summary>
    public partial class EditCheckpointControl : UserControl
    {
        #region ctor
        public EditCheckpointControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Checkpoint
        public Checkpoint Checkpoint
        {
            get { return (Checkpoint)GetValue(CheckpointProperty); }
            set { SetValue(CheckpointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Checkpoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CheckpointProperty =
            DependencyProperty.Register(nameof(Checkpoint), typeof(Checkpoint), typeof(EditCheckpointControl), new PropertyMetadata(new Checkpoint()));
        #endregion

        #region EditedChckpnt
        public Checkpoint EditedChckpnt
        {
            get { return (Checkpoint)GetValue(EditedChckpntProperty); }
            set { SetValue(EditedChckpntProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditedChckpnt.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditedChckpntProperty =
            DependencyProperty.Register(nameof(EditedChckpnt), typeof(Checkpoint), typeof(EditCheckpointControl), new PropertyMetadata(new Checkpoint()));
        #endregion

        #region Methods
        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void Root_Loaded(object sender, RoutedEventArgs e)
        {
            this.EditedChckpnt = new Checkpoint(this.Checkpoint);
        }
        #endregion
    }
}
