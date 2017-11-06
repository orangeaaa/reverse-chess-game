/*

Defines the chess board.

*/

#pragma once
#ifndef _board_h
#define _board_h

#include <vector>

#include "piece.h"

namespace game{

	class Board{
	public:
		Board(const size_t width, const size_t height):
			w(width), h(height), _x0(0), _y0(0),
			_occupancy(width, std::vector<std::vector<Piece*>>(height)) {}
		~Board();
		const size_t w;
		const size_t h;

		size_t numPieces()const { return _pieces.size(); }

		virtual bool canPlace(int x, int y, int type, int owner, int status) { return false; }
		virtual bool placePiece(int x, int y, int type, int owner, int status) { return false; }

		virtual bool canMove(Piece* piece, int x, int y) { return false; }
		virtual bool movePiece(Piece* piece, int x, int y) { return false; }

	protected:
		int _x0;
		int _y0;
		int _getX(int x)const { return x + _x0; }
		int _getY(int y)const { return y + _y0; }
		int _posInBoard(int x, int y){
			return _getX(x) >= 0 && _getX(x) < w && _getY(y) >= 0 && _getY(y) < h;
		}

		std::vector<std::vector<std::vector<Piece*>>> _occupancy;
		std::vector<Piece*> _pieces;
	};




}

#endif

