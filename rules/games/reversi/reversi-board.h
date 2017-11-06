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
        bool canPlace(int x, int y, int type, int owner, int status);
        bool placePiece(int x, int y, int type, int owner, int status);
        
        bool isValidAndConsequence();
    };
}

#endif