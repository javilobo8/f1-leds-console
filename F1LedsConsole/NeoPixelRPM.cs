using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F1LedsConsole
{
    class NeoPixelRPM
    {
        public static UInt32 C_RED = 16711680;
        public static UInt32 C_BLUE = 255;
        public static UInt32 C_GREEN = 65280;
        public static UInt32 C_BLACK = 0;

        public static UInt32[] F1_LED_LAYOUT = {
            C_BLACK, C_GREEN, C_GREEN, C_GREEN, C_GREEN, C_GREEN, C_RED, C_RED,
            C_RED, C_RED, C_RED, C_BLUE, C_BLUE, C_BLUE, C_BLUE, C_BLUE
        };

        public int led_count;
        public int brightness;
        public float rpm_min;
        public float rpm_max;
        public float rpm_diff;
        public UInt32[] LED_BUFFER;

        public NeoPixelRPM(int _led_count, int _brightness, float _rpm_min, float _rpm_max)
        {
            led_count = _led_count;
            brightness = _brightness;
            rpm_min = _rpm_min;
            rpm_max = _rpm_max;
            rpm_diff = _rpm_max - _rpm_min;
            LED_BUFFER = new UInt32[_led_count];
        }

        public UInt32[] CalcRGBLeds(float rpm)
        {
            float percent = (rpm - rpm_min) * 100f / rpm_diff;
            int leds_to_show = (int)Math.Ceiling(led_count * percent / 100f);

            for (int current_led = 0; current_led < led_count; current_led++)
            {
                if (current_led < leds_to_show)
                    LED_BUFFER[current_led] = F1_LED_LAYOUT[current_led];
                else
                    LED_BUFFER[current_led] = C_BLACK;
            }
            return LED_BUFFER;
        }

        public UInt32 changeBrightness(UInt32 c, int brightness)
        {
            UInt32 r = (byte)(c >> 16);
            UInt32 g = (byte)(c >> 8);
            UInt32 b = (byte)c;
            r = (UInt32)(r * brightness) >> 8;
            g = (UInt32)(g * brightness) >> 8;
            b = (UInt32)(b * brightness) >> 8;
            UInt32 rgb = r;
            rgb = (rgb << 8) + g;
            rgb = (rgb << 8) + b;
            return rgb;
        }
    }
}
