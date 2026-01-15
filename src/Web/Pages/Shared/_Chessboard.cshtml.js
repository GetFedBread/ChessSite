load_postition(null);

function load_postition(position) {
    if(position == null) {
        position = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    }

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

    document.getElementById("TurnDisplay").innerHTML = data[1];
}

function tile_clicked(tile) {
    //alert("tile clicked: "+tile);
    load_postition(null);
}