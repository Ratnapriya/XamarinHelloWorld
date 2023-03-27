using Abbott.Services.IMD;
using Abbott.Services.Platform.Event;
using Abbott.Services.Platform.Logging;
using Android.Bluetooth;
using Android.Content;
using PatientApp.Cloud.IoTHubMessages;
using PatientApp.PIM;
using System;

namespace PatientApp.Droid.BroadcastReceivers
{
    public class BluetoothBondChangeReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
#if !DEMO
            var bluetoothDevice = intent.GetParcelableExtra(BluetoothDevice.ExtraDevice) as BluetoothDevice;
            var deviceService = Abbott.Services.Platform.Common.ServiceLocator.ServiceUtil.Get<IDevice>();

            if (deviceService.BleIdentifier != null 
                && bluetoothDevice != null 
                && deviceService.BleIdentifier.Equals(bluetoothDevice.Address, StringComparison.InvariantCulture))
            {
                var bondState = intent.GetIntExtra(BluetoothDevice.ExtraBondState, -1);

                Log.Info(LogEnum.A0059, args: new object[] { bondState });

                if (bondState == (int) Bond.None)
                {
                    
                }
            }
#endif
        }
    }
}
