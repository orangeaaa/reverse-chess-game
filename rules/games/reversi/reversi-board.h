/*

Define the board for Reversi

*/

#pragma once
#ifndef _reversi_board_h
#define _reversi_board_h

#include "../board.h"

namespace game{
    class ReversiBoard: public Board{
    public:
        bool placePiece(int x, int y, int type, int owner, int status){
            // Check validity
            if(!_posInBoard(x, y)) return false;
            if(type != 0) return false;
            if(!(owner == 0 || owner == 1)) return false;
            if(status != 0) return false;

            // Check occupancy
            if(_occupancy[_getX(x)][_getY(y)] >= 1) return false;

            //
        }
    };
}

#endif