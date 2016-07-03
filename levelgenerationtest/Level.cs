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
    public class Level
    {
        public const int WIDTH = 97;
        public const int HEIGHT = 31;
        public const int LENGTH = WIDTH * HEIGHT;
        public const int EDGEBUFFER = 7;

        public char[] grid = new char[LENGTH];

        public Level()
        {
            for (int n = 0; n < LENGTH; n++)
            {
                grid[n] = '█';
            }
        }

        public override string ToString()
        {
            StringBuilder retval = new StringBuilder();

            int pos = 0;
            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    retval.Append(grid[pos]);
                    pos++;
                }

                retval.Append(Environment.NewLine);
            }

            return retval.ToString();
        }
    }
}
