let map;
let service;
let infoWindow;


async function initMap() {
    const { Map } = await google.maps.importLibrary("maps");    
    const { PlacesService } = await google.maps.importLibrary("places");
    const { DrawingService } = await google.maps.importLibrary("drawing");
    const { AdvancedMarkerElement } = await google.maps.importLibrary("marker");
    const { LatLngBounds } = await google.maps.importLibrary("core");

   
    map = new Map(document.getElementById("map"), {
        center: { lat: 37.6, lng:  - 118.33 },
        zoom: 6,
        streetViewControl: false,
        mapId: "CCIAMap",
        mapTypeId: google.maps.MapTypeId.HYBRID,
        labels: true,
    });

    service = new PlacesService(map);   
    const drawingManager = new google.maps.drawing.DrawingManager({
        
        drawingControl: true,
        drawingControlOptions: {
            position: google.maps.ControlPosition.LEFT_TOP,
            drawingModes: [                
                google.maps.drawing.OverlayType.POLYGON,                
            ],
        },
        markerOptions: {
            icon: "https://developers.google.com/maps/documentation/javascript/examples/full/images/beachflag.png",
        },
        circleOptions: {
            fillColor: "#ffff00",
            fillOpacity: 1,
            strokeWeight: 5,
            clickable: false,
            editable: true,
            zIndex: 1,
        },
    });

    drawingManager.setMap(map);

    drawingManager.addListener("drawingmode_changed", function () {
        alert("Please double click on your final point to close the field (don't do this close to the first point). Remember: Field must be drawn in a clockwise direction.");        
    });

    google.maps.event.addListener(drawingManager, "polygoncomplete", function (polygon) {
       
        var path = polygon.getPath().getArray();
        //alert(path.reverse())
        path = path.reverse();
        var strCoord = "";   
        var first = "";
        path.forEach(function (t, num) {
            if (num === 0) {
                first = t.lng() + ' ' + t.lat();
            }
            strCoord = strCoord + ' ' + t.lng() + ' ' + t.lat() + ', ';
        });
        strCoord = strCoord + first;        
        //alert(strCoord);
        $("#newPolygon").val(strCoord);
    });
    processRequest();
}

initMap();

function processRequest() {
    var data = JSON.parse($("#data").val());
    //console.log(data);
    var bounds = new google.maps.LatLngBounds();
    if (data.length == 0) {
        alert("No pins found");
        return;
    }
    
    for (i = 0; i <= data.length - 1; i++) {
        if (data[i].GeoType === "Polygon") {
            var geoField = data[i].GeoField;
            geoField = geoField.replace("POLYGON ((", "").replace("))", "").replace(/,/g, "").replace("POINT (", "").replace(")", "");
            var coords = new Array();
            coords = geoField.split(" ");
            //console.log(coords);

            var thisLocs = new Array();

            for (var k = 0; k <= coords.length - 1; k = k + 2) {
                var thisLoc = new google.maps.LatLng(coords[k + 1], coords[k]);
                bounds.extend(thisLoc);
                thisLocs.push(thisLoc);
            }
            thisLocs.pop();
            //console.log(thisLocs);
            //alert(getCenterFromLoc(thisLocs));

            const newPin = new google.maps.Polygon({
                paths: thisLocs,
                strokeColor: "#ffa600",
                strokeOpacity: 1,
                strokeWeight: 2,
                fillColor: "#ffa600",
                fillOpacity: 0.50,
                title: data[i].Title,
                description: data[i].Description,
                center: getCenterFromLoc(thisLocs),
            });
            newPin.setMap(map);
            newPin.addListener("mouseover", showInfo);
            newPin.addListener("mouseout", removeInfo);
            infoWindow = new google.maps.InfoWindow();
            
        }
        map.fitBounds(bounds);
    }
}

function showInfo(event) {
    //var shapeBounds = getCenter(this);
    //alert(shapeBounds.getCenter());
    //alert(this.center);    
    var content = "<b>" + this.title + "</b><br>" + this.description   
    infoWindow.setContent(content);
    infoWindow.setPosition(this.center);
    infoWindow.open(map);    
}

function removeInfo(event) {
    infoWindow.close();
}

function getCenterFromLoc(shape) {
    var thisBounds = new google.maps.LatLngBounds();
    shape.forEach(function (element, index) { thisBounds.extend(element) })
    return thisBounds.getCenter();
}

function Panmap() {
    //Davis 38.67 -121.75
    var x = $("#txtLat").val();
    var y = $("#txtLong").val();
    if (isNaN(x) || x < 32 || x > 42) {
        alert('Latitude must be a number between 32 and 42');
    }
    else
        if (isNaN(y) || y < -125 || y > -114) {
            alert('Longitude must be a number between -125 and -114');
        }
        else {
            map.setCenter(new google.maps.LatLng(x, y));
            map.setZoom(14);            
        }
};

function zoom_county() {
    var county = $("[id*=county_name").val() + " county california"; 
    const countyRequest = {
        query: county,
        fields: ["name", "geometry"],
    };
    service.findPlaceFromQuery(countyRequest, (results, status) => {
        if (status === google.maps.places.PlacesServiceStatus.OK && results) {
            map.setCenter(results[0].geometry.location);
            map.setZoom(10);
        }
    });
    
};


