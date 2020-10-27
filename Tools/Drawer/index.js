var fs = require('fs');
var gm = require('gm').subClass({ imageMagick: true });
var size = require('image-size');

const path = "./examples/"

iterateDirectory(path);

function iterateDirectory(path) {
    fs.readdir(path, (err, files) => {
        files.forEach(file => {
            if (file.includes(".txt")) {
                console.log(file);
                base = file.split(".");
                console.log(base[0] + ".png");
                readBoundingBoxes(path + file, path + base[0] + ".jpeg")
            }
        });
    });
}


function readBoundingBoxes(text_path, image_path) {
    fs.readFile(text_path, 'utf8', async function (err, data) {
        if (err) throw err;
        boxes = data.split('\n');

        for (let i = 0; i < 1; i++) {
            if (boxes[i].length > 10) {
                coords = boxes[i].split(" ");
                await drawBoundingBox(image_path, coords[1], coords[2], coords[3], coords[4]);
            }
        }
    });
}


function drawBoundingBox(image_path, xc, yc, nw, nh) {
    return new Promise((resolve, reject) => {
        size(image_path, function (err, dim) {
            width = nw * dim.width;
            height = nh * dim.height;
            tlx = xc * dim.width - width / 2;
            tly = yc * dim.height - height / 2;
            brx = xc * dim.width + width / 2;
            bry = yc * dim.height + height / 2;

            //console.log(dim.width, dim.height);
            console.log(width);
            console.log(height);
            console.log(tlx);
            console.log(tly);
            console.log(brx);
            console.log(bry);
            console.log("--------------");

            gm(image_path)
                .stroke("#ffffff")
                .fill("rgba( 255, 255, 255 , 0 )")
                .drawRectangle(3, 156, 228, 253)
                .write(image_path, function (err) {
                    console.log(err)
                    if (!err) console.log('done');
                    resolve();
                });
        });
    });
}


/* fs.readdir("./examples", (err, files) => {
    files.forEach(file => {
        if (file.includes(".txt")) {
            console.log(file);
            gm('./examples/image_4.png')
                .stroke("#ffffff")
                .fill("rgba( 255, 255, 255 , 0 )")
                .drawRectangle(100, 100, 200, 200)
                .write("./examples/image_4.png", function (err) {
                    console.log(err)
                    if (!err) console.log('done');
                });
        }
    });
});

fs.readFile('./examples/image_4.txt', 'utf8', function(err, data) {
    if (err) throw err;
    console.log(data);
    console.log(data.split('\n'));
}); */