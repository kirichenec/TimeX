using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;
using TimeXv2.Model;
using UniversalKLibrary.Classic.Simplificators;

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
        public SpecialTimeSpan EditedStartTime
        {
            get { return (SpecialTimeSpan)GetValue(EditedStartTimeProperty); }
            set { SetValue(EditedStartTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditedStartTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditedStartTimeProperty =
            DependencyProperty.Register(nameof(EditedStartTime), typeof(SpecialTimeSpan), typeof(EditCheckpointControl), new PropertyMetadata(new SpecialTimeSpan(0)));
        #endregion

        #region EditedDuration
        public SpecialTimeSpan EditedDuration
        {
            get { return (SpecialTimeSpan)GetValue(EditedDurationProperty); }
            set { SetValue(EditedDurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditedDuration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditedDurationProperty =
            DependencyProperty.Register(nameof(EditedDuration), typeof(SpecialTimeSpan), typeof(EditCheckpointControl), new PropertyMetadata(new SpecialTimeSpan(0)));
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
            this.EditedChckpnt = new Checkpoint(this.Checkpoint);
            this.EditedDuration = new SpecialTimeSpan(this.EditedChckpnt.Duration);
            this.EditedStartTime = new SpecialTimeSpan(this.EditedChckpnt.StartTime);
        }
        #endregion

        #endregion
    }

    public class SpecialTimeSpan : SimplePropertyChanged
    {
        #region ctor
        public SpecialTimeSpan(TimeSpan value)
        {
            this.Days = value.Days;
            this.Minutes = value.Minutes;
            this.Hours = value.Hours;
        }

        public SpecialTimeSpan(int ticks) : this(new TimeSpan(ticks))
        {
        }
        #endregion

        #region Properties

        #region Days
        private int _days;

        public int Days
        {
            get { return _days; }
            set
            {
                if (value < 0)
                {
                    _days = 0;
                }
                else
                {
                    _days = value;
                }
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(AsTimeSpan));
            }
        }
        #endregion

        #region Hours
        private int _hours;

        public int Hours
        {
            get { return _hours; }
            set
            {
                if (value < 0)
                {
                    if (this.Days > 0)
                    {
                        _hours = 24 + value;
                        this.Days -= 1;
                    }
                    else
                    {
                        _hours = 0;
                    }
                }
                else if (value > 23)
                {
                    _hours = 0;
                    this.Days += 1;
                }
                else
                {
                    _hours = value;
                }
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(AsTimeSpan));
            }
        }
        #endregion

        #region Minutes
        private int _minutes;

        public int Minutes
        {
            get { return _minutes; }
            set
            {
                if (value < 0)
                {
                    if (this.Hours > 0)
                    {
                        _minutes = 60 + value;
                        this.Hours -= 1;
                    }
                    else
                    {
                        _minutes = 0;
                    }
                }
                else if (value > 59)
                {
                    _minutes = 0;
                    this.Hours += 1;
                }
                else
                {
                    _minutes = value;
                }
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(AsTimeSpan));
            }
        }
        #endregion

        #region AsTimeSpan
        public TimeSpan AsTimeSpan
        {
            get { return new TimeSpan(this.Days, this.Hours, this.Minutes, 0); }
        }
        #endregion

        #endregion

        #region Methods

        #region ToString
        public override string ToString()
        {
            return this.AsTimeSpan.ToString();
        }
        #endregion

        #endregion
    }
}
