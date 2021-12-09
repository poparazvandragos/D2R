screenSpecs = {
    width: 1280,
    height: 720,
    drawGrid: false,
    gridColor: "#FF0000",
    showMercItems: false,
    playerBackground: "background",
    mercBackground: "merc",
    changeBkImageLastElementId: "background"
};

captureSpecs = {
    width: 1920,
    height: 1080
};

d2rScreenSpecs = {
    cell: {
        sizeX: 48 / captureSpecs.width,
        sizeY: 48 / captureSpecs.height
    },
    inventory: {
        start: {
            x: 1268 / captureSpecs.width,
            y: 553 / captureSpecs.height
        },
        size: { x: 10, y: 4 },
        imgName: "inventory",
        addCellIdxToImg: true

    },
    stash: {
        start: {
            x: 164 / captureSpecs.width,
            y: 222 / captureSpecs.height
        },
        size: { x: 10, y: 10 },
        imgName: "s",
        addCellIdxToImg: true
    },
    equipment:
    {
        head: {
            start: {
                x: 1464 / captureSpecs.width,
                y: 197 / captureSpecs.height
            },
            size: { x: 2, y: 2 },
            imgName: "head",
            addCellIdxToImg: false
        },
        armor: {
            start: {
                x: 1464 / captureSpecs.width,
                y: 318 / captureSpecs.height
            },
            size: { x: 2, y: 3 },
            imgName: "armor",
            addCellIdxToImg: false
        },

        belt: {
            start: {
                x: 1464 / captureSpecs.width,
                y: 489 / captureSpecs.height
            },
            size: { x: 2, y: 1 },
            imgName: "belt",
            addCellIdxToImg: false
        },
        amulet: {
            start: {
                x: 1583 / captureSpecs.width,
                y: 281 / captureSpecs.height
            },
            size: { x: 1, y: 1 },
            imgName: "amulet",
            addCellIdxToImg: false
        },
        ring1: {
            start: {
                x: 1395 / captureSpecs.width,
                y: 489 / captureSpecs.height
            },
            size: { x: 1, y: 1 },
            imgName: "ring1",
            addCellIdxToImg: false
        },
        ring2: {
            start: {
                x: 1583 / captureSpecs.width,
                y: 489 / captureSpecs.height
            },
            size: { x: 1, y: 1 },
            imgName: "ring2",
            addCellIdxToImg: false
        },
        gloves: {
            start: {
                x: 1277 / captureSpecs.width,
                y: 440 / captureSpecs.height
            },
            size: { x: 2, y: 2 },
            imgName: "gloves",
            addCellIdxToImg: false
        },
        boots: {
            start: {
                x: 1653 / captureSpecs.width,
                y: 440 / captureSpecs.height
            },
            size: { x: 2, y: 2 },
            imgName: "boots",
            addCellIdxToImg: false
        },
        weapon: {
            start: {
                x: 1278 / captureSpecs.width,
                y: 222 / captureSpecs.height
            },
            size: { x: 2, y: 4 },
            imgName: "weapon",
            addCellIdxToImg: false
        },
        shield: {
            start: {
                x: 1653 / captureSpecs.width,
                y: 222 / captureSpecs.height
            },
            size: { x: 2, y: 4 },
            imgName: "shield",
            addCellIdxToImg: false
        },
    },
    merc: {
        head: {
            start: {
                x: 360 / captureSpecs.width,
                y: 197 / captureSpecs.height
            },
            size: { x: 2, y: 2 },
            imgName: "merchead",
            addCellIdxToImg: false
        },
        armor: {
            start: {
                x: 360 / captureSpecs.width,
                y: 318 / captureSpecs.height
            },
            size: { x: 2, y: 3 },
            imgName: "mercarmor",
            addCellIdxToImg: false
        },
        weapon: {
            start: {
                x: 172 / captureSpecs.width,
                y: 272 / captureSpecs.height
            },
            size: { x: 2, y: 4 },
            imgName: "mercweapon",
            addCellIdxToImg: false
        },
        shield: {
            start: {
                x: 549 / captureSpecs.width,
                y: 272 / captureSpecs.height
            },
            size: { x: 2, y: 4 },
            imgName: "mercshield",
            addCellIdxToImg: false
        },
    }
};

