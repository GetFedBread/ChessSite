
const letter2piece = new Map([
    ["p", "♟"],["P", "♙"],
    ["n", "♞"],["N", "♘"],
    ["b", "♝"],["B", "♗"],
    ["r", "♜"],["R", "♖"],
    ["q", "♛"],["Q", "♕"],
    ["k", "♚"],["K", "♔"],
    ["", ""]
]);
const piece2letter = new Map([
    ["♟", "p"],["♙", "P"],
    ["♞", "n"],["♘", "N"],
    ["♝", "b"],["♗", "B"],
    ["♜", "r"],["♖", "R"],
    ["♛", "q"],["♕", "Q"],
    ["♚", "k"],["♔", "K"],
    ["", ""]
]);
const white = new Set(["P", "N", "B", "R", "Q", "K"]);

let turn = "w";
let starting_position = null;
let castling_rights = new Set();
let moves = [];
// This forces all chessboards to do the standard setup when loaded.
// This might not always be desired. 
load_postition(starting_position);

function load_postition(position) {
    if(position == null) {
        position = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    }
    clear_board();
    let data = position.split(" ");
    let row = 8;
    let column = 0;
    for(const c of data[0]) {
        let id = String.fromCharCode(97 + column)+row;
        if(c == '/') continue;
        if(c >= '1' && c <= '8') {
            let skip = parseInt(c);
            column += skip;
            if(column >= 8) {
                row -= 1;
                column -= 8;
            }
            continue;
        }
        //console.log("Piece "+c+" on "+id);
        document.getElementById(id).innerHTML = letter2piece.get(c);
        column += 1;
        if(column >= 8) {
            row -= 1;
            column -= 8;
        }
    }
    turn = data[1];
    for(const c of data[2]) {
        if(c != '-') {
            castling_rights.add(c);
        }
    }
    starting_position = position;
    moves = [];
    document.getElementById("MoveDisplay").innerHTML = "";
    document.getElementById("TurnDisplay").innerHTML = turn;
    document.getElementById("BoardStateDisplay").innerHTML = position;
}

function clear_board() {
    for(let row = 8; row >= 1; row--) {
        for(let column = 0; column < 8; column++) {
            let id = String.fromCharCode(97 + column)+row;
            document.getElementById(id).innerHTML = "";
        }
    }
}

let last_tile = null;
let legal_moves = null;
function tile_clicked(tile) {
    if(last_tile == null) {
        let moves = get_moves(tile);
        if(moves.size > 0) {
            last_tile = tile;
            legal_moves = moves;
        }
    } else if(tile != last_tile && legal_moves.has(tile)) {
        move(last_tile, tile);
        last_tile = null;
        legal_moves = null;
    } else {
        last_tile = null;
        legal_moves = null;
    }
}

/*
    TO-DO:
     - Detect check and mate
     - Castling
*/
function move(from, to) {
    if(from == null || to == null) {
        throw new Error("Null argument to move");
    }
    let piece = document.getElementById(from).innerHTML;
    let piece_letter = piece2letter.get(piece);
    
    if(piece.length == 0 || (turn == "w") == !white.has(piece_letter)) return;

    if(castling_rights.size != 0) {
        if(piece_letter == "K") {
            castling_rights.delete("K");
            castling_rights.delete("Q");
        }else if(piece_letter == "k") {
            castling_rights.delete("k");
            castling_rights.delete("q");
        }
        if(to == "a1" || from == "a1") {
            castling_rights.delete("Q");
        }
        if(to == "a8" || from == "a8") {
            castling_rights.delete("q");
        }
        if(to == "h1" || from == "h1") {
            castling_rights.delete("K");
        }
        if(to == "h8" || from == "h8") {
            castling_rights.delete("k");
        }
    }
    
    document.getElementById(to).innerHTML = piece;
    document.getElementById(from).innerHTML = "";
    moves.push(from+to);

    turn = turn == "w" ? "b" : "w";
    document.getElementById("TurnDisplay").innerHTML = turn;
    document.getElementById("BoardStateDisplay").innerHTML = generate_board_state();
    
    let moves_display = document.getElementById("MoveDisplay");
    moves_display.innerHTML = "";
    moves.forEach(move => {
        moves_display.innerHTML += "\n<p>"+move+"</p>";
    });
}

