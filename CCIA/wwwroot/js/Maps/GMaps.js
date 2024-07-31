let map;
let service;


async function initMap() {
    const { Map } = await google.maps.importLibrary("maps");    
    const { PlacesService } = await google.maps.importLibrary("places");

   
    map = new Map(document.getElementById("map"), {
        center: { lat: 37.6, lng:  - 118.33 },
        zoom: 6,
        streetViewControl: false,
        mapId: "CCIAMap",
        mapTypeId: "satellite",
    });

    service = new PlacesService(map);    
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