d2rPlayerSlotsSpecs = [
    d2rScreenSpecs.inventory,
    d2rScreenSpecs.stash,
    d2rScreenSpecs.equipment.head,
    d2rScreenSpecs.equipment.armor,
    d2rScreenSpecs.equipment.belt,
    d2rScreenSpecs.equipment.weapon,
    d2rScreenSpecs.equipment.shield,
    d2rScreenSpecs.equipment.ring1,
    d2rScreenSpecs.equipment.ring2,
    d2rScreenSpecs.equipment.armor,
    d2rScreenSpecs.equipment.boots,
    d2rScreenSpecs.equipment.gloves,
    d2rScreenSpecs.equipment.amulet
];

d2rMercSlotsSpecs = [
    d2rScreenSpecs.merc.head,
    d2rScreenSpecs.merc.armor,
    d2rScreenSpecs.merc.weapon,
    d2rScreenSpecs.merc.shield
];

d2rItemSlotsSpecs = d2rPlayerSlotsSpecs;

crtImg = "background";
lastErrImg = "";

addCanvasEvents = function () {
    var canvas = document.getElementById("gameCanvas");
    if (canvas) {
        canvas.onmousemove = (ev) => {
            var newImg = getBkImg(ev.offsetX, ev.offsetY);
            if (newImg) {
                if (crtImg != newImg)
                    if (newImg != lastErrImg) {
                        changeBkImage(newImg);
                    }
            }
        };
    }
};

changeBkImage = function (elementId, force) {

    imgId = "background";
    screenSpecs.changeBkImageLastElementId = elementId;

    var newSrc = "img/" + elementId + ".png";
    var img = document.getElementById(imgId);
    img.setAttribute("src", newSrc);

    img.onload = (ev) => {
        try {
            if (crtImg != elementId || force) {
                //console.log("changeBkImage:" + elementId);
                crtImg = elementId;
                var canvas = document.getElementById("gameCanvas");
                var img = document.getElementById(imgId);
                var ctx = canvas.getContext("2d");
                ctx.drawImage(img, 0, 0, screenSpecs.width, screenSpecs.height);
                drawGrid();
            }
        }
        catch (ex) {
        }
    };

    img.onerror = (ev) => {
        lastErrImg = elementId + "";
        //console.log("lastErrImg: " + lastErrImg);
        crtImg = screenSpecs.showMercItems ? screenSpecs.mercBackground : screenSpecs.playerBackground;
        changeBkImage(crtImg, force);
    }
};

drawGrid = function () {
    var canvas = document.getElementById("gameCanvas");
    if (canvas) {
        //DrawInventory
        if (screenSpecs.drawGrid) {
            //console.log("drawGrid()");
            for (var specIdx = 0; specIdx < d2rItemSlotsSpecs.length; specIdx++) {
                var crtSpec = d2rItemSlotsSpecs[specIdx];
                drawSpec(crtSpec, canvas);
            }
        }
    }
};

drawBackground = function () {
    var canvas = document.getElementById("gameCanvas");
    if (canvas) {

        canvas.width = screenSpecs.width;
        canvas.height = screenSpecs.height;
        var ctx = canvas.getContext("2d");
        var img = document.getElementById("background");
        ctx.drawImage(img, 0, 0, screenSpecs.width, screenSpecs.height);
    }
};

drawSpec = function (spec, canvas) {
    for (var rowIdx = 0; rowIdx < spec.size.x; rowIdx++)
        for (var colIdx = 0; colIdx < spec.size.y; colIdx++) {
            drawCell(canvas, getPos(spec, rowIdx, colIdx));
        }
};

getPos = function (spec, invx, invy) {
    return {
        x: spec.start.x + invx * d2rScreenSpecs.cell.sizeX + invx / captureSpecs.width,
        y: spec.start.y + invy * d2rScreenSpecs.cell.sizeY + invy / captureSpecs.height,
    };
};

