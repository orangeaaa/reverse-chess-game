#include "reversi-board.h"

using namespace game;

bool ReversiBoard::canPlace(int x, int y, int type, int owner, int status) {
    // Check validity
    if(!_posInBoard(x, y)) return false;
    if(!(owner == 0 || owner == 1)) return false;
    size_t nx = _getX(x), ny = _getY(y);

    // Check occupancy
    if(_occupancy[nx][ny].size() >= 1) return false;
    
    // TODO
}

bool ReversiBoard::placePiece(int x, int y, int type, int owner, int status){
    if(!canPlace(x, y, type, owner, status)) return false;
    size_t nx = _getX(x), ny = _getY(y);

    // Check occupancy
    if(_occupancy[nx][ny].size() >= 1) return false;
    
    // Place piece
    Piece* newPiece = new Piece(numPieces(), nx, ny, type, owner, status);
    _pieces.push_back(newPiece);
    _occupancy[nx][ny].push_back(newPiece);

    // Find consequences
    // TODO
}


// TODO
bool ReversiBoard::validConse(int nx, int ny, int owner, bool realMove = false)
{

    var pro_move = new List<Tuple<int, int, int, int>>();

    bool isValid = false;
    int validCount;

    validCount = 0;
    for (int i = x + 1; i < N; i++) // R
    {
        if (status[i, y] == pl) { validCount = i - x - 1; break; }
        else if (status[i, y] == -1) { validCount = 0; break; }
    }
    if (validCount > 0)
    {
        isValid = true;
        for (int i = x; i < N; i++)
        {
            if (status[i, y] == pl) break;
            else
            {
                pro_move.Add(new Tuple<int, int, int, int>(i, y, status[i, y], pl));
            }
        }
    }

    validCount = 0;
    for (int i = x + 1, j = y + 1; i < N && j < N; i++, j++) // BR
    {
        if (status[i, j] == pl) { validCount = i - x - 1; break; }
        else if (status[i, j] == -1) { validCount = 0; break; }
    }
    if (validCount > 0)
    {
        isValid = true;
        for (int i = x + 1, j = y + 1; i < N && j < N; i++, j++)
        {
            if (status[i, j] == pl) break;
            else
            {
                pro_move.Add(new Tuple<int, int, int, int>(i, j, status[i, j], pl));
            }
        }
    }

    validCount = 0;
    for (int j = y + 1; j < N; j++) // B
    {
        if (status[x, j] == pl) { validCount = j - y - 1; break; }
        else if (status[x, j] == -1) { validCount = 0; break; }
    }
    if (validCount > 0)
    {
        isValid = true;
        for (int j = y + 1; j < N; j++)
        {
            if (status[x, j] == pl) break;
            else
            {
                pro_move.Add(new Tuple<int, int, int, int>(x, j, status[x, j], pl));
            }
        }
    }

    validCount = 0;
    for (int i = x - 1, j = y + 1; i >= 0 && j < N; i--, j++) // BL
    {
        if (status[i, j] == pl) { validCount = x - i - 1; break; }
        else if (status[i, j] == -1) { validCount = 0; break; }
    }
    if (validCount > 0)
    {
        isValid = true;
        for (int i = x - 1, j = y + 1; i >= 0 && j < N; i--, j++)
        {
            if (status[i, j] == pl) break;
            else
            {
                pro_move.Add(new Tuple<int, int, int, int>(i, j, status[i, j], pl));
            }
        }
    }

    validCount = 0;
    for (int i = x - 1; i >= 0; i--) // L
    {
        if (status[i, y] == pl) { validCount = x - i - 1; break; }
        else if (status[i, y] == -1) { validCount = 0; break; }
    }
    if (validCount > 0)
    {
        isValid = true;
        for (int i = x - 1; i >= 0; i--)
        {
            if (status[i, y] == pl) break;
            else
            {
                pro_move.Add(new Tuple<int, int, int, int>(i, y, status[i, y], pl));
            }
        }
    }

    validCount = 0;
    for (int i = x - 1, j = y - 1; i >= 0 && j >= 0; i--, j--) // UL
    {
        if (status[i, j] == pl) { validCount = x - i - 1; break; }
        else if (status[i, j] == -1) { validCount = 0; break; }
    }
    if (validCount > 0) {
        isValid = true;
        for (int i = x - 1, j = y - 1; i >= 0 && j >= 0; i--, j--) {
            if (status[i, j] == pl) break;
            else {
                pro_move.Add(new Tuple<int, int, int, int>(i, j, status[i, j], pl));
            }
        }
    }

    validCount = 0;
    for (int j = y - 1; j >= 0; j--) // U
    {
        if (status[x, j] == pl) { validCount = y - j - 1; break; }
        else if (status[x, j] == -1) { validCount = 0; break; }
    }
    if (validCount > 0) {
        isValid = true;
        for (int j = y - 1; j >= 0; j--) {
            if (status[x, j] == pl) break;
            else {
                pro_move.Add(new Tuple<int, int, int, int>(x, j, status[x, j], pl));
            }
        }
    }

    validCount = 0;
    for (int i = x + 1, j = y - 1; i < N && j >= 0; i++, j--) // UR
    {
        if (status[i, j] == pl) { validCount = i - x - 1; break; }
        else if (status[i, j] == -1) { validCount = 0; break; }
    }
    if (validCount > 0) {
        isValid = true;
        for (int i = x + 1, j = y - 1; i < N && j >= 0; i++, j--) {
            if (status[i, j] == pl) break;
            else {
                pro_move.Add(new Tuple<int, int, int, int>(i, j, status[i, j], pl));
            }
        }
    }

    if (isValid)
    {
        pro_move.Add(new Tuple<int, int, int, int>(x, y, status[x, y], pl));
    }

    if (realMove) {
        moves.Add(pro_move);
        exe_move(pro_move);
    }

    return isValid;
}