function get_moves(tile_id) {
    // function to generate moves that go in a straight line
    let repeat_move = (tile, move, is_white) => {
    let column = tile.charCodeAt(0) - 96;
    let row = parseInt(tile.at(1));
    column += move[0];
    row += move[1];

    let tiles = new Set();

    while(column >= 1 && column <= 8 && row >= 1 && row <= 8) {
        let id = String.fromCharCode(96 + column)+row;
        let tile_letter = piece2letter.get(document.getElementById(id).innerHTML);

        if(tile_letter != "")  {
            if(white.has(tile_letter) != is_white) {
                tiles.add(id);
            }
            break;
        }

        tiles.add(id);
        column += move[0];
        row += move[1];
    }

    return tiles;
    }
    let piece_letter = piece2letter.get(document.getElementById(tile_id).innerHTML);
    let tile_column = tile_id.charCodeAt(0) - 96;
    let tile_row = parseInt(tile_id.at(1));
    let is_white = white.has(piece_letter);
    
    let tiles = new Set();

    switch(piece_letter.toLowerCase()) {
        case "p":
            for(let i = 1; i <= 2; i++) {
                let column = tile_column;
                let row = tile_row + i * (is_white ? 1 : -1);
                let id = String.fromCharCode(96 + column)+row;
                if(column >= 1 && column <= 8 && row >= 1 && row <= 8) {
                    let tile_letter = piece2letter.get(document.getElementById(id).innerHTML);
                    if(tile_letter == "")  {
                        tiles.add(id);
                    } else {
                        break;
                    }
                } else {
                    break;
                }
                if(is_white && row > 3 || !is_white && row < 6) {
                    break;
                }
            }
            for(i = -1; i <= 1; i += 2) {
                let column = tile_column + i;
                let row = tile_row + 1;
                let id = String.fromCharCode(96 + column)+row;
                if(column >= 1 && column <= 8 && row >= 1 && row <= 8) {
                    let tile_letter = piece2letter.get(document.getElementById(id).innerHTML);
                    if(tile_letter != "" && white.has(tile_letter) != is_white)  {
                        tiles.add(id);
                    }
                }
            }
            
        break;

        case "n":
            for(let i = 0; i < 2; i++){
                for(let j = 0; j < 2; j++){
                    let column_offset = i * 2 - 1;
                    let row_offset = j * 2 - 1;

                    let column = column_offset * 2 + tile_column;
                    let row = row_offset * 1 + tile_row;
                    if(column >= 1 && column <= 8 && row >= 1 && row <= 8) {
                        let id = String.fromCharCode(96 + column)+row;
                        let tile_letter = piece2letter.get(document.getElementById(id).innerHTML);
                        if(tile_letter == "" || white.has(tile_letter) != is_white)  {
                            tiles.add(id);
                        }
                    }

                    column = column_offset * 1 + tile_column;
                    row = row_offset * 2 + tile_row;
                    if(column >= 1 && column <= 8 && row >= 1 && row <= 8) {
                        let id = String.fromCharCode(96 + column)+row;
                        let tile_letter = piece2letter.get(document.getElementById(id).innerHTML);
                        if(tile_letter == "" || white.has(tile_letter) != is_white)  {
                            tiles.add(id);
                        }
                    }
                }
            }
        break;

        case "b":
            for(let i = 0; i < 4; i++) {
                var add_tiles = repeat_move(tile_id, [
                    i % 2 * 2 - 1,              // column
                    Math.floor(i / 2) * 2 - 1   // row
                ], is_white);
                add_tiles.forEach(tile => {
                    tiles.add(tile);
                });
            }
        break;

        case "r":
            for(let i = 0; i < 4; i++) {
                var add_tiles = repeat_move(tile_id, [
                    Math.floor(i / 2) * (i % 2 * 2 - 1),        // column
                    (Math.floor(i / 2) - 1) * (i % 2 * 2 - 1)   // row
                ], is_white);
                add_tiles.forEach(tile => {
                    tiles.add(tile);
                });
            }
        break;

        case "q":
            for(let i = 0; i < 4; i++) {
                // rook moves
                var add_tiles = repeat_move(tile_id, [
                    Math.floor(i / 2) * (i % 2 * 2 - 1),        // column
                    (Math.floor(i / 2) - 1) * (i % 2 * 2 - 1)   // row
                ], is_white);
                add_tiles.forEach(tile => {
                    tiles.add(tile);
                });

                // bishop moves
                add_tiles = repeat_move(tile_id, [
                    i % 2 * 2 - 1,              // column
                    Math.floor(i / 2) * 2 - 1   // row
                ], is_white);
                add_tiles.forEach(tile => {
                    tiles.add(tile);
                });
            }
        break;
        
        case "k":
            for(let i = -1; i <= 1; i++) {
                for(let j = -1; j <= 1; j++) {
                    if(i == 0 && j == 0) continue;
                    let column = tile_column + i;
                    let row = tile_row + j;
                    let id = String.fromCharCode(96 + column)+row;
                    if(column >= 1 && column <= 8 && row >= 1 && row <= 8) {
                        let tile_letter = piece2letter.get(document.getElementById(id).innerHTML);
                        if(tile_letter == "" || white.has(tile_letter) != is_white)  {
                            tiles.add(id);
                        }
                    }
                }
            }
        break;
    }
    return tiles;
}

function generate_board_state() {
    let state = "";
    for(let row = 8; row >= 1; row--) {
        let empty_count = 0;
        for(let column = 0; column < 8; column++) {
            let id = String.fromCharCode(97 + column)+row;
            let val = document.getElementById(id).innerHTML;
            if(val.length == 0) {
                empty_count++;
                continue;
            }
            if(empty_count > 0) {
                state += empty_count;
                empty_count = 0;
            }
            state += piece2letter.get(val);
        }
        if(empty_count > 0) {
            state += empty_count;
            empty_count = 0;
        }
        if(row > 1) {
            state += "/";
        }
    }

    let castling = "";
    ["K","Q","k","q"].forEach(c => {
        if(castling_rights.has(c)) castling += c;
    });
    castling = castling == "" ? "-" : castling;

    // Still yet to understand the last three values
    state += " "+turn+" "+castling+" - 0 1";
    return state;
}

