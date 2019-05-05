using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using TimeXv2.Extensions;
using TimeXv2.Model;

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

        #region Properties

        #region Checkpoint
        public Checkpoint Checkpoint
        {
            get { return (Checkpoint)GetValue(CheckpointProperty); }
            set { SetValue(CheckpointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Checkpoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CheckpointProperty =
            DependencyProperty.Register(
                nameof(Checkpoint),
                typeof(Checkpoint),
                typeof(EditCheckpointControl),
                new PropertyMetadata(new Checkpoint()));
        #endregion

        #region EditedChckpnt
        public Checkpoint EditedChckpnt
        {
            get { return (Checkpoint)GetValue(EditedChckpntProperty); }
            set { SetValue(EditedChckpntProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditedChckpnt.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditedChckpntProperty =
            DependencyProperty.Register(
                nameof(EditedChckpnt),
                typeof(Checkpoint),
                typeof(EditCheckpointControl),
                new PropertyMetadata(new Checkpoint()));
        #endregion

        #region EditedStartTime
        public SpecialTimeSpan EditedStartTime
        {
            get { return (SpecialTimeSpan)GetValue(EditedStartTimeProperty); }
            set { SetValue(EditedStartTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditedStartTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditedStartTimeProperty =
            DependencyProperty.Register(
                nameof(EditedStartTime),
                typeof(SpecialTimeSpan),
                typeof(EditCheckpointControl),
                new PropertyMetadata(new SpecialTimeSpan(0)));
        #endregion

        #region EditedDuration
        public SpecialTimeSpan EditedDuration
        {
            get { return (SpecialTimeSpan)GetValue(EditedDurationProperty); }
            set { SetValue(EditedDurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditedDuration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditedDurationProperty =
            DependencyProperty.Register(
                nameof(EditedDuration),
                typeof(SpecialTimeSpan),
                typeof(EditCheckpointControl),
                new PropertyMetadata(new SpecialTimeSpan(0)));
        #endregion

        #endregion

        #region Methods

        #region SaveDialog
        private void SaveDialog(object sender, RoutedEventArgs e)
        {
            this.EditedChckpnt.Duration = EditedDuration.AsTimeSpan;
            this.EditedChckpnt.StartTime = EditedStartTime.AsTimeSpan;
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
        #endregion

        #region Root_Loaded
        private void Root_Loaded(object sender, RoutedEventArgs e)
        {
            this.EditedChckpnt = new Checkpoint(this.Checkpoint, true);
            this.EditedDuration = new SpecialTimeSpan(this.EditedChckpnt.Duration);
            this.EditedStartTime = new SpecialTimeSpan(this.EditedChckpnt.StartTime);
        }
        #endregion

        #endregion
    }
}
