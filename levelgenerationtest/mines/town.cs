using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace levelgenerationtest
{
    public class town : regularlevel
    {
        protected override void initialsplit()
        {
            int _minroomsize = minRoomSize;
            minRoomSize = minRoomSize / 2 + 1;

            int bigroomheight = HEIGHT - EDGEBUFFER * 2 - 4;

            Rect bigroom = new Rect(
                (WIDTH - EDGEBUFFER * 2) * 3 / 10 + EDGEBUFFER,
                EDGEBUFFER + 1,
                (WIDTH - EDGEBUFFER * 2) * 7 / 10 + EDGEBUFFER,
                EDGEBUFFER + bigroomheight + 2
            );

            Rect topstores = new Rect(
                bigroom.left + 2,
                bigroom.top,
                bigroom.right - 2,
                bigroom.top + bigroom.height() * 3 / 14
            );
            split(topstores);

            Rect bottomstores = new Rect(
                bigroom.left + 2,
                bigroom.bottom - bigroom.height() * 3 / 14,
                bigroom.right - 2,
                bigroom.bottom
            );
            split(bottomstores);

            Rect midleftstores = new Rect(
                bigroom.left + 2,
                topstores.bottom + 2,
                bigroom.left + bigroom.width() / 3,
                bottomstores.top - 2
            );
            split(midleftstores);

            Rect midrightstores = new Rect(
                bigroom.right - bigroom.width() / 3,
                topstores.bottom + 2,
                bigroom.right - 2,
                bottomstores.top - 2
            );
            split(midrightstores);

            minRoomSize = _minroomsize;

            split(new Rect(EDGEBUFFER, EDGEBUFFER, bigroom.left, HEIGHT - 1 - EDGEBUFFER));
            rooms.Add(bigroom);
            split(new Rect(bigroom.right, EDGEBUFFER, WIDTH - 1 - EDGEBUFFER, HEIGHT - 1 - EDGEBUFFER));
        }
    }
}
