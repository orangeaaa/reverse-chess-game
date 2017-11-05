#include "board.h"

using namespace game;

Board::~Board(){
	for(Piece* piece: _pieces){
		delete piece;
	}
}
