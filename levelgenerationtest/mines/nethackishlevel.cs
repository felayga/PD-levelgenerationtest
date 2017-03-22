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
    public class nethackishlevel : Level
    {
        protected int minRoomSize = 1;
        protected int maxRoomSize = 5;
        protected int minRoomGap = 2;
        protected int edgebuffer = 3;

        Random rand;

        protected List<Rect> rooms;

        public nethackishlevel()
        {
            rand = new Random();
            rooms = new List<Rect>();

            initialsplit();

            for (int n = 0; n < rooms.Count; n++)
            {
                fillroom(rooms[n]);
            }
        }

        private double distancebetween(Rect first, Rect second)
        {
            double x1 = (double)(first.left + first.right) / 2.0;
            double y1 = (double)(first.top + first.bottom) / 2.0;

            double x2 = (double)(second.left + second.right) / 2.0;
            double y2 = (double)(second.top + second.bottom) / 2.0;

            x1 = x2 - x1;
            y1 = y2 - y1;
            return Math.Sqrt(x1 * x1 + y1 * y1);
        }

        protected virtual void initialsplit()
        {
            Rect area = new Rect(0 + edgebuffer, 0 + edgebuffer, WIDTH - 1 - edgebuffer, HEIGHT - 1 - edgebuffer);

            for (int n = 0; n < 8; n++)
            {
                int tries = 24;
                while (!buildroom(area) && tries > 0)
                {
                    tries--;
                }

                if (tries <= 0)
                {
                    break;
                }
            }

            maxRoomSize /= 2;

            for (int n = 0; n < 4; n++)
            {
                int tries = 24;
                while (!buildroom(area) && tries > 0)
                {
                    tries--;
                }

                if (tries <= 0)
                {
                    break;
                }
            }


            for (int n = 0; n < rooms.Count; n++)
            {
                Rect room = rooms[n];

                /*
                List<KeyValuePair<double, Rect>> roomdistances = new List<KeyValuePair<double, Rect>>();

                for (int subn = 0; subn < rooms.Count; subn++)
                {
                    if (subn == n)
                    {
                        continue;
                    }
                    roomdistances.Add(new KeyValuePair<double, Rect>(distancebetween(room, rooms[subn]), rooms[subn]));
                }

                roomdistances.Sort((a, b) => { return a.Key.CompareTo(b.Key); });
                */

                int links = rand.Next(1, 4);

                while (links > 0 /*&& roomdistances.Count > 0*/)
                {
                    int x;
                    int y;

                    if (rand.Next(2) == 0)
                    {
                        x = rand.Next(room.left + 1, room.right);

                        if (rand.Next(2) == 0)
                        {
                            y = room.top - 1;
                            grid[y * WIDTH + x] = '#';
                            y--;
                        }
                        else
                        {
                            y = room.bottom + 1;
                            grid[y * WIDTH + x] = '#';
                            y++;
                        }
                    }
                    else
                    {
                        y = rand.Next(room.top + 1, room.bottom);

                        if (rand.Next(2) == 0)
                        {
                            x = room.left - 1;
                            grid[y * WIDTH + x] = '#';
                            x--;
                        }
                        else
                        {
                            x = room.right + 1;
                            grid[y * WIDTH + x] = '#';
                            x++;
                        }
                    }

                    /*
                    Rect dest = roomdistances[0].Value;
                    roomdistances.RemoveAt(0);
                    */
                    int index = n;
                    while (index==n) {
                        index = rand.Next(rooms.Count);
                    }
                    Rect dest = rooms[index];

                    int destx;
                    int desty;

                    if (rand.Next(2) == 0)
                    {
                        destx = rand.Next(dest.left + 1, dest.right);

                        if (rand.Next(2) == 0)
                        {
                            desty = dest.top - 1;
                            grid[desty * WIDTH + destx] = '#';
                            desty--;
                        }
                        else
                        {
                            desty = dest.bottom + 1;
                            grid[desty * WIDTH + destx] = '#';
                            desty++;
                        }
                    }
                    else
                    {
                        desty = rand.Next(dest.top + 1, dest.bottom);

                        if (rand.Next(2) == 0)
                        {
                            destx = dest.left - 1;
                            grid[desty * WIDTH + destx] = '#';
                            destx--;
                        }
                        else
                        {
                            destx = dest.right + 1;
                            grid[desty * WIDTH + destx] = '#';
                            destx++;
                        }
                    }

                    while (grid[y * WIDTH + x] == '█')
                    {
                        grid[y * WIDTH + x] = ' ';

                        bool moved = false;

                        while (!moved && (x != destx || y != desty))
                        {
                            if (x != destx && rand.Next(2) == 0)
                            {
                                if (x < destx)
                                {
                                    x++;
                                    moved = true;
                                }
                                else if (x > destx)
                                {
                                    x--;
                                    moved = true;
                                }
                            }
                            else
                            {
                                if (y < desty)
                                {
                                    y++;
                                    moved = true;
                                }
                                else if (y > desty)
                                {
                                    y--;
                                    moved = true;
                                }
                            }
                        }
                    }

                    grid[y * WIDTH + x] = ' ';

                    links--;
                }
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

                    /*
                    if (grid[pos] >= 33 && grid[pos] <= 122)
                    {
                        grid[pos] = '!';
                    }
                    else
                    {*/
                        grid[pos] = c;
                    /*}*/
                }
            }

            charindex++;
            if (charindex == 91)
            {
                charindex = 97;
            }

            if (charindex == 123)
            {
                charindex = 48;
            }

            if (charindex == 58)
            {
                charindex = 65;
            }
        }

        protected bool buildroom(Rect rect)
        {
            int width = 1 + rand.Next(minRoomSize, maxRoomSize) + rand.Next(minRoomSize, maxRoomSize);
            int height = 1 + rand.Next(minRoomSize, maxRoomSize) + rand.Next(minRoomSize, maxRoomSize);

            int x = rand.Next(rect.left, rect.right - width);
            int y = rand.Next(rect.top, rect.bottom - height);

            Rect test = new Rect(x, y, x + width, y + height);

            for (int n = 0; n < rooms.Count; n++)
            {
                if (test.top > rooms[n].bottom + minRoomGap)
                {
                    //good
                }
                else if (test.bottom < rooms[n].top - minRoomGap)
                {
                    //good
                }
                else if (test.left > rooms[n].right + minRoomGap)
                {
                    //good
                }
                else if (test.right < rooms[n].left - minRoomGap)
                {
                    //good
                }
                else
                {
                    return false;
                }
            }

            rooms.Add(test);
            return true;
        }
    }
}
