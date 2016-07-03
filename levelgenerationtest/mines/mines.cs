/*
 * Pixel Dungeon
 * Copyright (C) 2012-2015  Oleg Dolya
 *
 * Shattered Pixel Dungeon
 * Copyright (C) 2014-2015 Evan Debenham
 *
 * Unpixel Dungeon
 * Copyright (C) 2015-2016 Randall Foudray
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace levelgenerationtest
{
    public class mines : Level
    {
        private double value, xscale, yscale, offset;

        public mines(double value, double xscale, double yscale, double offset)
        {
            this.value = value;
            this.xscale = xscale;
            this.yscale = yscale;
            this.offset = offset;

            simplexplain();
        }

        private static double noisefunc(mines what, int x, int y)
        {
            return SimplexNoise.noise((double)x / (double)WIDTH * what.xscale + what.offset, (double)y / (double)HEIGHT * what.yscale);
        }

        private void simplexplain()
        {
            int pos = 0;

            bool[] tested = new bool[LENGTH];

            for (int n = 0; n < tested.Length; n++)
            {
                tested[n] = false;
            }

            for (int y = 0; y < EDGEBUFFER; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    pos = x + y * WIDTH;
                    tested[pos] = true;

                    pos = (WIDTH - 1 - x) + y * WIDTH;
                    tested[pos] = true;

                    pos = x + (HEIGHT - 1 - y) * WIDTH;
                    tested[pos] = true;

                    pos = (WIDTH - 1 - x) + (HEIGHT - 1 - y) * WIDTH;
                    tested[pos] = true;
                }
            }

            for (int x = 0; x < EDGEBUFFER; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    pos = x + y * WIDTH;
                    tested[pos] = true;

                    pos = (WIDTH - 1 - x) + y * WIDTH;
                    tested[pos] = true;

                    pos = x + (HEIGHT - 1 - y) * WIDTH;
                    tested[pos] = true;

                    pos = (WIDTH - 1 - x) + (HEIGHT - 1 - y) * WIDTH;
                    tested[pos] = true;
                }
            }

            /*
            pos = 0;
            for (int y = EDGEBUFFER * 2; y < HEIGHT - EDGEBUFFER * 2; y++)
            {
                pos = y * WIDTH + EDGEBUFFER * 2;
                for (int x = EDGEBUFFER * 2; x < WIDTH - EDGEBUFFER * 2; x++)
                {
                    grid[pos] = noisefunc(this, x, y) >= value;

                    pos++;
                }
            }
            */

            for (int y = EDGEBUFFER * 2; y < HEIGHT - EDGEBUFFER * 2; y++)
            {
                int x = EDGEBUFFER * 2;

                simplexrecurse_method(x, y, tested);

                x = WIDTH - 1 - EDGEBUFFER * 2;

                simplexrecurse_method(x, y, tested);
            }

            for (int x = EDGEBUFFER * 2; x < WIDTH - EDGEBUFFER * 2; x++)
            {
                int y = EDGEBUFFER * 2;

                simplexrecurse_method(x, y, tested);

                y = HEIGHT - 1 - EDGEBUFFER * 2;

                simplexrecurse_method(x, y, tested);
            }
        }

        private void simplexrecurse()
        {
            //0.05, 5.0

            int startx = WIDTH / 2;
            int starty = HEIGHT / 2;

            Random rand = new Random();

            int x = startx + rand.Next(WIDTH / 8) - WIDTH / 16;
            int y = starty + rand.Next(HEIGHT / 8) - HEIGHT / 16;

            while (noisefunc(this, x, y) < value)
            {
                x = startx + rand.Next(WIDTH / 8) - WIDTH / 16;
                y = starty + rand.Next(HEIGHT / 8) - HEIGHT / 16;
            }

            bool[] tested = new bool[LENGTH];

            for (int n = 0; n < tested.Length; n++)
            {
                tested[n] = false;
            }

            simplexrecurse_method(x, y, tested);
        }

        private void simplexrecurse_method(int x, int y, bool[] tested)
        {
            int pos = x + y * WIDTH;
            if (pos < 0 || pos >= LENGTH || x < 0 || x >= WIDTH || tested[pos])
            {
                return;
            }

            if (noisefunc(this, x, y) >= value)
            {
                grid[pos] = ' ';
                tested[pos] = true;

                for (int suby = -1; suby <= 1; suby++)
                {
                    for (int subx = -1; subx <= 1; subx++)
                    {
                        if (subx == 0 && suby==0)
                        {
                            continue;
                        }

                        simplexrecurse_method(x + subx, y + suby, tested);
                    }
                }
            }
        }
    }
}
