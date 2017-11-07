/*

Defines the chess board.

*/

#pragma once
#ifndef _board_h
#define _board_h

#include <algorithm>
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

		// piece status query (used in count_if)
		static bool isPieceAlive(Piece* piece) { return piece -> isAlive(); }
		static bool isPieceInitialized(Piece* piece) { return piece -> isInitialized(); }

		// useful statistics
		size_t numPieces() const { return _pieces.size(); }
		size_t numPiecesAlive() const { return std::count_if(_pieces.begin(), _pieces.end(), isPieceAlive); }
		size_t numPiecesAt(int x, int y) const {
			if(!_posInBoard(x, y)) return 0;
			auto occu = &_occupancy[_getX(x)][_getY(y)];
			return occu -> size();
		}
		size_t numPieceAliveAt(int x, int y) const {
			if(!_posInBoard(x, y)) return 0;
			auto occu = &_occupancy[_getX(x)][_getY(y)];
			return std::count_if(occu->begin(), occu->end(), isPieceAlive);
		}

		// TODO
		virtual bool canPlace(int x, int y, int type, int owner, int status) { return false; }
		virtual bool placePiece(int x, int y, int type, int owner, int status) { return false; }

		virtual bool canMove(Piece* piece, int x, int y) { return false; }
		virtual bool movePiece(Piece* piece, int x, int y) { return false; }

	protected:
		int _x0;
		int _y0;
		int _getX(int x)const { return x + _x0; }
		int _getY(int y)const { return y + _y0; }
		bool _posInBoard(int x, int y) const {
			return _getX(x) >= 0 && _getX(x) < w && _getY(y) >= 0 && _getY(y) < h;
		}

		// _occupancy stores the ptrs to pieces at each position
		std::vector<std::vector<std::vector<Piece*>>> _occupancy;
		std::vector<Piece*> _pieces;
	};




}

#endif