getBkImg = function (mouseX, mouseY) {
    for (var specIdx = 0; specIdx < d2rItemSlotsSpecs.length; specIdx++) {
        var crtSpec = d2rItemSlotsSpecs[specIdx];

        for (var invX = 0; invX < crtSpec.size.x; invX++)
            for (var invY = 0; invY < crtSpec.size.y; invY++) {
                var startCellPos = getPos(crtSpec, invX, invY);
                var endCellPos = {
                    x: startCellPos.x + d2rScreenSpecs.cell.sizeX,
                    y: startCellPos.y + d2rScreenSpecs.cell.sizeY,
                };
                if (startCellPos.x <= mouseX / screenSpecs.width
                    && endCellPos.x > mouseX / screenSpecs.width
                    && startCellPos.y <= mouseY / screenSpecs.height
                    && endCellPos.y > mouseY / screenSpecs.height) {
                    if (crtSpec.addCellIdxToImg) {
                        return crtSpec.imgName + "" + invX + "" + invY;
                    }
                    else
                        return crtSpec.imgName;
                }
            }
    }
};

drawCell = function (canvas, position) {
    var ctx = canvas.getContext("2d");
    ctx.strokeStyle = screenSpecs.gridColor;
    ctx.beginPath();
    ctx.moveTo(position.x * screenSpecs.width, position.y * screenSpecs.height);
    ctx.lineTo((position.x + d2rScreenSpecs.cell.sizeX) * screenSpecs.width, position.y * screenSpecs.height);
    ctx.lineTo((position.x + d2rScreenSpecs.cell.sizeX) * screenSpecs.width, (position.y + d2rScreenSpecs.cell.sizeY) * screenSpecs.height);
    ctx.lineTo(position.x * screenSpecs.width, (position.y + d2rScreenSpecs.cell.sizeY) * screenSpecs.height);
    ctx.lineTo(position.x * screenSpecs.width, position.y * screenSpecs.height);
    ctx.stroke();
};

addSpecImages = function () {
    var imagesDiv = document.getElementById("imagesContainer");
    var lastImgId = "";
    for (var specIdx = 0; specIdx < d2rItemSlotsSpecs.length; specIdx++) {
        var crtSpec = d2rItemSlotsSpecs[specIdx];
        for (var invX = 0; invX < crtSpec.size.x; invX++)
            for (var invY = 0; invY < crtSpec.size.y; invY++) {
                var imgid = crtSpec.imgName;
                var imgsrc = imgid + ".png";
                if (crtSpec.addCellIdxToImg) {
                    imgid = crtSpec.imgName + "" + invX + "" + invY;
                    imgsrc = imgid + ".png";
                }

                if (lastImgId != imgid)
                    lastImgId = imgid;
                else continue;

                var img = document.createElement('img');
                img.setAttribute("id", imgid);
                img.setAttribute("src", imgsrc)
                imagesDiv.appendChild(img);

            }
    }
};

setIsLoading = function (isLoading) {
    var loadingDiv = document.getElementById("loadingDiv");
    var canvas = document.getElementById("gameCanvas");
    if (loadingDiv && canvas)
        if (isLoading) {
            loadingDiv.setAttribute("style", "text-align: center;width:" + screenSpecs.width + "px;height" + screenSpecs.height + "px;");
            canvas.setAttribute("style", "display:none");
        }
        else {
            loadingDiv.setAttribute("style", "display:none");
            canvas.setAttribute("style", "width:" + screenSpecs.width + "px;height" + screenSpecs.height + "px;");
        }
};

changeDrawGrid = function () {
    screenSpecs.drawGrid = !screenSpecs.drawGrid;
    changeBkImage(screenSpecs.changeBkImageLastElementId, true);
};

changeShowMercItems = function () {
    screenSpecs.showMercItems = !screenSpecs.showMercItems;
    if (screenSpecs.showMercItems) {
        changeBkImage(screenSpecs.mercBackground, true);
        d2rItemSlotsSpecs = d2rMercSlotsSpecs;
    }
    else {
        changeBkImage(screenSpecs.playerBackground, true);
        d2rItemSlotsSpecs = d2rPlayerSlotsSpecs;
    }
};

initialize = function () {
    drawGridCheckBox = document.getElementById("drawGridCheckBox");
    if (screenSpecs.drawGrid)
        drawGridCheckBox.setAttribute("checked", true);
    var canvas = document.getElementById("gameCanvas");
    if (canvas) {
        canvas.width = screenSpecs.width;
        canvas.height = screenSpecs.height;
    }
};

setTimeout(() => {
    setIsLoading(true);
    initialize();
    setTimeout(() => {
        addCanvasEvents();
        drawBackground();
        drawGrid();
        setTimeout(() => {
            setIsLoading(false);
        }, 100);
    }, 100);
}, 10);

