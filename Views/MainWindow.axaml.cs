using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using System;

namespace Timer2.Views {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            this.MinWidth = 290;
            this.MaxWidth = 290;
            this.MinHeight = 25;
            this.MaxHeight = 25;
            this.Height = 25;
        }
        private void OnTopButton(object? sender, RoutedEventArgs e) {
            if (sender is ToggleButton toggleButton) {
                // 根据 ToggleButton 的选中状态，动态开启或关闭窗口置顶
                Topmost = toggleButton.IsChecked ?? false;
            }
        }
        private void OnGridPointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e) {
            // 确保是鼠标左键按下的事件
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) {
                this.BeginMoveDrag(e);
            }
        }
        private void OnExitButton(object? sender, RoutedEventArgs e) {
            Environment.Exit(0);
        }
    }
}