/*

Defines the chess piece.

*/

#pragma once

namespace game{

	// Forward declaration
	class Board;

	class Piece{

	public:
		Piece(const size_t id): _id(id), _status(0) {}
		Piece(const size_t id, size_t x, size_t y, int type, int owner, int status):
			_id(id), _x(x), _y(y), _type(type), _owner(owner), _status(status) {}
		
		// status query
		bool isAlive() const { return _status & PieceStatusAlive; }
		bool isInitialized() const { return _status & PieceStatusInitialized; }

	protected:
		// basic operations
		void setPos(size_t x, size_t y) { _x = x; _y = y; }
		void setOwner(int owner) { _owner = owner; }
		void setType(int type) { _type = type; }
		void setStatus(int status) { _status = status; }

		// possible game operations
		virtual void place(size_t x, size_t y) { setPos(x, y); _status |= (PieceStatusInitialized | PieceStatusAlive); }
		virtual void moveTo(size_t x, size_t y) { setPos(x, y); }
		virtual void destroy() { _status &= ~PieceStatusAlive; }

		// restrictions
		virtual bool canPlace(const Board& board, size_t x, size_t y) const { return false; }
		virtual bool canMoveTo(const Board& board, size_t x, size_t y) const { return false; }

		// Piece attributes
		const size_t _id;
		int _type;
		int _owner;
		int _status;

		// Piece status
		static const int PieceStatusInitialized = 1;
		static const int PieceStatusAlive       = 2;
		
		// Actual Stored Coordinates might be different from the displayed ones.
		size_t _x;
		size_t _y;

	};




}
