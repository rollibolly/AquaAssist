using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.Generic;
using System.ComponentModel;

namespace AquaAssist.Mobile.ViewModels
{
    public class ChartViewModel : INotifyPropertyChanged
    {
        private PlotModel chartData;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public PlotModel ChartData
        {
            get => chartData;
            set
            {
                chartData = value;
                OnPropertyChanged(nameof(ChartData));
            }
        }

        public ChartViewModel()
        {
            CategoryAxis xaxis = new CategoryAxis();
            xaxis.Position = AxisPosition.Bottom;
            xaxis.Minimum = 15;
            xaxis.Maximum = 25;
            xaxis.MajorGridlineStyle = LineStyle.Solid;
            xaxis.MinorGridlineStyle = LineStyle.Dot;
            xaxis.MajorGridlineColor = OxyColors.LightGray;
            xaxis.MinorGridlineColor = OxyColors.LightGray;
            
            LinearAxis yaxis = new LinearAxis();
            yaxis.Position = AxisPosition.Left;
            yaxis.Minimum = 20;
            yaxis.Maximum = 30;
            yaxis.MajorGridlineStyle = LineStyle.Dot;
            xaxis.MinorGridlineStyle = LineStyle.Dot;
            xaxis.MajorGridlineColor = OxyColors.LightGray;
            xaxis.MinorGridlineColor = OxyColors.LightGray;

            AreaSeries s1 = new AreaSeries();
            s1.ItemsSource = new List<DataPoint>
            {
                new DataPoint(15, 20),
                new DataPoint(16, 22),
                new DataPoint(17, 26),
                new DataPoint(18, 23),
                new DataPoint(19, 29),
                new DataPoint(20, 24.6),
                new DataPoint(21, 25),
                new DataPoint(24, 20)
            };
            s1.Color = OxyColors.Orange;
            s1.LineStyle = LineStyle.Solid;
            
            ChartData = new PlotModel();
            ChartData.Title = "Temperature";
            ChartData.Background = OxyColors.White;
            ChartData.TitleColor = OxyColors.LightGray;
            ChartData.PlotAreaBorderColor = OxyColors.White;

            ChartData.Axes.Add(xaxis);
            ChartData.Axes.Add(yaxis);
            ChartData.Series.Add(s1);
        }
    }
}
