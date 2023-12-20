using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace testWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
     public partial class MainWindow
    {
        Grid grid;
        public MainWindow()
        {
            InitializeComponent();

            grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());

            // 添加输入框
            for (int i = 0; i < 3; i++)
            {
                var textBox = new TextBox();
                Grid.SetColumn(textBox, 1);
                Grid.SetRow(textBox, i);
                grid.Children.Add(textBox);
            }

            // 添加按钮
            var button = new Button();
            button.Content = "Click me";
            Grid.SetColumn(button, 1);
            Grid.SetRow(button, 3);
            grid.Children.Add(button);
            button.Click += ButtonClick;
            // 添加表格到窗口
            this.Content = grid;
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            List<string> inputValues = new List<string>();
            for (int i = 0; i < grid.Children.Count; i++)
            {
                if (grid.Children[i] is TextBox textBox)
                {
                    inputValues.Add(textBox.Text);
                }
            }

            Console.WriteLine("done");
        }
    }
}