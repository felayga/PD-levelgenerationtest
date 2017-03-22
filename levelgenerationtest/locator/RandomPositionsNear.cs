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
    public class RandomPositionsNear : Level
    {
        protected int minRoomSize = 1;
        protected int maxRoomSize = 5;
        protected int minRoomGap = 2;
        protected int edgebuffer = 3;

        Random rand;

        protected List<Rect> rooms;

        public RandomPositionsNear()
        {
            rand = new Random();
            rooms = new List<Rect>();

            initialsplit();
        }

        protected virtual void initialsplit()
        {
            int x = WIDTH / 2;
            int y = HEIGHT / 2;
            int pos = x + y * WIDTH;

            List<int> positions = randomPositionsNear(pos, 9, (p) => {
                return grid[p]=='█';
            });

            for (int subx = 0; subx < WIDTH; subx++)
            {
                grid[subx + y * WIDTH] = 'x';
            }

            for (int suby = 0; suby < HEIGHT; suby++)
            {
                grid[x + suby * WIDTH] = 'y';
            }

            if (positions != null)
            {
                for (int n = 0; n < positions.Count; n++)
                {
                    grid[positions[n]] = (char)('0' + n);
                }
            }
        }

        public List<int> randomPositionsNear(int pos, int quantity, Func<int, bool> validator)
        {
            List<int> retval = new List<int>();
            List<int> positionOffsets = new List<int>();

            //GLog.d("fallback spawn");
            //fallback
            //todo: this rarely returns a null ArrayList for some reason, fixme


            positionOffsets.Clear();

            foreach (int offset in Level.NEIGHBOURS4)
            {
                positionOffsets.Add(offset);
            }

            List<int> oldPositionsOffsets = positionOffsets;
            var result = oldPositionsOffsets.OrderBy(item => rand.Next());
            positionOffsets = new List<int>();
            foreach (int item in result)
            {
                positionOffsets.Add(item);
            }

            positionOffsets.Insert(0, 0);

            Dictionary<int, object> tested = new Dictionary<int, object>();
            object alreadyTestedFlag = new object();

            int repeatOffset = -1;
            int iterationLimit = 16;

            while (iterationLimit > 0)
            {
                if (repeatOffset >= 0)
                {
                    if (tested.Count <= 0)
                    {
                        return null;
                    }

                    int failsafe = tested.Count * tested.Count;

                    Object test = alreadyTestedFlag;
                    while (test == alreadyTestedFlag)
                    {
                        int index = rand.Next(tested.Count);
                        foreach (var pair in tested)
                        {
                            index--;
                            if (index < 0)
                            {
                                pos = pair.Key;
                                test = pair.Value;
                                break;
                            }
                        }

                        failsafe--;
                        if (failsafe <= 0)
                        {
                            return null;
                        }
                    }

                    tested[pos] = alreadyTestedFlag;
                }

                /*
                List<int> oldPositionsOffsets = positionOffsets;
                var result = oldPositionsOffsets.OrderBy(item => rand.Next());
                positionOffsets = new List<int>();
                foreach (int item in result)
                {
                    positionOffsets.Add(item);
                }
                */

                foreach (int ofs in positionOffsets)
                {
                    int cell = pos + ofs;

                    bool passable = validator(cell);

                    if (passable)
                    {
                        tested[cell] = null;

                        if (tested.Count >= quantity)
                        {
                            iterationLimit = 0;
                            break;
                        }
                    }
                }

                repeatOffset++;
                iterationLimit--;
            }

            retval.Clear();
            foreach (var pair in tested)
            {
                retval.Add(pair.Key);
            }
            return retval;
            /*
            var sresult = retval.OrderBy(item => rand.Next());
            List<int> tretval = new List<int>();
            foreach (int item in sresult)
            {
                tretval.Add(item);
            }
            return tretval;
            */
        }
    }
}
