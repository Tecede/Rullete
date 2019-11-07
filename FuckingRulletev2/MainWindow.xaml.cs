// TODO: 3. Поворот стрелки и выбор вопросов
// TODO: 4. Ускорить поворот стрелки
// TODO: 5. Исчезание вопросов после выбора (ХАРД)

using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace FuckingRulletev2
{
    public partial class MainWindow : Window
    {
        static int r = 0;
        int rl = 0, Counter = 0, QuestionNumber = 0;
        bool Ready = false;
        List<int> Questions = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };

        SoundPlayer StartMusic = new SoundPlayer("msc/Start.wav");
        SoundPlayer QuestionMusic = new SoundPlayer("msc/QuestionSound.wav");
        SoundPlayer TenSecMusic = new SoundPlayer("msc/Questions10sec.wav");

        TextWindow textWindow = new TextWindow();

        public MainWindow()
        {
            KeyDown += Space_KeyDown;
            InitializeComponent(); 
            
            Shuffle(Questions);
        }

        private void Space_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space && !Ready && Questions.Count > 0)
            {
                StartMusic.Play();

                if(Questions[0]<=10) // TODO: Баг поворота стрелки, логика на бумаге
                    rl = (Questions[0] + 9) * 18;
                else
                    rl = Math.Abs(Questions[0] - 10) * 18;

                Thread thread = new Thread(new ThreadStart(Generate));
                thread.Start();

                QuestionNumber = Questions[0];
                Questions.RemoveAt(0);
            }

            if(e.Key == Key.Space && Ready)
            {
                QuestionMusic.Play();
                ShowText(QuestionNumber);

                Thread thread2 = new Thread(new ThreadStart(Question));
                thread2.Start();
            }
        }

        public void Generate()
        {
            while (r != rl) // Counter < 2 ||
            {
                Thread.Sleep(10);

                r++;

                if (r == 360)
                {
                    r = 0;
                    Counter++;
                }

                Dispatcher.Invoke(() =>
                {
                    Arrow.RenderTransform = new RotateTransform(r);
                });
            }

            StartMusic.Stop();
            Ready = true;
        }

        public void Shuffle(List<int> arr)
        {
            Random rand = new Random();

            for(int i = arr.Count-1; i >=1; i--)
            {
                int j = rand.Next(i + 1);

                var tmp = arr[j];
                arr[j] = arr[i];
                arr[i] = tmp;
            }
        }

        public void Question()
        {
            for (int i = 3; i > 0; i--) // TODO: Return to 60 sec
            {
                Thread.Sleep(1000);

                if(i == 11)
                {
                    TenSecMusic.Play();
                }
            }

            Dispatcher.Invoke(() =>
            {
                textWindow.Hide();
            });

            Ready = false;
            Counter = 0;
        }

        public void ShowText(int Question)
        {
            // TODO: Абсолютный путь
            textWindow.questionText.Text = File.ReadAllText($"C:/Users/Tecede/source/repos/FuckingRulletev2/FuckingRulletev2/bin/Debug/qst/q{Question}.txt");
            textWindow.Show();
        }
    }
}