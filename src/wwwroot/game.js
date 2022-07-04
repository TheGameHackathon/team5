const field = document.getElementById("field");
const startMessage = document.getElementsByClassName("startMessage")[0];
const startgameOverlay = document.getElementsByClassName("start")[0];
const scoreElement = document.getElementsByClassName("scoreContainer")[0];
const startButton = document.getElementsByClassName("startButton")[0];
const exitButton = document.getElementById("exitButton");

let game = null;
let currentCells = {};
let isWaiting = false;
let spectatorMode = false;
let pollingTimeout = null;

async function handleSpectatePolling(){
    let resp = await fetch(`/api/games/${game.id}`,
        {
            method: "GET",
            headers: {
                "Content-Type": "application/json"
            },
        })
    if (resp.ok){
        game = await resp.json()
        renderField(game);
        pollingTimeout = setTimeout(handleSpectatePolling, 2000);
    }
    else{
        pollingTimeout = null;
    }
}

function handleApiErrors(result) {
    if (!result.ok) {
        alert(`API returned ${result.status} ${result.statusText}. See details in Dev Tools Console`);
        throw result;
    }
    return result.json();
}

async function startGame() {
    if (game && game.id){
        fetch(`/api/games/${game.id}`, { method: "DELETE", headers: {'content-type': 'application/json'}})
            .then(handleApiErrors);
    }
    spectatorMode = false;
    exitButton.classList.toggle("hidden", false);
    let diff = document.getElementById('difficult').value;
    game = await fetch("/api/games", { method: "POST", body: JSON.stringify({ difficult: diff }), headers: {'content-type': 'application/json'}})
        .then(handleApiErrors);
    window.history.replaceState(game.id, "The Game", "/" + game.id);
    renderField(game);
}

async function exitGame() {
    await fetch(`/api/games/${game.id}`, { method: "DELETE", headers: {'content-type': 'application/json'}})
    window.location = '/'
}
function SimpleLock(){
    this.locked = false;
    this.callbacks = [];
    this.acquire = function(withLock = true){
        if(this.locked){
            if (withLock)
                return new Promise(function(resolve){
                    this.callbacks.push(resolve);
                }.bind(this));
        } else {
            this.locked = true;
            return Promise.resolve(true);
        }
    }
    this.release = function(){
        if(this.callbacks.length > 0){
            let callback = this.callbacks.shift();
            callback(this);
        } else {
            this.locked = false;
        }
    }
}

let lock = new SimpleLock();

function makeMove(userInput) {
    if (!game || game.isFinished) return;
    isWaiting = true;
    console.log("send userInput: %o", userInput);
    lock.acquire().then(()=> {
        fetch(`/api/games/${game.id}/moves`,
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(userInput)
            })
            .then(handleApiErrors)
            .then(newGame => {
                game = newGame;
                updateField(game);
                isWaiting = false;
            });
        lock.release()
    });
}

function renderField(game) {
    field.innerHTML = "";
    for (let y = 0; y < game.height; y++) {
        const row = document.createElement("tr");
        for (let x = 0; x < game.width; x++) {
            const cell = document.createElement("td");
            cell.id = "td_" + x + "_" + y;
            cell.dataset.x = x;
            cell.dataset.y = y;
            cell.addEventListener("click", onCellClick);
            row.appendChild(cell);
        }
        field.appendChild(row);
    }
    updateField(game);
}

function updateField(game) {
    if (game) {
        scoreElement.innerText = `Your score: ${game.score}`;
        startMessage.innerText = `Your score: ${game.score}. Again?`;
    }
    setTimeout(
        () => {
            startgameOverlay.classList.toggle("hidden", !game.isFinished);
            startButton.focus();
        },
        300);

    const cells = game.cells;
    const existedCells = {};
    for (let newCell of cells) {
        if (newCell.id in currentCells) {
            moveCell(newCell);
        } else {
            createCell(newCell);
        }
        existedCells[newCell.id] = newCell;
    }
    for (var currentCell of Object.values(currentCells)) {
        if (!(currentCell.id in existedCells)) {
            deleteCell(currentCell);
        }
    }
    currentCells = existedCells;
}

function moveCell(cell) {
    const cellDiv = document.getElementById(cell.id);
    updateCellDiv(cellDiv, cell);
}

function createCell(cell) {
    let cellDiv = document.createElement("div");
    cellDiv.id = cell.id;
    cellDiv.addEventListener("click", onCellClick);
    updateCellDiv(cellDiv, cell);
    document.body.appendChild(cellDiv);
}

function deleteCell(cell) {
    let cellDiv = document.getElementById(cell.id);
    cellDiv.remove();
}

function updateCellDiv(cellDiv, cell) {
    const staticGridCell = document.getElementById(`td_${cell.pos.x}_${cell.pos.y}`);
    const rect = staticGridCell.getBoundingClientRect();
    cellDiv.dataset.x = cell.pos.x;
    cellDiv.dataset.y = cell.pos.y;
    cellDiv.style.width = rect.width;
    cellDiv.style.height = rect.height;
    cellDiv.style.top = rect.top + "px";
    cellDiv.style.left = rect.left + "px";
    cellDiv.style.zIndex = cell.zIndex;
    cellDiv.className = cell.type + " animated cell";
    cellDiv.innerText = cell.content;
}


function addKeyboardListener() {
    window.addEventListener("keydown",
        e => {
            if (game && game.monitorKeyboard) {
                makeMove({ keyPressed: e.keyCode });
                if (e.keyCode >= 37 && e.keyCode <= 40)
                    e.preventDefault();
            }
        });
};

function addResizeListener() {
    window.addEventListener("resize",
        () => updateField(game));
}

function onCellClick(e) {
    if (!game || !game.monitorMouseClicks || spectatorMode) return;
    const x = e.target.dataset.x;
    const y = e.target.dataset.y;
    makeMove({ clickedPos: { x, y } });
}

async function initializePage() {
    exitButton.addEventListener("click", exitGame);
    startButton.addEventListener("click", e => {
        startgameOverlay.classList.toggle("hidden", true);
        startGame();
    });
    const gameId = window.location.pathname.substring(1);
    startgameOverlay.classList.toggle("hidden", true);
    if (gameId){
        let resp = await fetch(`/api/games/${gameId}`,
            {
                method: "GET",
                headers: {
                    "Content-Type": "application/json"
                },
            })
        if (resp.ok){
            game = await resp.json()
            spectatorMode = true;
            addResizeListener();
            renderField(game);
            exitButton.classList.toggle("hidden", false);
            pollingTimeout = setTimeout(handleSpectatePolling, 2000);
            return;
        }
    }
    startgameOverlay.classList.toggle("hidden", false);
    // use gameId if you want

    addKeyboardListener();
    addResizeListener();
    startButton.focus();
}
window.addEventListener("load",()=>{
    initializePage();
});
