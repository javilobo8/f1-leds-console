using System.Runtime.InteropServices;

namespace F1LedsConsole
{
    class F1DataStruct
    {
        public static F1Data Convert(byte[] bytes)
        {
            F1Data stuff;
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try {
                stuff = (F1Data)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(F1Data));
            }
            finally {
                handle.Free();
            }
            return stuff;
        }

        public struct F1Data
        {
            public float time;
            public float lapTime;
            public float lapDistance;
            public float totalDistance;
            public float x;
            public float y;
            public float z;
            public float speed;
            public float xv;
            public float yv;
            public float zv;
            public float xr;
            public float yr;
            public float zr;
            public float xd;
            public float yd;
            public float zd;
            public float susp_pos_bl;
            public float susp_pos_br;
            public float susp_pos_fl;
            public float susp_pos_fr;
            public float susp_vel_bl;
            public float susp_vel_br;
            public float susp_vel_fl;
            public float susp_vel_fr;
            public float wheel_speed_bl;
            public float wheel_speed_br;
            public float wheel_speed_fl;
            public float wheel_speed_fr;
            public float throttle;
            public float steer;
            public float brake;
            public float clutch;
            public float gear;
            public float gforce_lat;
            public float gforce_lon;
            public float lap;
            public float engineRate;
            public float sli_pro_native_support;
            public float car_position;
            public float kers_level;
            public float kers_max_level;
            public float drs;
            public float traction_control;
            public float anti_lock_brakes;
            public float fuel_in_tank;
            public float fuel_capacity;
            public float in_pits;
            public float sector;
            public float sector1_time;
            public float sector2_time;
            public float brakes_temp_0;
            public float brakes_temp_1;
            public float brakes_temp_2;
            public float brakes_temp_3;
            public float wheels_pressure_0;
            public float wheels_pressure_1;
            public float wheels_pressure_2;
            public float wheels_pressure_3;
            public float teainfo;
            public float total_laps;
            public float track_size;
            public float last_lap_time;
            public float max_rpm;
            public float idle_rpm;
            public float max_gears;
            public float sessionType;
            public float drsAllowed;
            public float track_number;
            public float vehicleFIAFlags;
        }

    }
}
