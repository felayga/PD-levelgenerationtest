/*
 * Pixel Dungeon
 * Copyright (C) 2012-2015  Oleg Dolya
 *
 * Shattered Pixel Dungeon
 * Copyright (C) 2014-2015 Evan Debenham
 *
 * unPixel Dungeon
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
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace levelgenerationtest
{
    public class TunnelPainter : Level
    {
        Random random = new Random();

        public TunnelPainter()
        {
            int x1 = -1;
            int x2 = -1;
            int y1 = -1;
            int y2 = -1;

            while (x2 - x1 < 2 || y2 - y1 < 2)
            {
                x1 = random.Next(WIDTH);
                x2 = random.Next(WIDTH);
                y1 = random.Next(HEIGHT);
                y2 = random.Next(HEIGHT);

                if (x1 > x2)
                {
                    int swap = x1;
                    x1 = x2;
                    x2 = swap;
                }
                if (y1 > y2)
                {
                    int swap = y1;
                    y1 = y2;
                    y2 = swap;
                }
            }

            for (int y = y1 + 1; y < y2; y++)
            {
                for (int x = x1 + 1; x < x2; x++)
                {
                    _set(this, x, y, '▓');
                }
            }

            paint(this, new Rect(x1, y1, x2, y2), random);
        }

        public static void _set(Level level, int x, int y, char value)
        {
            level.grid[x + y * WIDTH] = value;
            //level.setRepairing(x + y * Level.WIDTH, value, false, false);
        }

        public static void paint(Level level, Rect room, Random random) {
            const char floor = '*';
            const char doorc = '#';

            List<Point> doors = new List<Point>();

            while (doors.Count < 2)
            {
                doors.Clear();
                int tries = 125;
                for (int n = random.Next(1, 5); n >= 0; n--)
                {
                    Point door;
                    if (random.Next(2) == 0)
                    {
                        if (random.Next(2) == 0)
                        {
                            door = new Point(room.left, random.Next(room.top + 1, room.bottom));
                        }
                        else
                        {
                            door = new Point(room.right, random.Next(room.top + 1, room.bottom));
                        }
                    }
                    else
                    {
                        if (random.Next(2) == 0)
                        {
                            door = new Point(random.Next(room.left + 1, room.right), room.top);
                        }
                        else
                        {
                            door = new Point(random.Next(room.left + 1, room.right), room.bottom);
                        }
                    }

                    bool found = false;
                    for (int subn = 0; subn < doors.Count; subn++)
                    {
                        if (Math.Abs(doors[subn].x - door.x) < 2 || Math.Abs(doors[subn].y - door.y) < 2)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found)
                    {
                        n++;
                        tries--;
                        if (tries < 0)
                        {
                            break;
                        }
                    }
                    else
                    {
                        doors.Add(door);
                    }
                }
            }

            //connectStraight(level, room, random, doors, floor);
            connectPoint(level, room, random, doors, floor);

            foreach (Point door in doors)
            {
                _set(level, door.x, door.y, doorc);
            }

            /*
            for (Room.Door door : room.connected.values()) {
                door.set(Room.Door.Type.TUNNEL);
            }
            */
        }

        private static void connectPoint(Level level, Rect room, Random random, List<Point> doors, char floor)
        {
            int w = (room.right - room.left) * 2 / 5;
            int h = (room.bottom - room.top) * 2 / 5;

            Point c = new Point(
                room.left + random.Next(w) + w*3/10 + 1,
                room.top + random.Next(h) + h*3/10 + 1
            );

            _set(level, c.x, c.y, 'X');

            for (int n = 0; n < doors.Count; n++)
            {
                int x = doors[n].x;
                int y = doors[n].y;

                if (x == room.left)
                {
                    x++;
                }
                if (x == room.right)
                {
                    x--;
                }
                if (y == room.top)
                {
                    y++;
                }
                if (y == room.bottom)
                {
                    y--;
                }

                while (c.x != x || c.y != y)
                {
                    _set(level, x, y, floor);

                    bool moved = false;

                    while (!moved && (x != c.x || y != c.y))
                    {
                        if (x != c.x && random.Next(2) == 0)
                        {
                            if (x < c.x)
                            {
                                x++;
                                moved = true;
                            }
                            else if (x > c.x)
                            {
                                x--;
                                moved = true;
                            }
                        }
                        else
                        {
                            if (y < c.y)
                            {
                                y++;
                                moved = true;
                            }
                            else if (y > c.y)
                            {
                                y--;
                                moved = true;
                            }
                        }
                    }
                }
            }
        }

        private static void connectStraight(Level level, Rect room, Random random, List<Point> doors, char floor)
        {
            Point c = room.center();


            if (room.width() > room.height() || (room.width() == room.height() && random.Next(2) == 0))
            {
                int from = room.right - 1;
                int to = room.left + 1;

                foreach (Point door in doors)
                {
                    int step = door.y < c.y ? +1 : -1;

                    if (door.x == room.left)
                    {
                        from = room.left + 1;
                        for (int i = door.y; i != c.y; i += step)
                        {
                            _set(level, from, i, floor);
                        }
                    }
                    else if (door.x == room.right)
                    {
                        to = room.right - 1;
                        for (int i = door.y; i != c.y; i += step)
                        {
                            _set(level, to, i, floor);
                        }
                    }
                    else
                    {
                        if (door.x < from)
                        {
                            from = door.x;
                        }
                        if (door.x > to)
                        {
                            to = door.x;
                        }

                        for (int i = door.y + step; i != c.y; i += step)
                        {
                            _set(level, door.x, i, floor);
                        }
                    }
                }

                for (int i = from; i <= to; i++)
                {
                    _set(level, i, c.y, floor);
                }
            }
            else
            {
                int from = room.bottom - 1;
                int to = room.top + 1;

                foreach (Point door in doors)
                {
                    int step = door.x < c.x ? +1 : -1;

                    if (door.y == room.top)
                    {
                        from = room.top + 1;
                        for (int i = door.x; i != c.x; i += step)
                        {
                            _set(level, i, from, floor);
                        }
                    }
                    else if (door.y == room.bottom)
                    {
                        to = room.bottom - 1;
                        for (int i = door.x; i != c.x; i += step)
                        {
                            _set(level, i, to, floor);
                        }
                    }
                    else
                    {
                        if (door.y < from)
                        {
                            from = door.y;
                        }
                        if (door.y > to)
                        {
                            to = door.y;
                        }

                        for (int i = door.x + step; i != c.x; i += step)
                        {
                            _set(level, i, door.y, floor);
                        }
                    }
                }

                for (int i = from; i <= to; i++)
                {
                    _set(level, c.x, i, floor);
                }
            }

        }
    }
}
