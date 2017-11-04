/*

Defines the chess board.

*/

#pragma once
#ifndef _board_h
#define _board_h

namespace game{
	class board{
	public:
		board(const size_t width, const size_t height): w(width), h(height){}
		const size_t w;
		const size_t h;
	};




}

#endif

