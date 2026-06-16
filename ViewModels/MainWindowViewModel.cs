using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Platform;
using Avalonia.Threading;
using NAudio.Wave;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Media;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning; // 添加此行
namespace Timer2.ViewModels {
    public partial class MainWindowViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propName = null)=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        private bool _isContentMode;
        private bool _isButtonMode = true;
        private string _contentText = string.Empty;
        private DispatcherTimer? _countdownTimer;
        private int _remainingSeconds;
        public bool IsContentMode {
            get => _isContentMode;
            set { _isContentMode = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsButtonMode)); }
        }

        public bool IsButtonMode => !IsContentMode;

        public string ContentText {
            get => _contentText;
            set { _contentText = value; OnPropertyChanged(); }
        }

        public void OnSelectButton(string param) {
            IsContentMode = true;
            if (int.TryParse(param, out int minutes)) {
                _remainingSeconds = minutes * 60; // 分钟转秒
            } else {
                _remainingSeconds = 0; // 解析失败则默认为 0
            }
            UpdateCountdownText();
            if (_countdownTimer == null) {
                _countdownTimer = new DispatcherTimer();
                _countdownTimer.Interval = TimeSpan.FromSeconds(1); // 间隔 1 秒
                _countdownTimer.Tick += OnTimerTick;               // 绑定触发事件
            }
            _countdownTimer.Start();
        }
        private void OnTimerTick(object? sender, EventArgs e) {
            if (_remainingSeconds > 0) {
                _remainingSeconds--; // 秒数减 1
                UpdateCountdownText(); // 刷新界面
            } else {
                // 倒计时结束，停止定时器
                _countdownTimer?.Stop();
                ContentText = "00:00:00";
                // 播放提示音或执行其他操作
                PlaySound();
            }
        }

        private void PlaySound() {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "avares://Timer2/Assets/tips.wav";
            var assetUri = new Uri(resourceName);
            using (var stream = AssetLoader.Open(assetUri)) {
                if (stream != null) {
                    using (var player = new WaveOutEvent()) {
                        using (var audioFile = new WaveFileReader(stream)) {
                            player.Init(audioFile);
                            player.Play();
                            while (player.PlaybackState == PlaybackState.Playing) {
                                System.Threading.Thread.Sleep(100);
                            }
                        }
                    }
                }
            }
        }


        public void OnSubmit() {
            ContentText = string.Empty;
            IsContentMode = false;
            _countdownTimer?.Stop();
        }
        private void UpdateCountdownText() {
            TimeSpan time = TimeSpan.FromSeconds(_remainingSeconds);
            ContentText = time.ToString(@"hh\:mm\:ss");
        }

    }
}