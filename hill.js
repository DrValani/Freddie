
var segments = [];
var path = [];

var factor = 1025.0/(Math.PI * 2229.0);
function scale(pos){
  return [pos[0]*factor, pos[1]*factor];
}

function parseLine(line){
  // 3,053m (4631.813323, 1255.323318) step: 785.012411
  var start = line.indexOf("(")
  var end = line.indexOf(")");
  var pos = line.slice(start+1, end);
  return pos.split(',').map(parseFloat);
}

function data2segments(data){
  var lines = data.split('\n');
  var positions = lines.map(parseLine);
  return _.zip(_.slice(positions,0, positions.length-1), _.slice(positions, 1));
}

function data2positions(data){
  var lines = data.split('\n');
  return lines.map(parseLine);
}

function Mars(p){
  var width = 1025;
  var height = 1025;
  var landscape;

    // Attach functions to processing object
    p.setup = function () {
        p.size(width, height);
         landscape = p.loadImage("./fixed.png");

    };

    p.draw = function () {
      p.image(landscape, 0,0);
      p.stroke(0,255,150);
      p.strokeWeight(4);
      p.noFill();
      p.beginShape();
      path.forEach(function(pos, i){
        pos = scale(pos);
        var x = width-pos[0];
        var y = height-pos[1];
        p.vertex(x, y);
        if (i==0){
          p.ellipse(x,y,20,20);
        }else if(i == (path.length-1)){
          p.endShape();
          p.fill(255,255,255);
          p.stroke(150,0,150);
          p.triangle(x+10,y+10,x-10,y+10,x,y-14.14);
        }else{
          p.ellipse(x,y,5,5);
        }
      });
    };
}

function loadData(data){
  segments = data2segments(data);
  path = data2positions(data);
  console.log(""+path.length+" data points loaded");
}

$(function(){
  var elem = document.getElementById("sketch");
  var processingInstance = new Processing(elem, Mars);
  $("#inputbox").on('input propertychange change', function(){
    loadData($(this).val());
  });
})
