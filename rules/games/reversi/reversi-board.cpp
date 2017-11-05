#include "reversi-board.h"

using namespace game;

bool ReversiBoard::placePiece(int x, int y, int type, int owner, int status){
    // Check validity
    if(!_posInBoard(x, y)) return false;
    if(!(owner == 0 || owner == 1)) return false;

    // Check occupancy
    if(_occupancy[_getX(x)][_getY(y)] >= 1) return false;

    // Place piece
    _pieces.push_back(new Piece(numPieces(), x, y, type, owner, status));

    // Find consequences
    
}