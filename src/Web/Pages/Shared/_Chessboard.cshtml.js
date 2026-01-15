
let turn = "w";
// This forces all chessboards to do the standard setup when loaded.
// This might not always be desired. 
load_postition(null);

function load_from_input() {
    let position = document.getElementById("BoardPosition").value;
    console.log(position);
    load_postition(position);
}

function load_postition(position) {
    if(position == null) {
        position = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    }
    clear_board();
    let data = position.split(" ");
    let row = 8;
    let column = 0;
    for(const c of data[0]) {
        var id = String.fromCharCode(97 + column)+row;
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
        document.getElementById(id).innerHTML = c;
        column += 1;
        if(column >= 8) {
            row -= 1;
            column -= 8;
        }
    }
    turn = data[1];
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
function tile_clicked(tile) {
    if(last_tile == null) {
        last_tile = tile;
    } else if(tile != last_tile) {
        move(last_tile, tile);
        last_tile = null;
    }
}

/*
    TO-DO:
     - Allow only valid moves
     - Detect check and mate
     - Only the player whose move it is can move
     - Swap turn after move is made
*/
function move(from, to) {
    if(from == null || to == null) {
        throw new Error("Null argument to move");
    }
    let piece = document.getElementById(from).innerHTML;
    if(piece.length != 0) {
        document.getElementById(to).innerHTML = piece;
        document.getElementById(from).innerHTML = "";
    }
    turn = turn == "w" ? "b" : "w";
    document.getElementById("TurnDisplay").innerHTML = turn;
    document.getElementById("BoardStateDisplay").innerHTML = generate_board_state();
}


/*
    TO-DO:
     - Remember castling rights
     - Remember whose move it is
*/
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
            state += val;
        }
        if(empty_count > 0) {
            state += empty_count;
            empty_count = 0;
        }
        if(row > 1) {
            state += "/";
        }
    }

    state += " "+turn+" KQkq - 0 1" // Still needs work
    return state;
}