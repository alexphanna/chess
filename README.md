# Chess

<img src="screenshot.png" width="300" />

## Prerequisites
- .NET 5.0
## Features
- [x] Abstract piece object class w/ abstract IsLegal method 
- [x] Inherited children w/ unique IsLegal methods for the piece types (King, Queen, Rook, Bishop, Knight, Pawn)
- [x] Board object that inherits List<Piece> w/ methods such as: find and exists
- [x] Arrow keys based movement and selection system 
- [x] Automatically change console font to MS Gothic and font size to 72
- [x] Check and checkmate
- [X] Stalemate and insufficient material
- [X] Special moves (en passant, promotion, castling)
- [ ] Threefold and fivefold repetition 
- [ ] Fifty and seventy-five move rule
- [ ] Mutual draw agreement and resignation

## Controls
| Key          | Function           |
| ------------ | ------------------ |
| Up Arrow     | Move cursor up     |
| Down Arrow   | Move cursor down   |
| Left Arrow   | Move cursor left   |
| Right Arrow  | Move cursor right  |
| Enter        | Select             |
| Escape       | Unselect           |
## Contributions
Made alongside [Matthew Romano](https://github.com/MatthewDRomano)
