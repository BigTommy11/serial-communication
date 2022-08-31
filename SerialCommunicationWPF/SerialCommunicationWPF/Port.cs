using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows;

namespace SerialConnectWPF
{
    class Port
    {
        private  SerialPort _serialPort = null;

        public bool PortOpen(string comNum)
        {
            _serialPort ??= new SerialPort
            {
                PortName = comNum,
                BaudRate = 9600,
                DataBits = 8,
                Parity = Parity.None,
                StopBits = StopBits.One,
                Handshake = Handshake.None,
                Encoding = Encoding.UTF8,
                WriteTimeout = 100000,
                ReadTimeout = 100000,
                NewLine = "\r\n",
            };

            // データ受信イベント設定
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(EventReciveAsync);

            if (!_serialPort.IsOpen)
            {
                try
                {
                    _serialPort.Open();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// ポート切断
        /// _serialPortがnull状態で実行するとエラーになるのでnull条件演算子で確認する
        /// </summary>
        public void PortClose()
        {
            _serialPort?.Close();
            _serialPort = null;
        }


        /// <summary>
        /// 電文送信
        /// </summary>
        /// <param name="serialTxt"></param>
        public void Send(string serialTxt)
        {
            // COMポートがオープン状態じゃないとエラーになるので確認し、閉じている場合は戻す
            if (!_serialPort.IsOpen) return;

            try
            {
                var tmp = serialTxt.Split("-").Select(hex => Convert.ToByte(hex, 16));
                byte[] sendData = tmp.ToArray();

                // データ書き込み
                _serialPort.Write(sendData, 0, sendData.Length);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        /// <summary>
        ///  データ受信用イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EventReciveAsync(object sender, SerialDataReceivedEventArgs e)
        {
            if (!_serialPort.IsOpen) return;

            var srtDataReceived = "";
            var buf = new byte[_serialPort.BytesToRead];

            try
            {
                _serialPort.Read(buf, 0, buf.Length);
                srtDataReceived = BitConverter.ToString(buf);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (_serialPort.IsOpen)
            {
                MessageBox.Show(srtDataReceived);
            }
        }


        /// <summary>
        /// PCが認識しているCOM番号の一覧を取得する
        /// </summary>
        /// <returns></returns>
        public string[] GetPortNum()
        {
            return SerialPort.GetPortNames();
        }

    }
}
