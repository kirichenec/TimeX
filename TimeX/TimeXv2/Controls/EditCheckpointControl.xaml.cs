using MaterialDesignThemes.Wpf;
using System;
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

        #region Properties

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

        #region EditedStartTime
        public DateTime EditedStartTime
        {
            get { return (DateTime)GetValue(EditedStartTimeProperty); }
            set { SetValue(EditedStartTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditedDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditedStartTimeProperty =
            DependencyProperty.Register(nameof(EditedStartTime), typeof(DateTime), typeof(EditCheckpointControl), new PropertyMetadata(DateTime.MinValue));
        #endregion

        #region EditedDuration
        public DateTime EditedDuration
        {
            get { return (DateTime)GetValue(EditedDurationProperty); }
            set { SetValue(EditedDurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditedTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditedDurationProperty =
            DependencyProperty.Register(nameof(EditedDuration), typeof(DateTime), typeof(EditCheckpointControl), new PropertyMetadata(DateTime.MinValue));
        #endregion

        #endregion

        #region Methods

        #region SaveDialog
        private void SaveDialog(object sender, RoutedEventArgs e)
        {
            this.EditedChckpnt.Duration = TimeSpan.FromTicks(this.EditedDuration.Ticks);
            this.EditedChckpnt.StartTime = TimeSpan.FromTicks(this.EditedStartTime.Ticks);
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
        #endregion

        #region Root_Loaded
        private void Root_Loaded(object sender, RoutedEventArgs e)
        {
            this.EditedChckpnt = new Checkpoint(this.Checkpoint);
            this.EditedDuration = new DateTime(this.EditedChckpnt.Duration.Ticks);
            this.EditedStartTime = new DateTime(this.EditedChckpnt.StartTime.Ticks);
        }
        #endregion

        #endregion
    }
}
