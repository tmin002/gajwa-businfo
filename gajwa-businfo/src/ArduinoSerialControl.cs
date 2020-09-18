using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Runtime.InteropServices;

namespace gajwa_businfo
{

    public static class ArduinoSerialControl
    {

        private static SerialPort arduinoPort = new SerialPort();
        private static string ComPort = "";

        public static void OpenConnection(string ComPort_)
        {
            arduinoPort.PortName = ComPort;    //아두이노가 연결된 시리얼 포트 번호 지정
            arduinoPort.BaudRate = 9600;       //시리얼 통신 속도 지정
            arduinoPort.Open();

            ComPort = ComPort_;

            d.write($"[ArduinoSerialControl] port open at {ComPort}");
        }

        public static void CloseConnection() => arduinoPort.Close();


        public static void ToggleTVpower()
        {
            d.write($"[ArduinoSerialControl] toggle signal sent to {ComPort}");
            arduinoPort.Write(base_.ARDUINO_SERIAL_POWER_TOGGLE_CHAR.ToString());
        }


    }
}
