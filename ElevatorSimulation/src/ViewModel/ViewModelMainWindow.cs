using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;

using ElevatorSimulation.Model;
using ElevatorSimulation.Commands;
using ElevatorSimulation.Model.SimulationModel;
using ElevatorSimulation.Model.SimulationModel.Statistics;

namespace ElevatorSimulation.ViewModel
{
    /// <summary>
    /// View model of the main window
    /// </summary>
    class ViewModelMainWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /* Linked properties */
        public ObservableCollection<FloorData> Floors
        {
            get { return m_floors; }
            set
            {
                m_floors = value;
                OnPropertyChanged("Floors");
            }
        }
        public FloorData CurrentFloor
        {
            get { return m_currentFloor; }
            set { m_currentFloor = value; OnPropertyChanged("CurrentFloor"); }
        }

        public ObservableCollection<ElevatorData> Elevators
        {
            get { return m_elevators; }
            set
            {
                m_elevators = value;
                OnPropertyChanged("Elevators");
            }
        }
        public ElevatorData CurrentElevator
        {
            get { return m_currentElevator; }
            set { m_currentElevator = value; OnPropertyChanged("CurrentElevator"); }
        }

        public int Duration
        {
            get { return m_duration; }
            set { m_duration = value; OnPropertyChanged("Duration"); }
        }

        public bool IsShowEvents
        {
            get { return m_isShowEvents; }
            set { m_isShowEvents = value; OnPropertyChanged("IsShowEvents"); }
        }

        public ObservableCollection<string> EventsLog
        {
            get { return m_eventsLog; }
            set { m_eventsLog = value; OnPropertyChanged("EventsLog"); }
        }
        public ObservableCollection<SPair> Statistics
        {
            get { return m_statistics; }
            set { m_statistics = value; OnPropertyChanged("Statistics"); }
        }

        /* Commands */
        public ICommand AddFloorCommand { get; set; }
        public ICommand DeleteFloorCommand { get; set; }
        public ICommand ClearFloorsCommand { get; set; }
        public ICommand AddElevatorCommand { get; set; }
        public ICommand DeleteElevatorCommand { get; set; }
        public ICommand ClearElevatorsCommand { get; set; }
        public ICommand BuildCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand RunCommand { get; set; }

        public ViewModelMainWindow()
        {
            m_model = new MainModel();

            Floors = new ObservableCollection<FloorData>();
            CurrentFloor= new FloorData();
            Elevators = new ObservableCollection<ElevatorData>();
            CurrentElevator = new ElevatorData();
            EventsLog = new ObservableCollection<string>();

            // Set starting IDs
            CurrentFloor.ID = 1;
            CurrentElevator.ID = 1;

            IsShowEvents = true;

            AddFloorCommand = new RelayCommand(arg => AddFloorHandle());
            DeleteFloorCommand = new RelayCommand(arg => DeleteFloorHandle());
            ClearFloorsCommand = new RelayCommand(arg => ClearFloorsHandle());
            AddElevatorCommand = new RelayCommand(arg => AddElevatorHandle());
            DeleteElevatorCommand = new RelayCommand(arg => DeleteElevatorHandle());
            ClearElevatorsCommand = new RelayCommand(arg => ClearElevatorsHandle());
            BuildCommand = new RelayCommand(arg => BuildHandle());
            ResetCommand = new RelayCommand(arg => ResetHandle());
            RunCommand = new RelayCommand(arg => RunHandle());
        }

        /* Command handlers */
        private void AddFloorHandle()
        {
            try
            {
                CheckCurrentFloor();

                Floors.Add(new FloorData(CurrentFloor));
                CurrentFloor.ID++;
                OnPropertyChanged("CurrentFloor");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
            }
        }
        private void DeleteFloorHandle()
        {
            try
            {
                if (Floors.Count > 0)
                {
                    Floors.RemoveAt(Floors.Count - 1);
                    CurrentFloor.ID--;
                    OnPropertyChanged("CurrentFloor");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
            }
        }
        private void ClearFloorsHandle()
        {
            try
            {
                Floors.Clear();
                CurrentFloor.ID = 1;
                OnPropertyChanged("CurrentFloor");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
            }
        }
        private void AddElevatorHandle()
        {
            try
            {
                CheckCurrentElevator();

                Elevators.Add(new ElevatorData(CurrentElevator));
                CurrentElevator.ID++;
                OnPropertyChanged("CurrentElevator");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
            }
        }
        private void DeleteElevatorHandle()
        {
            try
            {
                if (Elevators.Count > 0)
                {
                    Elevators.RemoveAt(Elevators.Count - 1);
                    CurrentElevator.ID--;
                    OnPropertyChanged("CurrentElevator");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
            }
        }
        private void ClearElevatorsHandle()
        {
            try
            {
                Elevators.Clear();
                CurrentElevator.ID = 1;
                OnPropertyChanged("CurrentElevator");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
            }
        }
        private void BuildHandle()
        {
            try
            {
                // Checks
                if (Floors.Count < 2)
                {
                    throw new Exception("The simulation model must have at least 2 floors");
                }
                if (Elevators.Count < 1)
                {
                    throw new Exception("The simulation model must have at least 1 elevator");
                }

                ElevatorSimModel simModel = m_model.BuildModel(Floors.ToList(), Elevators.ToList());
                simModel.Log += AddEventLog;

                m_model.LinkStatistics(Floors.Count, Elevators.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
            }
        }
        private void ResetHandle()
        {
            //try
            //{
                // Reset model
                m_model.Reset();

                // Clear log
                EventsLog.Clear();

                // Clear statistics
                Statistics.Clear();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
            //}
        }
        private void RunHandle()
        {
            try
            {
                m_model.RunModel(Duration);
                Statistics = new ObservableCollection<SPair>(m_model.HandleStatistics());
            }
            catch (Exception ex)
            {
              MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
            }
        }

        private void AddEventLog(string text)
        {
            if (IsShowEvents)
            {
                EventsLog.Add(text);
            }
        }
        private void CheckCurrentFloor()
        {
            if (CurrentFloor.Period < 0 ||
                CurrentFloor.Spread < 0 ||
                CurrentFloor.Period < CurrentFloor.Spread)
            {
                throw new Exception("Incorrect values");
            }
        }
        private void CheckCurrentElevator()
        {
            if (CurrentElevator.StartFloor <= 0 ||
                CurrentElevator.Capacity < 0   ||
                CurrentElevator.Period < 0     ||
                CurrentElevator.Spread < 0     ||
                CurrentElevator.StartFloor > Floors.Count ||
                CurrentElevator.Period < CurrentElevator.Spread)
            {
                throw new Exception("Incorrect values");
            }
        }

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private ObservableCollection<FloorData> m_floors;
        private FloorData m_currentFloor;
        private ObservableCollection<ElevatorData> m_elevators;
        private ElevatorData m_currentElevator;
        private int m_duration;
        private bool m_isShowEvents;
        private ObservableCollection<string> m_eventsLog;
        private ObservableCollection<SPair> m_statistics;

        private readonly MainModel m_model;
    }
}
