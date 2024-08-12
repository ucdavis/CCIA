let map;
let service;


async function initMap() {
    const { Map } = await google.maps.importLibrary("maps");    
    const { PlacesService } = await google.maps.importLibrary("places");
    const { DrawingService } = await google.maps.importLibrary("drawing");

   
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
        drawingMode: google.maps.drawing.OverlayType.MARKER,
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
        //path = path.replaceAll("(", "");
        alert(strCoord);
        $("#newPolygon").val(strCoord);
    });
}

initMap();

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


