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
    public class regularlevel : Level
    {
        public class Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public Rect(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            public Rect(Rect source)
            {
                this.left = source.left;
                this.top = source.top;
                this.right = source.right;
                this.bottom = source.bottom;
            }

            public int width()
            {
                return right - left;
            }

            public int height()
            {
                return bottom - top;
            }

            public int square()
            {
                return (right - left) * (bottom - top);
            }
        }

        protected int minRoomSize = 7; //original=7
        protected int maxRoomSize = 13; //original=9 -> 13

        protected int splitMargin = 2;

        Random rand;

        List<Rect> rooms;

        public regularlevel()
        {
            rand = new Random();
            rooms = new List<Rect>();

            split(new Rect(0 + Level.EDGEBUFFER, 0 + Level.EDGEBUFFER, WIDTH - 1 - Level.EDGEBUFFER, HEIGHT - 1 - Level.EDGEBUFFER));

            for (int n = 0; n < rooms.Count; n++)
            {
                fillroom(rooms[n]);
            }
        }

        int charindex = 65;

        public void fillroom(Rect rect)
        {
            int pos;
            char c = (char)charindex;

            for (int y = rect.top; y <= rect.bottom; y++)
            {
                for (int x = rect.left; x <= rect.right; x++)
                {
                    pos = x + y * WIDTH;

                    grid[pos] = c;
                }
            }

            charindex++;
            if (charindex == 91)
            {
                charindex = 97;
            }
        }

        protected void split(Rect rect)
        {
            int w = rect.width();
            int h = rect.height();

            if (w > minRoomSize * 2 && h < minRoomSize)
            {
                int vw = rand.Next(rect.left + splitMargin, rect.right - splitMargin);
                split(new Rect(rect.left, rect.top, vw, rect.bottom));
                split(new Rect(vw, rect.top, rect.right, rect.bottom));
            }
            else if (h > minRoomSize * 2 && w < minRoomSize)
            {
                int vh = rand.Next(rect.top + splitMargin, rect.bottom - splitMargin);
                split(new Rect(rect.left, rect.top, rect.right, vh));
                split(new Rect(rect.left, vh, rect.right, rect.bottom));
            }
            else if ((rand.NextDouble() <= (minRoomSize * minRoomSize / rect.square()) && w <= maxRoomSize && h <= maxRoomSize) || w < minRoomSize || h < minRoomSize)
            {
                rooms.Add(new Rect(rect));
            }
            else
            {
                if (rand.NextDouble() < (float)(w - 2) / (w + h - 4))
                {
                    int vw = rand.Next(rect.left + splitMargin, rect.right - splitMargin);
                    split(new Rect(rect.left, rect.top, vw, rect.bottom));
                    split(new Rect(vw, rect.top, rect.right, rect.bottom));
                }
                else
                {
                    int vh = rand.Next(rect.top + splitMargin, rect.bottom - splitMargin);
                    split(new Rect(rect.left, rect.top, rect.right, vh));
                    split(new Rect(rect.left, vh, rect.right, rect.bottom));
                }
            }
        }
    }
}
