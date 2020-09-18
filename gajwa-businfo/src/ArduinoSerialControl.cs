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
            arduinoPort.PortName = ComPort_;    
            arduinoPort.BaudRate = 9600;       
            arduinoPort.Open();

            ComPort = ComPort_;

            d.write($"[ArduinoSerialControl] port open at {ComPort}");
        }

        public static void CloseConnection()
        {
            d.write($"[ArduinoSerialControl] connection with {ComPort} closed");
            arduinoPort.Close();
        }


        public static void ToggleTVpower()
        {
            d.write($"[ArduinoSerialControl] toggle signal sent to {ComPort}");
            arduinoPort.Write(base_.ARDUINO_SERIAL_POWER_TOGGLE_CHAR.ToString());
        }


    }
}
