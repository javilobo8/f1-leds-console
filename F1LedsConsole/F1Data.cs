using System;
using System.Collections.Generic;

namespace F1LedsConsole
{
    class F1Data
    {
        public static Dictionary<String, int> VALUES = new Dictionary<String, int>() {
            {"time", 0},
            {"lapTime", 1},
            {"lapDistance", 2},
            {"totalDistance", 3},
            {"x", 4},
            {"y", 5},
            {"z", 6},
            {"speed", 7},
            {"xv", 8},
            {"yv", 9},
            {"zv", 10},
            {"xr", 11},
            {"yr", 12},
            {"zr", 13},
            {"xd", 14},
            {"yd", 15},
            {"zd", 16},
            {"susp_pos_bl", 17},
            {"susp_pos_br", 18},
            {"susp_pos_fl", 19},
            {"susp_pos_fr", 20},
            {"susp_vel_bl", 21},
            {"susp_vel_br", 22},
            {"susp_vel_fl", 23},
            {"susp_vel_fr", 24},
            {"wheel_speed_bl", 25},
            {"wheel_speed_br", 26},
            {"wheel_speed_fl", 27},
            {"wheel_speed_fr", 28},
            {"throttle", 29},
            {"steer", 30},
            {"brake", 31},
            {"clutch", 32},
            {"gear", 33},
            {"gforce_lat", 34},
            {"gforce_lon", 35},
            {"lap", 36},
            {"engineRate", 37},
            {"sli_pro_native_support", 38},
            {"car_position", 39},
            {"kers_level", 40},
            {"kers_max_level", 41},
            {"drs", 42},
            {"traction_control", 43},
            {"anti_lock_brakes", 44},
            {"fuel_in_tank", 45},
            {"fuel_capacity", 46},
            {"in_pits", 47},
            {"sector", 48},
            {"sector1_time", 49},
            {"sector2_time", 50},
            {"brakes_temp1", 51},
            {"brakes_temp2", 52},
            {"brakes_temp3", 53},
            {"brakes_temp4", 54},
            {"wheels_pressure1", 55},
            {"wheels_pressure2", 56},
            {"wheels_pressure3", 57},
            {"wheels_pressure4", 58},
            {"team_info", 59},
            {"total_laps", 60},
            {"track_size", 61},
            {"last_lap_time", 62},
            {"max_rpm", 63},
            {"idle_rpm", 64},
            {"max_gears", 65},
            {"sessionType", 66},
            {"drsAllowed", 67},
            {"track_number", 68},
            {"vehicleFIAFlags", 69},
        };
        private byte[] packet;
        public F1Data(byte[] packet)
        {
            this.packet = packet;
        }

        public float Get(String key)
        {
            int index = VALUES[key] * 4;
            return BitConverter.ToSingle(packet, index);
        }
    }
}
