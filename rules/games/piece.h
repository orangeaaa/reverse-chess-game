/*

Defines the chess piece.

*/

#pragma once
#ifndef _piece_h
#define _piece_h

#include <string>

namespace game{
	class piece{
	public:
		piece(const std::string& name): _name(name){}
		
		std::string name()const{return _name;}
		
	private:
		const std::string _name;
		int _status;
		
		size_t _x;
		size_t _y;
	};




}

#endif

