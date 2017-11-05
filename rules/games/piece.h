/*

Defines the chess piece.

*/

#pragma once
#ifndef _piece_h
#define _piece_h

#include <string>

namespace game{

	class Piece{
	public:
		Piece(const size_t id): _id(id){}
		Piece(const size_t id, int x, int y, int type, int owner, int status):
			_id(id), _x(x), _y(y), _type(type), _owner(owner), _status(status) {}
		
		void init(int x, int y, int type, int owner, int status){
			_x = x; _y = y;
			_type = type;
			_owner = owner;
			_status = status;
		}
		void setPos(int x, int y) { _x = x; _y = y; }
		void setOwner(int owner) { _owner = owner; }
		void setStatus(int status) { _status = status; }

	private:
		const size_t _id;
		int _type;
		int _owner;
		int _status;
		
		int _x;
		int _y;

	};




}

#endif

