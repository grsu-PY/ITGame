using System;
using System.Threading;
using System.Threading.Tasks;

namespace ITGame.UIElements
{
    public class ProgressBar
    {
        private bool _run;
        private Task _task;
        private readonly string[] lines = { "\\ ", "| ", "/ ", "-" };

        public void StartProgress()
        {
            _run = true;
            _task = Task.Run(() => Counter());
        }

        public async void StopProgress()
        {
            _run = false;
            await _task;
        }
        private void Counter()
        {
            int i = 0;
            int len = lines.Length;
            while (_run)
            {
                Thread.Sleep(75);
                Console.Write("\r{0}", lines[i++ % len]);
            }

        }
    }
}
