/// <reference path="//www.bing.com/api/maps/mapcontrol" />

var credentials = "AuDPGh7OdNXmPMm1r4XpiZP64gmrkG1rAliZ3ZTNW2ML6wUntAeFDr9e5z6M0VpE";

var map, searchManager, tools, rightClickHandler, chbkPrompt, yardStickLayer, radiusLayer, yardStickCount, pinLayer, drawingLayer, lblIDs,
    myPolygonOptions, infobox, ddlYear, chbkGOZ, drawingManager, bolDrawing, btnDrawingMode, pinID, currentShape;
var currentlyDrawing = 0;
var txtcommentsfocus = false;


function loadMapScenario() {
    map = new Microsoft.Maps.Map('#myMap', {
        credentials: credentials,
        center: new Microsoft.Maps.Location(37.6, -118.33), mapTypeId: Microsoft.Maps.MapTypeId.aerial,
        zoom: 6, enableClickableLogo: false, showCopyright: false, disableStreetside: true
    });
    Microsoft.Maps.loadModule("Microsoft.Maps.SpatialMath", function () {
        var displayField = $("[id*=showField]").val();
        processRequest();
        setMapView();
    });

    lblIDs = $("#lblIDs");

    pinLayer = new Microsoft.Maps.Layer();
     map.layers.insert(pinLayer);
       
    infobox = new Microsoft.Maps.Infobox(map.getCenter(), { visible: false });
    infobox.setMap(map);

    chbkPrompt = $("[id*=chbkPrompt]");
    bolDrawing = false;
    var myFillColor = new Microsoft.Maps.Color(100, 255, 165, 0);
    var myStrokeColor = new Microsoft.Maps.Color(200, 255, 165, 0);
    var myStrokeThickness = 5;

    myPolygonOptions = {
        fillColor: myFillColor,
        strokeColor: myStrokeColor,
        strokeThickness: myStrokeThickness
    };
    
}

function round(n) {
    number = new Number(n);
    return Math.round(number * 1000) / 1000;
}


function processRequest () {  
    pinLayer.clear();
    var data = JSON.parse($("#data").val());
    console.log(data);
    if(data.length ==0){
        alert("No pins found");
        return;
    }
    
    for(i=0; i<= data.length -1; i++) {
        var geoField = data[i].GeoField; 
        geoField = geoField.replace("POLYGON ((","").replace("))","").replace(/,/g,"").replace("POINT (","").replace(")","");

        var coords = new Array();
        coords = geoField.split(" ");        
        console.log(coords);

        if (coords[coords.length - 1] === "") {
            coords.pop();
        }

        var thisLocs = new Array();

        for (var k = 0; k <= coords.length - 1; k = k + 2) {
            var thisLoc = new Microsoft.Maps.Location(coords[k + 1], coords[k]);
            thisLocs.push(thisLoc);
        }
        var shape = null;

        switch (data[i].GeoType) {
            case "Point":
                shape = new Microsoft.Maps.Pushpin(thisLocs[0]);
                shape.centroid = Microsoft.Maps.SpatialMath.Geometry.centroid(shape);
                break;
            case "Polygon":
                shape = new Microsoft.Maps.Polygon(thisLocs, myPolygonOptions);
                shape.centroid = Microsoft.Maps.SpatialMath.Geometry.centroid(shape);
                break;
        }
        shape.title = data[i].Title;
        shape.description = data[i].Description;
        var pushpinOptions = { height: 28, width: 28, anchor: new Microsoft.Maps.Point(15, 15) };
        var pin = new Microsoft.Maps.Pushpin(Microsoft.Maps.SpatialMath.Geometry.centroid(shape), pushpinOptions);
        pin.title = shape.title;
        pin.description = shape.description;
        pin.centroid = shape.centroid;
        
        if (shape.geometryType != 1) {
            pinLayer.add(pin);
        }
        
    
        pinLayer.add(shape);

        Microsoft.Maps.Events.addHandler(pinLayer, 'mouseover', showLayerInfoBox);
        Microsoft.Maps.Events.addHandler(pinLayer, 'mouseout', hideInfoBox); 
        
        if (!lblIDs.is(':empty')) {                        
            var appId = data[i].AppId;
            lblIDs.text(lblIDs.text().replace(appId,""));
        }       
    }
}


function showInfoBox(e) {
    if (e.geometryType ==1 || e.geometryType==3) {
        infobox.setOptions({
            location: e.centroid,
            title: e.title,
            description: e.description,
            maxHeight: 300,
            maxWidth: 300,
            visible: true
        });
    }
}

function showLayerInfoBox(e) {
    showInfoBox(e.target);
}


function hideInfoBox() {
    infobox.setOptions({ visible: false });
}



function setMapView() {
    var pins = [];
    var shapes = pinLayer.getPrimitives();
    for (i = 0; i < shapes.length; i++) {
        var p = shapes[i];
        if (p.geometryType == 1) {
            pins.push(p.centroid);
        }
     };
    var viewRect = Microsoft.Maps.LocationRect.fromLocations(pins);
    map.setView({ bounds: viewRect });
    map.setView({ zoom: 9 });
}

